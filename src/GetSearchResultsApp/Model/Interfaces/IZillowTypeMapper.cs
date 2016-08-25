using GetSearchResultsApp.ViewModels;

namespace GetSearchResultsApp.Model.Interfaces
{
    public interface IZillowTypeMapper
    {
        /// <summary>
        /// Maps the zillow service entity into our own view model entity.
        /// </summary>
        /// <param name="searchresults">The Zillow service <see cref="searchresults"/> entity.</param>
        /// <returns>The <see cref="SearchResponse"/> viewmodel entity</returns>
        SearchResponse MapSearchResponse(searchresults searchresults);
    }
}