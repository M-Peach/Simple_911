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
    public class IncidentSupportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IncidentSupportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: IncidentSupports
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.IncidentSupports.Include(i => i.Incident);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: IncidentSupports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentSupport = await _context.IncidentSupports
                .Include(i => i.Incident)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incidentSupport == null)
            {
                return NotFound();
            }

            return View(incidentSupport);
        }

        // GET: IncidentSupports/Create
        public IActionResult Create()
        {
            ViewData["IncidentId"] = new SelectList(_context.Incidents, "Id", "Address");
            return View();
        }

        // POST: IncidentSupports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IncidentId")] IncidentSupport incidentSupport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(incidentSupport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IncidentId"] = new SelectList(_context.Incidents, "Id", "Address", incidentSupport.IncidentId);
            return View(incidentSupport);
        }

        // GET: IncidentSupports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentSupport = await _context.IncidentSupports.FindAsync(id);
            if (incidentSupport == null)
            {
                return NotFound();
            }
            ViewData["IncidentId"] = new SelectList(_context.Incidents, "Id", "Address", incidentSupport.IncidentId);
            return View(incidentSupport);
        }

        // POST: IncidentSupports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IncidentId")] IncidentSupport incidentSupport)
        {
            if (id != incidentSupport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incidentSupport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidentSupportExists(incidentSupport.Id))
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
            ViewData["IncidentId"] = new SelectList(_context.Incidents, "Id", "Address", incidentSupport.IncidentId);
            return View(incidentSupport);
        }

        // GET: IncidentSupports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentSupport = await _context.IncidentSupports
                .Include(i => i.Incident)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incidentSupport == null)
            {
                return NotFound();
            }

            return View(incidentSupport);
        }

        // POST: IncidentSupports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incidentSupport = await _context.IncidentSupports.FindAsync(id);
            _context.IncidentSupports.Remove(incidentSupport);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncidentSupportExists(int id)
        {
            return _context.IncidentSupports.Any(e => e.Id == id);
        }
    }
}
