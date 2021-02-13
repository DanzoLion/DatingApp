using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FallbackController : Controller          // base class controller with MVC support // where angular app is the view of our applicaton // from indext the routes will be served
    {
        public ActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/HTML");     // we then instruct our Startup.cs where to fall back to this controller
        }
    }
}