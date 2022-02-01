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
    public class IncidentNotesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IncidentNotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: IncidentNotes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.IncidentNotes.Include(i => i.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: IncidentNotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentNote = await _context.IncidentNotes
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incidentNote == null)
            {
                return NotFound();
            }

            return View(incidentNote);
        }

        // GET: IncidentNotes/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: IncidentNotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TicketId,Note,Created,UserId")] IncidentNote incidentNote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(incidentNote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", incidentNote.UserId);
            return View(incidentNote);
        }

        // GET: IncidentNotes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentNote = await _context.IncidentNotes.FindAsync(id);
            if (incidentNote == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", incidentNote.UserId);
            return View(incidentNote);
        }

        // POST: IncidentNotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TicketId,Note,Created,UserId")] IncidentNote incidentNote)
        {
            if (id != incidentNote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incidentNote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidentNoteExists(incidentNote.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", incidentNote.UserId);
            return View(incidentNote);
        }

        // GET: IncidentNotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentNote = await _context.IncidentNotes
                .Include(i => i.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incidentNote == null)
            {
                return NotFound();
            }

            return View(incidentNote);
        }

        // POST: IncidentNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incidentNote = await _context.IncidentNotes.FindAsync(id);
            _context.IncidentNotes.Remove(incidentNote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncidentNoteExists(int id)
        {
            return _context.IncidentNotes.Any(e => e.Id == id);
        }
    }
}
