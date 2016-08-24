using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GetSearchResultsApp.Model.Interfaces
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> GetAsync(Uri uri);
    }
}