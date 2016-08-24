using System.Threading.Tasks;
using GetSearchResultsApp.ViewModels;

namespace GetSearchResultsApp.Model.Interfaces
{
    public interface ISearchRepository
    {
        Task<SearchResponse> SearchAsync(SearchRequest searchRequest);
    }
}
