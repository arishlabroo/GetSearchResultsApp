using System;
using System.Net.Http;
using System.Threading.Tasks;
using GetSearchResultsApp.Model.Interfaces;

namespace GetSearchResultsApp.Model
{
    /// <seealso cref="GetSearchResultsApp.Model.Interfaces.IHttpClientWrapper" />
    public class HttpClientWrapper : IHttpClientWrapper
    {
        public async Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            using (var client = new HttpClient())
            {
                return await client.GetAsync(uri);
            }
        }
    }
}