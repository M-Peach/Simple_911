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
    public class IncidentUnitsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IncidentUnitsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: IncidentUnits
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.IncidentUnit.Include(i => i.Incident).Include(i => i.Unit);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: IncidentUnits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentUnit = await _context.IncidentUnit
                .Include(i => i.Incident)
                .Include(i => i.Unit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incidentUnit == null)
            {
                return NotFound();
            }

            return View(incidentUnit);
        }

        // GET: IncidentUnits/Create
        public IActionResult Create()
        {
            ViewData["IncidentId"] = new SelectList(_context.Incidents, "Id", "Address");
            ViewData["UnitId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: IncidentUnits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IncidentId,UnitId")] IncidentUnit incidentUnit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(incidentUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IncidentId"] = new SelectList(_context.Incidents, "Id", "Address", incidentUnit.IncidentId);
            ViewData["UnitId"] = new SelectList(_context.Users, "Id", "Id", incidentUnit.UnitId);
            return View(incidentUnit);
        }

        // GET: IncidentUnits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentUnit = await _context.IncidentUnit.FindAsync(id);
            if (incidentUnit == null)
            {
                return NotFound();
            }
            ViewData["IncidentId"] = new SelectList(_context.Incidents, "Id", "Address", incidentUnit.IncidentId);
            ViewData["UnitId"] = new SelectList(_context.Users, "Id", "Id", incidentUnit.UnitId);
            return View(incidentUnit);
        }

        // POST: IncidentUnits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IncidentId,UnitId")] IncidentUnit incidentUnit)
        {
            if (id != incidentUnit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incidentUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidentUnitExists(incidentUnit.Id))
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
            ViewData["IncidentId"] = new SelectList(_context.Incidents, "Id", "Address", incidentUnit.IncidentId);
            ViewData["UnitId"] = new SelectList(_context.Users, "Id", "Id", incidentUnit.UnitId);
            return View(incidentUnit);
        }

        // GET: IncidentUnits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incidentUnit = await _context.IncidentUnit
                .Include(i => i.Incident)
                .Include(i => i.Unit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incidentUnit == null)
            {
                return NotFound();
            }

            return View(incidentUnit);
        }

        // POST: IncidentUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incidentUnit = await _context.IncidentUnit.FindAsync(id);
            _context.IncidentUnit.Remove(incidentUnit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncidentUnitExists(int id)
        {
            return _context.IncidentUnit.Any(e => e.Id == id);
        }
    }
}
