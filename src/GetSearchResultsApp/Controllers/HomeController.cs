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
            if (!ModelState.IsValid)
            {
                var errorResponse = new SearchResponse
                {
                    ErrorMessages = ModelState.Values.SelectMany(m => m.Errors)
                        .Where(e => e != null).Select(e => e.ErrorMessage)
                        .ToArray()
                };
                return Json(errorResponse);
            }

            var response = await _searchRepository.SearchAsync(request);

            return Json(response);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}