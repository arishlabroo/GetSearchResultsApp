using System;
using System.Threading.Tasks;
using GetSearchResultsApp.Model.Interfaces;
using GetSearchResultsApp.ViewModels;
using Microsoft.Extensions.Logging;

namespace GetSearchResultsApp.Model
{
    /// <seealso cref="GetSearchResultsApp.Model.Interfaces.ISearchRepository" />
    public class SearchRepository : ISearchRepository
    {
        private readonly ISearchRequestProcessor _requestProcessor;
        private readonly ISearchResponseMapper _responseMapper;
        private readonly ILogger<SearchRepository> _logger;

        public SearchRepository(
            ISearchRequestProcessor requestProcessor,
            ISearchResponseMapper responseMapper,
            ILogger<SearchRepository> logger)
        {
            _requestProcessor = requestProcessor;
            _responseMapper = responseMapper;
            _logger = logger;
        }

        public async Task<SearchResponse> SearchAsync(SearchRequest searchRequest)
        {
            if (searchRequest == null)
            {
                return FailedResponse("Invalid Request");
            }

            var rawResponse = await GetRawResponse(searchRequest);

            if (string.IsNullOrWhiteSpace(rawResponse))
            {
                return FailedResponse("Processing Error");
            }

            _logger.LogInformation(SearchEvents.RawResponse, rawResponse);

            var mappedResponse = GetMappedResponse(rawResponse);

            if (mappedResponse == null)
            {
                return FailedResponse("Parsing Error");
            }

            return mappedResponse;
        }

        private async Task<string> GetRawResponse(SearchRequest searchRequest)
        {
            var rawResponse = string.Empty;

            try
            {
                rawResponse = await _requestProcessor.SearchAsync(searchRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(SearchEvents.RequestProcessing, e, "An error occured while processing");
            }

            return rawResponse;
        }

        private SearchResponse GetMappedResponse(string rawResponse)
        {
            SearchResponse searchResponse = null;
            try
            {
                searchResponse = _responseMapper.MapResponse(rawResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(SearchEvents.ResponseMapping, e, "An error occured while mapping response");
            }

            return searchResponse;
        }

        private static SearchResponse FailedResponse(string errorMessage)
        {
            return new SearchResponse
            {
                Success = false,
                ErrorMessages = new[] {errorMessage}
            };
        }
    }
}