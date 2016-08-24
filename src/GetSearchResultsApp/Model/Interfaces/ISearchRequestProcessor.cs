using System.Threading.Tasks;
using GetSearchResultsApp.ViewModels;

namespace GetSearchResultsApp.Model.Interfaces
{
    public interface ISearchRequestProcessor
    {
        Task<string> SearchAsync(SearchRequest searchRequest);
    }
}