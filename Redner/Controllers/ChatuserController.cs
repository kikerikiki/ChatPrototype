using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LAPTemplateMVC.Models;
using LAPTemplateMVC.Models.dboSchema;

namespace LAPTemplateMVC.Controllers
{
    public class ChatuserController : Controller
    {
        private readonly chatlerContext _context;

        public ChatuserController(chatlerContext context)
        {
            _context = context;
        }

        // GET: Chatuser
        public async Task<IActionResult> Index()
        {
            return View(await _context.Chatuser.ToListAsync());
        }

        // GET: Chatuser/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatuser = await _context.Chatuser
                .FirstOrDefaultAsync(m => m.Chatuserid == id);
            if (chatuser == null)
            {
                return NotFound();
            }

            return View(chatuser);
        }

        // GET: Chatuser/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chatuser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Chatuserid,Username,Publickey,Passwordhash,Createdat,Valid,ModUser,ModTimestamp,CrUser,CrTimestamp")] Chatuser chatuser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chatuser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chatuser);
        }

        // GET: Chatuser/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatuser = await _context.Chatuser.FindAsync(id);
            if (chatuser == null)
            {
                return NotFound();
            }
            return View(chatuser);
        }

        // POST: Chatuser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Chatuserid,Username,Publickey,Passwordhash,Createdat,Valid,ModUser,ModTimestamp,CrUser,CrTimestamp")] Chatuser chatuser)
        {
            if (id != chatuser.Chatuserid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chatuser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChatuserExists(chatuser.Chatuserid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(chatuser);
        }

        // GET: Chatuser/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatuser = await _context.Chatuser
                .FirstOrDefaultAsync(m => m.Chatuserid == id);
            if (chatuser == null)
            {
                return NotFound();
            }

            return View(chatuser);
        }

        // POST: Chatuser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var chatuser = await _context.Chatuser.FindAsync(id);
            if (chatuser != null)
            {
                _context.Chatuser.Remove(chatuser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChatuserExists(long id)
        {
            return _context.Chatuser.Any(e => e.Chatuserid == id);
        }
    }
}
