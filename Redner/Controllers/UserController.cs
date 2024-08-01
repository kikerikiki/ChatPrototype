using Microsoft.AspNetCore.Mvc;
using LAPTemplateMVC.Models.dboSchema;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using LAPTemplateMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace LAPTemplateMVC.Controllers
{
    public class UserController : Controller
    {
        private readonly chatlerContext _context;

        public UserController(chatlerContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Chatuser user)
        {
       
                user.Passwordhash = BCrypt.Net.BCrypt.HashPassword(user.Passwordhash);
                user.Createdat = DateTime.Now;
                user.Valid = 1; // Set to valid user

                // Set a default or generated public key for now
                user.Publickey = "your-generated-public-key"; // Replace with actual key generation logic

                await _context.Procedures.ChatuserInsertAsync(user.Username, user.Publickey, user.Passwordhash, user.Createdat, user.Valid);
                return RedirectToAction("Login");
            
            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Chatuser.SingleOrDefaultAsync(u => u.Username == username && u.Valid == 1);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Passwordhash))
            {
                // Benutzer authentifiziert, Session setzen
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetInt32("UserId", (int)user.Chatuserid);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Invalid username or password");
            return View();
        }
    }
}
