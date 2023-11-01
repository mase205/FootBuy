using System.Diagnostics;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using project.Models;

namespace project.Controllers {
    public class HomeController : Controller
{
    private readonly AppDbContext _context;
    private readonly string url = "http://api.exchangeratesapi.io/v1/latest?access_key=c14cd501e0dcee9a14fb24ad2da65753&base=EUR&symbols=ILS,NIS";
    
    CookieOptions options = new CookieOptions();

    public HomeController(AppDbContext context)
    {
        _context = context;
        options.Expires = DateTime.Now.AddDays(7);
    }

    public ActionResult Index()
    {
        if(Request.Cookies["CookieIdentifier"] == null) {
            Response.Cookies.Append("CookieIdentifier", "", options);
            Response.Cookies.Append("CookiePassword", "", options);
        }
        var PlayerList = _context.Players.ToList();
        ViewBag.PlayerList = PlayerList;
        ViewBag.Rate = AsyncGetRate();
        return View();
    }

    public ActionResult Privacy()
    {
        return View();
        
    } 

    public ActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<double> AsyncGetRate () {
        using (var cli = new HttpClient()) {
            HttpResponseMessage request = await cli.GetAsync(url);
            var content = await request.Content.ReadAsStringAsync();
            var json = JsonNode.Parse(content);
            double rate = double.Parse(json["rates"]["ILS"].ToString());
            return rate;
        }
    }

    public ActionResult SearchBar() {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Search(string name, string club, string country, int? pricemin, int? pricemax) {
        pricemin??=-1;
        pricemax??=int.MaxValue;
        name??="";
        country??="";
        club??="";
        var SearchList = _context.Players.AsEnumerable().Where(prop => 
            prop.Name.Normalize().ToLower().Contains(name.ToLower()) && 
            prop.Club.Normalize().ToLower().Contains(club.ToLower()) &&
            prop.Country.Normalize().ToLower().Contains(country.ToLower()) && 
            prop.Price > pricemin/1000000 && 
            prop.Price < pricemax/1000000
        );
         Console.WriteLine(SearchList.ToString());
         ViewBag.PlayerList=SearchList;
         ViewBag.Rate = AsyncGetRate();
        return View();
    }
}

}

