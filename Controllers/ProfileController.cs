using Microsoft.AspNetCore.Mvc;
using project.Models;

namespace project.Controllers {
    public class ProfileController : Controller
{
    private readonly AppDbContext _context;
    
    CookieOptions options = new CookieOptions();

    public ProfileController(AppDbContext context)
    {
        _context = context;
        options.Expires = DateTime.Now.AddDays(7);
    }

    public ActionResult Index () {
        var FindUser = UserProps();
        if (FindUser != null) {
            ViewBag.user = FindUser;
        }
        else {
            ViewBag.user = null;
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult ChangeFieldsUnlocked()
    {
        var FindUser = UserProps();
        if (FindUser != null) {
            ViewBag.user = FindUser;
        }
        else {
            ViewBag.user = null;
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult ChangeFieldsLocked(string username, string email, string club)
    {
        var FindUser = UserProps();
        if (FindUser is not null) {
            FindUser.Username = username;
            FindUser.Email = email;
            FindUser.Club = club;
            Response.Cookies.Append("CookieIdentifier", username, options);
            _context.Users.Update(FindUser);
            _context.SaveChanges();
            ViewBag.user = FindUser;
        }
        else {
            ViewBag.user = null;
        }
        return View();
    }

     public ActionResult Logout(){
        if (!string.IsNullOrEmpty(Request.Cookies["CookieIdentifier"])) {
            Response.Cookies.Delete("CookieIdentifier");
            Response.Cookies.Delete("CookiePassword");
        }
        return Redirect("/");
    }

     public ActionResult ViewWishlist() {
        var FindUser = UserProps();
         if (FindUser is not null) {
            ViewBag.User = FindUser;
            var listings = _context.Wishlists.Where(prop => prop.UserID == FindUser.UID);
            List<Player> players = new() { };
            foreach (var listing in listings) {
                var player = _context.Players.FirstOrDefault(prop => prop.PlayerID == listing.PlayerID);
                if (player is not null) {
                    players.Add(player);
                }
            }
            ViewBag.WishlistPlayers = players;
        }
        return View();
     }

     public ActionResult ListPlayer() {
        var FindUser = UserProps();
        ViewBag.Club = FindUser.Club;
         int RequestedPID = -1;
        Player? _player = null;
        if(TempData["rPID"] is not null) {
            RequestedPID = int.Parse(TempData["rPID"].ToString());
            _player = _context.Players.FirstOrDefault(prop => prop.PlayerID == RequestedPID);
        }
        ViewBag.player = _player;
        TempData["rPID"] = RequestedPID;
        return View();
     }

     [HttpPost]
     [ValidateAntiForgeryToken]

     public ActionResult ListPlayerExtend(string name, string country, int? age, int? price) {
        var user = UserProps();
        int RequestedPID = -1;
        Player? _player = null;
        if(TempData["rPID"] is not null) {
            RequestedPID = int.Parse(TempData["rPID"].ToString());
            _player = _context.Players.FirstOrDefault(prop => prop.PlayerID == RequestedPID);
        }
        if (user is not null && name is not null && age is not null && country is not null && price is not null) {
            Player player = new Player{Name=name, Club=user.Club, Age=(int)age, Country=country, Price=(int)price/1000000};
            _context.Players.Add(player);
            if (_player is not null) {
                 _context.Players.Remove(_player);
            }
            _context.SaveChanges();
            TempData["rPID"] = null;
            return Redirect("/");
        }
        return Redirect("/home/error/");

     }

     public ActionResult ClubListings () {
        var user = UserProps();
        if (user is not null) {
            ViewBag.User = user;
            var listings = _context.Players.Where(prop => prop.Club == user.Club);
            ViewBag.ClubListings = listings;
        }
        return View();
     }

    [HttpPost]
    [ValidateAntiForgeryToken]
     public ActionResult DeactivateListing(int pid) {
        var backToSource = Request.Headers["Referer"].ToString();
        var TargerPlayer = _context.Players.FirstOrDefault(prop => prop.PlayerID == pid);
        if (TargerPlayer is not null) {
            _context.Players.Remove(TargerPlayer);
            _context.SaveChanges();
        }
        return Redirect(backToSource);
     }

     [HttpPost]
    [ValidateAntiForgeryToken]
     public ActionResult EditListing(int pID) {
        var TargetPlayer = _context.Players.FirstOrDefault(prop => prop.PlayerID == pID);
        var user = UserProps();
        if (TargetPlayer is not null && user is not null) {
            TempData["rPID"] = TargetPlayer.PlayerID;
            return RedirectToAction("ListPlayer");
        }
        return Redirect("/");
     }

    public User UserProps() {
        var Identifier = Request.Cookies["CookieIdentifier"];
        var FindUser = _context.Users.FirstOrDefault(prop => prop.Username == Identifier || prop.Email == Identifier);
        return FindUser;
     }
}
}

