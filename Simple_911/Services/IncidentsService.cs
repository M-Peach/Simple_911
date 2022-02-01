using Microsoft.EntityFrameworkCore;
using Simple_911.Data;
using Simple_911.Models;
using Simple_911.Services.Interfaces;

namespace Simple_911.Services
{
    public class IncidentsService : ITIncidentsService
    {
        private readonly ApplicationDbContext _context;

        public IncidentsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddIncidentNoteAsync(IncidentNote note)
        {
            try
            {
                await _context.AddAsync(note);
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Incident>> GetAllIncidentsAsync()
        {
            try
            {
                List<Incident> incidents = await _context.Incidents
                    .Include(t => t.Notes)
                    .Include(t => t.CallTaker)
                    .Include(t => t.Dispatcher)
                    .Include(t => t.Priority)
                    .Include(t => t.CallType)
                    .Include(t => t.Status)
                    .ToListAsync();

                return incidents;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Incident>> GetAllIncidentsByTypeAsync(string typeName)
        {
            int typeId = (await LookupIncidentTypeIdAsync(typeName)).Value;

            try
            {
                List<Incident> incidents = await _context.Incidents
                    .Include(t => t.Notes)
                    .Include(t => t.CallTaker)
                    .Include(t => t.Dispatcher)
                    .Include(t => t.Priority)
                    .Include(t => t.CallType)
                    .Include(t => t.Status)
                    .Where(t => t.TypeId == typeId)
                    .ToListAsync();

                return incidents;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Incident> GetIncidentByIdAsync(int incidentId)
        {
            try
            {
                Incident incident = (await GetAllIncidentsAsync()).FirstOrDefault(t => t.Id == incidentId);

                return incident;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int?> LookupIncidentTypeIdAsync(string typeName)
        {
            try
            {
                CallType type = await _context.CallTypes.FirstOrDefaultAsync(p => p.Name == typeName);

                return type?.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
