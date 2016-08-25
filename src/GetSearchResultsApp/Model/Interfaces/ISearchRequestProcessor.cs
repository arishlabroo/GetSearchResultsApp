using System.Threading.Tasks;
using GetSearchResultsApp.ViewModels;

namespace GetSearchResultsApp.Model.Interfaces
{
    public interface ISearchRequestProcessor
    {
        /// <summary>
        /// Perform search asynchronously.
        /// </summary>
        /// <param name="searchRequest">The <see cref="SearchRequest"/>.</param>
        /// <returns>A task which upon completion results in the raw response processed for the provided SearchRequest.</returns>
        Task<string> SearchAsync(SearchRequest searchRequest);
    }
}