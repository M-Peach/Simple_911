using System.ComponentModel.DataAnnotations;

namespace Simple_911.Models
{
    public class IncidentSupport
    {
        public int Id { get; set; }

        [Display(Name = "Incident Id")]
        public int IncidentId { get; set; }

        [Display(Name = "Support Units")]
        public string SupportUnitId { get; set; }

        public virtual Incident Incident { get; set; } 
        public virtual SimpleUser SupportUnit { get; set; }
    }
}
