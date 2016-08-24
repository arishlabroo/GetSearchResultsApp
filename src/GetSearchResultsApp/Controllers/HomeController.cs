using System.Threading.Tasks;
using GetSearchResultsApp.Model.Interfaces;
using GetSearchResultsApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace GetSearchResultsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISearchRepository _searchRepository;

        public HomeController(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        public ViewResult Index()
        {
            return View();
        }

        public async Task<JsonResult> Search(SearchRequest request)
        {
            SearchResponse response;
            if (ModelState.IsValid)
            {
                response = await _searchRepository.SearchAsync(request);
            }
            else
            {
                response = new SearchResponse
                {
                    ErrorMessages = ModelState?.SelectMany(m => m.Value.Errors)?.Select(e => e.ErrorMessage)?.ToArray()
                };
            }

            return Json(response);
        }

        public IActionResult Error()
        {
            return View();
        }

    }
}