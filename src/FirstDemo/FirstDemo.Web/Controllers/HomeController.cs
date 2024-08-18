using FirstDemo.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FirstDemo.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISmsSender _smsSender;

        public HomeController(ILogger<HomeController> logger, 
            ISmsSender smsSender)
        {
            _logger = logger;
            _smsSender = smsSender;
        }

        public IActionResult Index()
        {
            var model = new IndexModel();
            model.Message = "Hello World";
            _logger.LogInformation("I am in index");
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Test()
        {
            var model = new TestModel();
            model.Email = "jalaluddin@gmail.com";
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Test(TestModel model)
        {
            if(ModelState.IsValid)
            {
                // code to write in future
                int x = 5;
            }
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}