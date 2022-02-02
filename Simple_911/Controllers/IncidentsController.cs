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
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Incidents.Include(i => i.CallTaker).Include(i => i.Dispatcher).Include(i => i.PrimaryUnit).Include(i => i.Priority).Include(i => i.Status).Include(i => i.CallType);
            return View(await applicationDbContext.ToListAsync());
        }

        //GET: DASHBOARD
        [Authorize(Roles = "Admin, Manager, Dispatcher, Call Taker, Ground Unit")]
        public async Task<IActionResult> Dashboard()
        {
            var applicationDbContext = _context.Incidents.Include(i => i.CallTaker).Include(i => i.Dispatcher).Include(i => i.PrimaryUnit).Include(i => i.Priority).Include(i => i.Status).Include(i => i.CallType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TIMEKEEPER
        [Authorize(Roles = "Admin, Manager, Dispatcher, Call Taker, Ground Unit")]
        public async Task<IActionResult> TimeKeeper(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }



            Incident incident = await _incidentsService.GetIncidentByIdAsync(id.Value);

            //incident.TimeKeeper = TimeKeeper timeKeeper.Id;

            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        // GET: Incidents/Details/5
        [Authorize(Roles = "Admin, Manager, Dispatcher, Call Taker, Ground Unit")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Incident incident = await _incidentsService.GetIncidentByIdAsync(id.Value);

            if (incident == null)
            {
                return NotFound();
            }

            return View(incident);
        }

        // GET: Incidents/Create
        [Authorize(Roles = "Admin, Manager, Dispatcher, Call Taker")]
        public IActionResult Create()
        {
            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Name");
            ViewData["CallTypeId"] = new SelectList(_context.CallTypes, "Id", "Name");
            return View();
        }

        // POST: Incidents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager, Dispatcher, Call Taker")]
        public async Task<IActionResult> Create([Bind("Id,Address,City,State,Zip,Created,IsClosed,Callback,PriorityId,CallTypeId,StatusId,CallTakerId,DispatcherId,PrimaryUnitId")] Incident incident)
        {
            incident.DispatcherId = null;

            incident.PrimaryUnitId = null;

            incident.IsClosed = false;

            SimpleUser user = await _userManager.GetUserAsync(User);

            incident.CallTakerId = user.Id;

            incident.Created = DateTimeOffset.Now;

            incident.TimeKeeper.TCreated = DateTimeOffset.Now;

            incident.TimeKeeper.Id = incident.Id;

            incident.StatusId = 1; // "1" is "Pending"

            _context.Add(incident);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }

        // GET: PATIENT
        [Authorize(Roles = "Admin, Manager, Dispatcher, Call Taker")]
        public async Task<IActionResult> Patient(int? id)
        {
            Incident incident = await _incidentsService.GetIncidentByIdAsync(id.Value);

            return View(incident);
        }

        // POST: PATIENT

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager, Dispatcher, Call Taker")]
        public async Task<IActionResult> Patient([Bind("Id,Address,City,State,Zip,Created,IsClosed,Callback,PriorityId,CallTypeId,StatusId,CallTakerId,DispatcherId,PrimaryUnitId,PtAge,PtSex,PtCon,PtBreath,PtHistory")] Incident incident)
        {
            if (incident.PtSex.ToUpper() == "M" || incident.PtSex.ToUpper() == "MAN" || incident.PtSex.ToUpper() == "MALE")
            {
                incident.PtSex = "MALE";
            }
            else if (incident.PtSex.ToUpper() == "F" || incident.PtSex.ToUpper() == "FEMALE" || incident.PtSex.ToUpper() == "W" || incident.PtSex.ToUpper() == "WOMAN")
            {
                incident.PtSex = "FEMALE";
            }
            else {  }

            if (incident.PtCon.ToUpper() == "Y" || incident.PtCon.ToUpper() == "YES" || incident.PtCon.ToUpper() == "C" || incident.PtCon.ToUpper() == "CONSCIOUS")
            {
                incident.PtCon = "CONSCIOUS";
            }
            else if (incident.PtCon.ToUpper() == "N" || incident.PtCon.ToUpper() == "NO" || incident.PtCon.ToUpper() == "UNC")
            {
                incident.PtCon = "UNCONSCIOUS";
            }
            else { }

            if (incident.PtBreath.ToUpper() == "Y" || incident.PtBreath.ToUpper() == "YES" || incident.PtBreath.ToUpper() == "B" || incident.PtBreath.ToUpper() == "BREATHING")
            {
                incident.PtBreath = "BREATHING";
            }
            else if (incident.PtBreath.ToUpper() == "N" || incident.PtBreath.ToUpper() == "NO" || incident.PtBreath.ToUpper() == "NOT" || incident.PtBreath.ToUpper() == "NOT BREATHING")
            {
                incident.PtBreath = "NOT BREATHING";
            }
            else { }

            _context.Update(incident);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Dashboard));
        }

        // GET: Incidents/Edit/5
        [Authorize(Roles = "Admin, Manager, Dispatcher, Call Taker")]
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
            ViewData["CallTakerId"] = new SelectList(_context.Users, "Id", "FullName", incident.CallTakerId);
            ViewData["DispatcherId"] = new SelectList(_context.Users, "Id", "FullName", incident.DispatcherId);
            ViewData["PrimaryUnitId"] = new SelectList(_context.Users, "Id", "UnitNumber", incident.PrimaryUnitId);
            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Name", incident.PriorityId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", incident.StatusId);
            ViewData["CallTypeId"] = new SelectList(_context.CallTypes, "Id", "Name", incident.CallTypeId);
            return View(incident);
        }

        // POST: Incidents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager, Dispatcher, Call Taker")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Address,City,State,Zip,Created,IsClosed,Callback,PriorityId,CallTypeId,StatusId,CallTakerId,DispatcherId,PrimaryUnitId")] Incident incident)
        {
            if (id != incident.Id)
            {
                return NotFound();
            }

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
                return RedirectToAction(nameof(Dashboard));

            ViewData["CallTakerId"] = new SelectList(_context.Users, "Id", "FullName", incident.CallTakerId);
            ViewData["DispatcherId"] = new SelectList(_context.Users, "Id", "FullName", incident.DispatcherId);
            ViewData["PrimaryUnitId"] = new SelectList(_context.Users, "Id", "UnitNumber", incident.PrimaryUnitId);
            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Name", incident.PriorityId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", incident.StatusId);
            ViewData["CallTypeId"] = new SelectList(_context.CallTypes, "Id", "Name", incident.CallTypeId);
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
            ViewData["PrimaryUnitId"] = new SelectList(_context.Users, "Id", "UnitNumber", incident.PrimaryUnitId);
            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Name", incident.PriorityId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", incident.StatusId);
            ViewData["CallTypeId"] = new SelectList(_context.CallTypes, "Id", "Name", incident.CallTypeId);
            return View(incident);
        }

        // POST: DISPATCH
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager, Dispatcher")]
        public async Task<IActionResult> Dispatch(int id, [Bind("Id,Address,City,State,Zip,Created,IsClosed,Callback,PriorityId,CallTypeId,StatusId,CallTakerId,DispatcherId,PrimaryUnitId")] Incident incident)
        {
            if (id != incident.Id)
            {
                return NotFound();
            }

            try
            {
                incident.TimeKeeper.TDispatched = DateTimeOffset.Now;

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
            ViewData["PrimaryUnitId"] = new SelectList(_context.Users, "Id", "UnitNumber", incident.PrimaryUnitId);
            ViewData["PriorityId"] = new SelectList(_context.Priorities, "Id", "Name", incident.PriorityId);
            ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", incident.StatusId);
            ViewData["CallTypeId"] = new SelectList(_context.CallTypes, "Id", "Name", incident.CallTypeId);
            return View(incident);
        }

        // INCIDENT NOTES

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager, Dispatcher, Call Taker")]
        public async Task<IActionResult> AddIncidentNote([Bind("Id,IncidentId,Note,UserId")] IncidentNote incidentNote)
        {
            incidentNote.UserId = _userManager.GetUserId(User);
            incidentNote.Created = DateTimeOffset.Now;


            await _incidentsService.AddIncidentNoteAsync(incidentNote);

            Incident incident = await _incidentsService.GetIncidentByIdAsync(incidentNote.IncidentId);

            incident.Notes.Add(incidentNote);

            return RedirectToAction("Details", new { id = incidentNote.IncidentId });
        }

        // INCIDENT ENROUTE BUTTON
        [HttpGet]
        [Authorize(Roles = "Admin, Manager, Dispatcher, Call Taker")]
        public async Task<IActionResult> Enroute(int id)
        {
            Incident incident = await _incidentsService.GetIncidentByIdAsync(id);

            return View(incident);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager, Dispatcher, Call Taker")]
        public async Task<IActionResult> Enroute([Bind("Id,Address,City,State,Zip,Created,IsClosed,Callback,PriorityId,CallTypeId,StatusId,CallTakerId,DispatcherId,PrimaryUnitId,PtAge,PtSex,PtCon,PtBreath,PtHistory")] Incident incident)
        {
            incident.TimeKeeper.TEnroute = DateTimeOffset.Now;

            incident.StatusId = 3;

            _context.Update(incident);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = incident.Id });
        }

        // INCIDENT ONSCENE BUTTON

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager, Dispatcher, Call Taker")]
        public async Task<IActionResult> Onscene([Bind("Id,Address,City,State,Zip,Created,IsClosed,Callback,PriorityId,CallTypeId,StatusId,CallTakerId,DispatcherId,PrimaryUnitId,PtAge,PtSex,PtCon,PtBreath,PtHistory")] Incident incident)
        {
            incident.TimeKeeper.TOnscene = DateTimeOffset.Now;

            incident.StatusId = 4;

            _context.Update(incident);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = incident.Id });
        }

        // INCIDENT TRANSPORTING BUTTON

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager, Dispatcher, Call Taker")]
        public async Task<IActionResult> Transporting([Bind("Id,Address,City,State,Zip,Created,IsClosed,Callback,PriorityId,CallTypeId,StatusId,CallTakerId,DispatcherId,PrimaryUnitId,PtAge,PtSex,PtCon,PtBreath,PtHistory")] Incident incident)
        {
            incident.TimeKeeper.TTransporting = DateTimeOffset.Now;

            incident.StatusId = 5;

            _context.Update(incident);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = incident.Id });
        }

        // INCIDENT AT HOSPITAL BUTTON

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager, Dispatcher, Call Taker")]
        public async Task<IActionResult> AtHospital([Bind("Id,Address,City,State,Zip,Created,IsClosed,Callback,PriorityId,CallTypeId,StatusId,CallTakerId,DispatcherId,PrimaryUnitId,PtAge,PtSex,PtCon,PtBreath,PtHistory")] Incident incident)
        {
            incident.TimeKeeper.THospital = DateTimeOffset.Now;

            incident.StatusId = 6;

            _context.Update(incident);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = incident.Id });
        }

        // INCIDENT IN SERVICE BUTTON

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager, Dispatcher, Call Taker")]
        public async Task<IActionResult> InService([Bind("Id,Address,City,State,Zip,Created,IsClosed,Callback,PriorityId,CallTypeId,StatusId,CallTakerId,DispatcherId,PrimaryUnitId,PtAge,PtSex,PtCon,PtBreath,PtHistory")] Incident incident)
        {
            incident.TimeKeeper.TInService = DateTimeOffset.Now;

            incident.StatusId = 7;

            _context.Update(incident);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = incident.Id });
        }

        // INCIDENT OOS BUTTON

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager, Dispatcher, Call Taker")]
        public async Task<IActionResult> OOS([Bind("Id,Address,City,State,Zip,Created,IsClosed,Callback,PriorityId,CallTypeId,StatusId,CallTakerId,DispatcherId,PrimaryUnitId,PtAge,PtSex,PtCon,PtBreath,PtHistory")] Incident incident)
        {
            incident.TimeKeeper.TOOS = DateTimeOffset.Now;

            incident.StatusId = 8;

            _context.Update(incident);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = incident.Id });
        }

        // INCIDENT ASSISTING UNIT BUTTON

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager, Dispatcher, Call Taker")]
        public async Task<IActionResult> AssistUnit([Bind("Id,Address,City,State,Zip,Created,IsClosed,Callback,PriorityId,CallTypeId,StatusId,CallTakerId,DispatcherId,PrimaryUnitId,PtAge,PtSex,PtCon,PtBreath,PtHistory")] Incident incident)
        {
            incident.TimeKeeper.TAssist = DateTimeOffset.Now;

            incident.StatusId = 9;

            _context.Update(incident);

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = incident.Id });
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
                .Include(i => i.PrimaryUnit)
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

            incident.IsClosed = true;

            _context.Incidents.Update(incident);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Dashboard));
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
                .Include(i => i.PrimaryUnit)
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

        private bool IncidentExists(int id)
        {
            return _context.Incidents.Any(e => e.Id == id);
        }
    }
}
