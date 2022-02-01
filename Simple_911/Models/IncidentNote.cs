using System.ComponentModel.DataAnnotations;

namespace Simple_911.Models
{
    public class IncidentNote
    {
        public int Id { get; set; }

        [Display(Name = "Incident Id")]
        public int IncidentId { get; set; }

        [Display(Name = "Note")]
        public string Note { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Note Date & Time")]
        public DateTimeOffset Created { get; set; }

        [Display(Name = "User")]
        public string UserId { get; set; }

        public virtual Incident Incident { get; set; }

        public virtual SimpleUser User { get; set; }
    }
}
