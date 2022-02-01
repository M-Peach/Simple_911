using Simple_911.Models;

namespace Simple_911.Services.Interfaces
{
    public interface ITIncidentsService
    {
        public Task<List<Incident>> GetAllIncidentsAsync();

        public Task<Incident> GetIncidentByIdAsync(int ticketId);

        public Task AddIncidentNoteAsync(IncidentNote note);

        public Task<List<Incident>> GetAllIncidentsByTypeAsync(string typeName);

        public Task<int?> LookupIncidentTypeIdAsync(string typeName);
    }
}
