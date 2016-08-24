using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GetSearchResultsApp.Model.Interfaces;
using GetSearchResultsApp.ViewModels;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GetSearchResultsApp.Model
{
    public class HttpSearchRequestProcessor : ISearchRequestProcessor
    {
        private const string WhiteSpace = " ";
        private const string ZwsIdParamName = "zws-id";
        private const string AddressParamName = "address";
        private const string CityStateZipParamName = "citystatezip";
        private const string ZestimateRentParamName = "rentzestimate";

        private readonly IHttpClientWrapper _httpClient;
        private readonly ILogger<HttpSearchRequestProcessor> _logger;
        private readonly ZillowServiceSettings _zillowServiceSettings;

        public HttpSearchRequestProcessor(
            IOptions<ZillowServiceSettings> options,
            IHttpClientWrapper httpClient,
            ILogger<HttpSearchRequestProcessor> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _zillowServiceSettings = options.Value;
        }

        public async Task<string> SearchAsync(SearchRequest searchRequest)
        {
            if (searchRequest == null)
            {
                throw new ArgumentNullException(nameof(searchRequest));
            }

            var queryParameters = new Dictionary<string, string>
            {
                {ZwsIdParamName, _zillowServiceSettings.ZwsId},
                {AddressParamName, searchRequest.AddressLine},
                {CityStateZipParamName, GetCityStateZipQueryStringParameter(searchRequest)},
                {ZestimateRentParamName, searchRequest.ZestimateRent.ToString()},
            };

            var url = QueryHelpers.AddQueryString(_zillowServiceSettings.SearchServiceUrl, queryParameters);

            var response = await _httpClient.GetAsync(new Uri(url));

            try
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError(SearchEvents.HttpRequestFailed, exception, "Http Request failed");
                throw;
            }
        }

        private static string GetCityStateZipQueryStringParameter(SearchRequest searchRequest)
        {
            /**return Possibilities
             *  ""
             *  "Irvine"
             *  "Irvine CA"
             *  "CA"
             *  "Irvine, 92618"
             *  "Irvine CA, 92618"
             *  "CA, 92618"
             *  "92618"            
             **/


            var hasCity = !string.IsNullOrWhiteSpace(searchRequest.City);
            var hasState = !string.IsNullOrWhiteSpace(searchRequest.State);
            var hasZipCode = !string.IsNullOrWhiteSpace(searchRequest.ZipCode);

            if (!(hasCity || hasState || hasZipCode))
            {
                return string.Empty;
            }

            if (!(hasCity || hasState))
            {
                return searchRequest.ZipCode;
            }

            var builder = new StringBuilder();

            if (hasCity)
            {
                builder.Append(searchRequest.City);
                if (hasState)
                {
                    builder.Append(WhiteSpace);
                }
            }

            if (hasState)
            {
                builder.Append(searchRequest.State);
            }

            if (hasZipCode)
            {
                builder.Append($", {searchRequest.ZipCode}");
            }

            return builder.ToString();
        }
    }
}