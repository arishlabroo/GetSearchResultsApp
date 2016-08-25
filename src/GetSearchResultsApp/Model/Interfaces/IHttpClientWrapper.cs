using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GetSearchResultsApp.Model.Interfaces
{
    /// <summary>
    /// A simple pass through wrapper interface for HttpClient.
    /// This interface decouples the consumer from HttpClient and enhaces testability
    /// HttpClient is not easy to mock. 
    /// </summary>
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> GetAsync(Uri uri);
    }
}