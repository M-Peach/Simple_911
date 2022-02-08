using System.ComponentModel.DataAnnotations;

namespace Simple_911.Models
{
    public class IncidentUnit
    {
        public int Id { get; set; }

        [Display(Name = "Incident Id")]
        public int IncidentId { get; set; }

        [Display(Name = "Unit Id")]
        public string UnitId { get; set; }

        public virtual Incident Incident { get; set; }

        public virtual SimpleUser Unit { get; set; }
    }
}
