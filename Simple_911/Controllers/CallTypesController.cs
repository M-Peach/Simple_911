#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Simple_911.Data;
using Simple_911.Models;

namespace Simple_911.Controllers
{
    public class CallTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CallTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CallTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.CallTypes.ToListAsync());
        }

        // GET: CallTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var callType = await _context.CallTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (callType == null)
            {
                return NotFound();
            }

            return View(callType);
        }

        // GET: CallTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CallTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] CallType callType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(callType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(callType);
        }

        // GET: CallTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var callType = await _context.CallTypes.FindAsync(id);
            if (callType == null)
            {
                return NotFound();
            }
            return View(callType);
        }

        // POST: CallTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] CallType callType)
        {
            if (id != callType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(callType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CallTypeExists(callType.Id))
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
            return View(callType);
        }

        // GET: CallTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var callType = await _context.CallTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (callType == null)
            {
                return NotFound();
            }

            return View(callType);
        }

        // POST: CallTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var callType = await _context.CallTypes.FindAsync(id);
            _context.CallTypes.Remove(callType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CallTypeExists(int id)
        {
            return _context.CallTypes.Any(e => e.Id == id);
        }
    }
}
