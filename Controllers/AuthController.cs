using project.Models;
using Microsoft.AspNetCore.Mvc;


namespace project.Controllers {
    public class AuthController : Controller {
    private AppDbContext _context;
    CookieOptions options = new CookieOptions();
    
    public AuthController(AppDbContext context)
    {
        _context = context;
        options.Expires = DateTime.Now.AddDays(7);
    }

    [HttpGet]
    public ActionResult SignUp() {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult SignUp(User user) {
        var CheckIfExists = _context.Users.FirstOrDefault(prop => prop.Username == user.Username || prop.Email == user.Email);
        var CheckClubValid = _context.Players.FirstOrDefault(prop => prop.Club == user.Club);
        if (CheckIfExists is null && CheckClubValid is not null) {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
            Response.Cookies.Append("CookieIdentifier", user.Username, options);
            Response.Cookies.Append("CookiePassword", user.Password, options);
            return RedirectToAction("Login");
        }
        ViewBag.err = "Your Username or Email are already taken, or you club isn't valid. please try again";
        return View();
    }

    public ActionResult Login() {
        if (!string.IsNullOrEmpty(Request.Cookies["CookieIdentifier"])) {
            Console.WriteLine($"cookie username {Request.Cookies["CookieIdentifier"]}");
            return Redirect("/");
        }
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Login(string Identifier, string password) {
        if(!string.IsNullOrEmpty(Request.Cookies["CookieIdentifier"]) && !string.IsNullOrEmpty(Request.Cookies["CookiePassword"])) {
            
            return Redirect("/");
        }
        var FindUser = _context.Users.FirstOrDefault(prop => prop.Email == Identifier || prop.Username == Identifier);
        Console.WriteLine($"Identifier: {Identifier}");
        Console.WriteLine($"FindUser: {FindUser?.Username}");
        if (FindUser is not null) {
            bool PasswordCheck = BCrypt.Net.BCrypt.Verify(password, FindUser.Password);
            if (PasswordCheck) {
                Response.Cookies.Append("CookieIdentifier", Identifier, options);
                Response.Cookies.Append("CookiePassword", password, options);
                return Redirect("/");
            }
            ViewBag.err = "Your Username, Email or Password are incorrect. Please try again";
            return View();
        }
        ViewBag.err = "You are not signed up. Please sign up and try again";
        return View();
    }

    

   
}

}

