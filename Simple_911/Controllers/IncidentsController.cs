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
    public class IncidentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IncidentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Incidents
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Incidents.Include(i => i.CallTaker).Include(i => i.Dispatcher).Include(i => i.Priority).Include(i => i.Status);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Incidents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents
                .Include(i => i.CallTaker)
                .Include(i => i.Dispatcher)
                .Include(i => i.Priority)
                .Include(i => i.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        // GET: Incidents/Create
        public IActionResult Create()
        {
            ViewData["CallTakerId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["DispatcherId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Id");
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id");
            return View();
        }

        // POST: Incidents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Address,City,State,Zip,Created,IsClosed,Callback,PriorityId,TypeId,StatusId,CallTakerId,DispatcherId")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                _context.Add(incident);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CallTakerId"] = new SelectList(_context.Users, "Id", "Id", incident.CallTakerId);
            ViewData["DispatcherId"] = new SelectList(_context.Users, "Id", "Id", incident.DispatcherId);
            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Id", incident.PriorityId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", incident.StatusId);
            return View(incident);
        }

        // GET: Incidents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
            {
                return NotFound();
            }
            ViewData["CallTakerId"] = new SelectList(_context.Users, "Id", "Id", incident.CallTakerId);
            ViewData["DispatcherId"] = new SelectList(_context.Users, "Id", "Id", incident.DispatcherId);
            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Id", incident.PriorityId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", incident.StatusId);
            return View(incident);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Address,City,State,Zip,Created,IsClosed,Callback,PriorityId,TypeId,StatusId,CallTakerId,DispatcherId")] Incident incident)
        {
            if (id != incident.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incident);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IncidentExists(incident.Id))
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
            ViewData["CallTakerId"] = new SelectList(_context.Users, "Id", "Id", incident.CallTakerId);
            ViewData["DispatcherId"] = new SelectList(_context.Users, "Id", "Id", incident.DispatcherId);
            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Id", incident.PriorityId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", incident.StatusId);
            return View(incident);
        }

        // GET: Incidents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _context.Incidents
                .Include(i => i.CallTaker)
                .Include(i => i.Dispatcher)
                .Include(i => i.Priority)
                .Include(i => i.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        // POST: Incidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IncidentExists(int id)
        {
            return _context.Incidents.Any(e => e.Id == id);
        }
    }
}
