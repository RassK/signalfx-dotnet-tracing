using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Samples.AspNetMvc5.Controllers
{
    public class HomeController : Controller
    {
        [ValidateInput(false)]
        public ActionResult Index()
        {
            var prefixes = new[] { "COR_", "CORECLR_", "SIGNALFX_", "DATADOG_" };

            var envVars = from envVar in Environment.GetEnvironmentVariables().Cast<DictionaryEntry>()
                          from prefix in prefixes
                          let key = (envVar.Key as string)?.ToUpperInvariant()
                          let value = envVar.Value as string
                          where key.StartsWith(prefix)
                          orderby key
                          select new KeyValuePair<string, string>(key, value);

            return View(envVars.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Shutdown()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.DefinedTypes)
                {
                    if (type.Namespace == "Coverlet.Core.Instrumentation.Tracker")
                    {
                        var unloadModuleMethod = type.GetMethod("UnloadModule", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                        unloadModuleMethod.Invoke(null, new object[] { this, EventArgs.Empty });
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}
