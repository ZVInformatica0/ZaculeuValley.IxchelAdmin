using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using ZaculeuValley.IxchelAdmin.Models;

namespace ZaculeuValley.IxchelAdmin.Controllers
{
    public class HomeController : Controller
    {
        

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var controllers = GetAllControllers();
            var filteredControllers = controllers.Where(c => c.Name != "HomeController").ToList();

            var viewModelList = filteredControllers.Select(c => new ControllerListViewModel
            {
                Name = c.Name,
                ApiUrl = Url.Action("Index", c.Name.Replace("Controller", ""), null, Request.Scheme, Request.Host.Value)
            }).ToList();

            return View(viewModelList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private List<Type> GetAllControllers()
        {
            var assembly = Assembly.GetExecutingAssembly(); // Get the assembly where your controllers are located

            var controllerTypes = assembly.GetTypes()
                .Where(t => typeof(Controller).IsAssignableFrom(t) && t.Name.EndsWith("Controller"))
                .ToList();

            return controllerTypes;
        }
    }
}