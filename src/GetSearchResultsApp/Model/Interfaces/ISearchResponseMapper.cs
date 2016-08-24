using GetSearchResultsApp.ViewModels;

namespace GetSearchResultsApp.Model.Interfaces
{
    public interface ISearchResponseMapper
    {
        SearchResponse MapResponse(string response);
    }
}