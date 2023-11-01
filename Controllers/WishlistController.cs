using Microsoft.AspNetCore.Mvc;
using project.Models;

namespace project.Controllers {
    public class WishlistController : Controller
{
    private readonly AppDbContext _context;
    
    CookieOptions options = new CookieOptions();

    public WishlistController(AppDbContext context) {
        _context = context;
        options.Expires = DateTime.Now.AddDays(7);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult AddToWishlist(int PID) {
        var Identifier = Request.Cookies["CookieIdentifier"];
        var FindUser = _context.Users.FirstOrDefault(prop => prop.Username == Identifier || prop.Email == Identifier);
        if (FindUser is not null) {
            var Wishlisted = _context.Wishlists.FirstOrDefault(prop => prop.PlayerID == PID && prop.UserID == FindUser.UID);
            if(Wishlisted is null) {
                _context.Wishlists.Add(new Wishlist{UserID=FindUser.UID, PlayerID=PID});
                _context.SaveChanges();
                return Redirect("/");
            }
        }
        return Redirect("/");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult RemoveFromWishlist(int PID) {
        var backToSource = Request.Headers["Referer"].ToString();
        var Identifier = Request.Cookies["CookieIdentifier"];
        var FindUser = _context.Users.FirstOrDefault(prop => prop.Username == Identifier || prop.Email == Identifier);
        if (FindUser is not null) {
            var Wishlisted = _context.Wishlists.FirstOrDefault(prop => prop.PlayerID == PID && prop.UserID == FindUser.UID);
            if(Wishlisted is not null) {   
                _context.Wishlists.Remove(Wishlisted);
                _context.SaveChanges();
            }
        }
        return Redirect(backToSource);
    }
    public Wishlist? FindWishlist() {
        var backToSource = Request.Headers["Referer"].ToString();
        var Identifier = Request.Cookies["CookieIdentifier"];
        var FindUser = _context.Users.FirstOrDefault(prop => prop.Username == Identifier || prop.Email == Identifier);
        if (FindUser is not null) {
            var UserWishlist = _context.Wishlists.FirstOrDefault(prop => prop.UserID == FindUser.UID);
            if (UserWishlist is not null) {
                return UserWishlist;
            }
        }
        return null;
     }
}
}

