using FreeCourse.Web.Exceptions;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interface;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FreeCourse.Web.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        private readonly ICatalogService _catalogService;

        public HomeController(ILogger<HomeController> logger, ICatalogService catalogService) {
            _logger = logger;
            _catalogService = catalogService;
        }

        public async Task<IActionResult> Index() {
            return View(await _catalogService.GetAllCourseAsync());
        }

        public async Task<IActionResult> Detail(string id) {
            return View(await _catalogService.GetByCourseId(id));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            //fırlatılan hatanın yakalanması 
            var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            //fırlatılan hata unauth hatası ise bu bizim fırlattığımız hata oluyor 
            if (errorFeature != null && errorFeature.Error is UnAuthorizeException) {
                //hata unauth ise logout yap
                return RedirectToAction(nameof(AuthController.Logout), "Auth");
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}