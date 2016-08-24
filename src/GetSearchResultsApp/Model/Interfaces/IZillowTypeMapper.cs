using GetSearchResultsApp.ViewModels;

namespace GetSearchResultsApp.Model.Interfaces
{
    public interface IZillowTypeMapper
    {
        SearchResponse MapSearchResponse(searchresults searchresults);
    }
}