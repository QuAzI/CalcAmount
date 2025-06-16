using CalcAmount.Services;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CalcAmount.Controllers
{
    public class HomeController : Controller
    {
        public ICurrenciesService CurrenciesService { get; }

        public HomeController(ICurrenciesService currenciesService)
        {
            CurrenciesService = currenciesService;
        }

        public async Task<ActionResult> Index()
        {
            ViewBag.Currencies = await CurrenciesService.GetCurrenciesAsync();
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