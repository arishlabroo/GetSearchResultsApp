using GetSearchResultsApp.ViewModels;

namespace GetSearchResultsApp.Model.Interfaces
{
    public interface ISearchResponseMapper
    {
        /// <summary>
        /// Maps the provided raw response into the search response view model.
        /// </summary>
        /// <param name="response">The raw response.</param>
        /// <returns>Fully mapped <see cref="SearchResponse"/></returns>
        SearchResponse MapResponse(string response);
    }
}