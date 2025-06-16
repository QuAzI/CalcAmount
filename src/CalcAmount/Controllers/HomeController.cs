using System.Web.Mvc;

namespace CalcAmount.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var httpClient = new HttpClient()
            //{
            //    BaseAddress = new Uri("https://api.frankfurter.dev/v1/"),
            //};

            //using (HttpResponseMessage response = await httpClient.GetAsync(path))
            //{
            //    response.EnsureSuccessStatusCode();

            //    var jsonResponse = await response.Content.ReadAsStringAsync();

            //    Directory.CreateDirectory("c:\\temp\\cache");
            //    File.WriteAllText(cache, jsonResponse);
            //}

            ViewBag.Currencies = new[] { "USD", "CAN" };
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}