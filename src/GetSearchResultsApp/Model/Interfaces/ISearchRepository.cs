using System.Threading.Tasks;
using GetSearchResultsApp.ViewModels;

namespace GetSearchResultsApp.Model.Interfaces
{
    public interface ISearchRepository
    {
        /// <summary>
        /// Returns a Search Response View Model for the passed in Search Request.
        /// Search response entity helps to keep the view separate from the servie entity.
        /// This method handles errors gracefully and does not throw.
        /// </summary>
        /// <param name="searchRequest">The <see cref="SearchRequest"/>.</param>
        /// <returns>A task which upon completion results in the mapped <see cref="SearchResponse"/>.</returns>
        Task<SearchResponse> SearchAsync(SearchRequest searchRequest);
    }
}
