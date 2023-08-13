using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VirtuaConecta.Wms.UI.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Inicio()
        {
            return View();
        }
    }
}
