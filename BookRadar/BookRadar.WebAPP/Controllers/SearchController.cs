using BookRadar.Bussiness.Service.OpenLibrary;
using BookRadar.WebAPP.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookRadar.WebAPP.Controllers
{
    public class SearchController : Controller
    {
        private readonly IOpenLibraryService _openLibraryService;

        public SearchController(IOpenLibraryService openLibraryService)
        {
            _openLibraryService = openLibraryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string author = "", int page = 1, int pageSize = 10)
        {
            var vm = new SearchViewModel { Author = author, Page = page, PageSize = pageSize };

            if (!string.IsNullOrWhiteSpace(author))
            {
                var result = await _openLibraryService.SearchByAuthorAsync(author, page, pageSize);
                vm.Items = result.Items;
                vm.TotalItems = result.TotalItems;
                vm.TotalPages = result.TotalPages;
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_Resultados", vm);
            }

            return View(vm);
        }

        [HttpPost]
        public IActionResult Index(SearchViewModel form)
        {
            return RedirectToAction(nameof(Index), new { author = form.Author, page = 1, pageSize = form.PageSize });
        }
    }
}
