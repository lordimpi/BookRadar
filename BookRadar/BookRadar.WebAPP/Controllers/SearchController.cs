using BookRadar.Bussiness.Service.OpenLibrary;
using BookRadar.WebAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace BookRadar.WebAPP.Controllers
{
    public class SearchController : Controller
    {
        private readonly IOpenLibraryService _openLibraryService;
        private readonly ILogger<SearchController> _logger;

        public SearchController(IOpenLibraryService openLibraryService, ILogger<SearchController> logger)
        {
            _openLibraryService = openLibraryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string author = "", int page = 1, int pageSize = 10)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar la búsqueda de autor '{Author}' en página {Page}.", author, page);
                return StatusCode(500, "Ocurrió un error al procesar la búsqueda.");
            }
        }

        [HttpPost]
        public IActionResult Index(SearchViewModel form)
        {
            try
            {
                return RedirectToAction(nameof(Index), new { author = form.Author, page = 1, pageSize = form.PageSize });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar el formulario de búsqueda.");
                return StatusCode(500, "Ocurrió un error al procesar el formulario.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> IndexHistorialBusqueda(int page = 1, int pageSize = 10)
        {
            try
            {
                var historial = await _openLibraryService.ObtenerHistorialAsync(page, pageSize);

                var model = new HistorialViewModel
                {
                    Items = historial.Items,
                    PageNumber = page,
                    PageSize = pageSize,
                    TotalRows = historial.TotalItems
                };

                if (Request.Headers.TryGetValue("X-Requested-With", out var h) && h == "XMLHttpRequest")
                    return PartialView("_HistorialTable", model);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el historial de búsquedas. Página {Page}", page);
                return StatusCode(500, "Ocurrió un error al obtener el historial.");
            }
        }
    }
}
