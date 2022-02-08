#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Simple_911.Data;
using Simple_911.Models;
using Simple_911.Services.Interfaces;

namespace Simple_911.Controllers
{
    public class IncidentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<SimpleUser> _userManager;
        private readonly ITIncidentsService _incidentsService;

        public IncidentsController(ApplicationDbContext context, UserManager<SimpleUser> userManager, ITIncidentsService incidentsService)
        {
            _context = context;
            _userManager = userManager;
            _incidentsService = incidentsService;
        }

        // GET: Incidents
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Incidents.Include(i => i.CallTaker).Include(i => i.CallType).Include(i => i.Dispatcher).Include(i => i.Priority).Include(i => i.Status);
            return View(await applicationDbContext.ToListAsync());
        }

        //GET: DASHBOARD
        [Authorize(Roles = "Admin, Manager, Dispatcher, Call Taker, Ground Unit")]
        public async Task<IActionResult> Dashboard()
        {
            var applicationDbContext = _context.Incidents.Include(i => i.CallTaker).Include(i => i.Dispatcher).Include(i => i.Priority).Include(i => i.Status).Include(i => i.CallType);
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
                .Include(i => i.CallType)
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
            ViewData["CallTypeId"] = new SelectList(_context.CallTypes, "Id", "Id");
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
        public async Task<IActionResult> Create([Bind("Id,Address,City,State,Zip,Created,IsClosed,Callback,PriorityId,CallTypeId,StatusId,CallTakerId,DispatcherId,PtAge,PtSex,PtCon,PtBreath,PtHistory")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                _context.Add(incident);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CallTakerId"] = new SelectList(_context.Users, "Id", "Id", incident.CallTakerId);
            ViewData["CallTypeId"] = new SelectList(_context.CallTypes, "Id", "Id", incident.CallTypeId);
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
            ViewData["CallTypeId"] = new SelectList(_context.CallTypes, "Id", "Id", incident.CallTypeId);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Address,City,State,Zip,Created,IsClosed,Callback,PriorityId,CallTypeId,StatusId,CallTakerId,DispatcherId,PtAge,PtSex,PtCon,PtBreath,PtHistory")] Incident incident)
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
            ViewData["CallTypeId"] = new SelectList(_context.CallTypes, "Id", "Id", incident.CallTypeId);
            ViewData["DispatcherId"] = new SelectList(_context.Users, "Id", "Id", incident.DispatcherId);
            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Id", incident.PriorityId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Id", incident.StatusId);
            return View(incident);
        }

        // GET: DISPATCH
        [Authorize(Roles = "Admin, Manager, Dispatcher")]
        public async Task<IActionResult> Dispatch(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incident = await _incidentsService.GetIncidentByIdAsync(id);
            if (incident == null)
            {
                return NotFound();
            }
            ViewData["CallTakerId"] = new SelectList(_context.Users, "Id", "FullName", incident.CallTakerId);
            ViewData["DispatcherId"] = new SelectList(_context.Users, "Id", "FullName", incident.DispatcherId);
            ViewData["UnitsId"] = new SelectList(_context.Users, "Id", "UnitNumber", incident.Units);
            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Name", incident.PriorityId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", incident.StatusId);
            ViewData["CallTypeId"] = new SelectList(_context.CallTypes, "Id", "Name", incident.CallTypeId);
            return View(incident);
        }

        // POST: DISPATCH
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager, Dispatcher")]
        public async Task<IActionResult> Dispatch(int id, [Bind("Id,Address,City,State,Zip,Created,IsClosed,Callback,PriorityId,CallTypeId,StatusId,CallTakerId,DispatcherId,UnitsId")] Incident incident)
        {
            if (id != incident.Id)
            {
                return NotFound();
            }

            try
            {
                incident.StatusId = 2;

                SimpleUser user = await _userManager.GetUserAsync(User);

                incident.DispatcherId = user.Id;

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
            return RedirectToAction(nameof(Dashboard));

            ViewData["CallTakerId"] = new SelectList(_context.Users, "Id", "FullName", incident.CallTakerId);
            ViewData["DispatcherId"] = new SelectList(_context.Users, "Id", "FullName", incident.DispatcherId);
            ViewData["UnitsId"] = new SelectList(_context.Users, "Id", "UnitNumber", incident.Units);
            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Name", incident.PriorityId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", incident.StatusId);
            ViewData["CallTypeId"] = new SelectList(_context.CallTypes, "Id", "Name", incident.CallTypeId);
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
                .Include(i => i.CallType)
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

        // GET: RESTORE
        public async Task<IActionResult> Restore(int? id)
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

        // POST: RESTORE
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);

            incident.IsClosed = false;

            _context.Incidents.Update(incident);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Dashboard));
        }

    }
}

