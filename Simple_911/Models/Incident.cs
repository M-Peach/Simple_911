using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple_911.Models
{
    public class Incident
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "City")]
        public string? City { get; set; }

        [Required]
        [Display(Name = "State")]
        public string? State { get; set; }

        [Required]
        [Display(Name = "Zip")]
        public string? Zip { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset Created { get; set; }

        [Display(Name = "Closed")]
        public bool IsClosed { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Callback Number")]
        public string? Callback { get; set; }

        [NotMapped]
        [Display(Name = "Callback Number")]
        public string FormattedPhone { get { return String.Format("{0:(###) ###-####}", Convert.ToInt64(Callback)); } }

        [NotMapped]
        public string? NumberStreet { get { return Address.Replace(" ", "+"); } }

        [NotMapped]
        [Display(Name = "Address Link")]
        public string AddressLink { get { return $"https://google.com/maps/search/{NumberStreet}+{City}+{State}+{Zip}"; } }

        [NotMapped]
        [Display(Name = "Formatted Time")]
        public string FormattedTime { get { return Created.ToString("MM/dd ~ H:mm"); } }

        [Display(Name = "Priority")]
        public int PriorityId { get; set; }

        [Display(Name = "Call Type")]
        public int CallTypeId { get; set; }

        [Display(Name = "Unit Status")]
        public int StatusId { get; set; }

        [Display(Name = "Call Taker")]
        public string CallTakerId { get; set; }

        [Display(Name = "Dispatcher")]
        public string? DispatcherId { get; set; }

        [Display(Name = "Patient Age")]
        public string? PtAge { get; set; }

        [Display(Name = "Patient Sex")]
        public string? PtSex { get; set; }

        [Display(Name = "Patient Conciousness")]
        public string? PtCon { get; set; }

        [Display(Name = "Patient Breathing")]
        public string? PtBreath { get; set; }

        [Display(Name = "Patient History")]
        public string? PtHistory { get; set; }


        public virtual Priority Priority { get; set; }

        public virtual CallType CallType { get; set; }

        public virtual Status Status { get; set; }

        public virtual SimpleUser CallTaker { get; set; }

        public virtual SimpleUser? Dispatcher { get; set; }

        public virtual ICollection<IncidentUnit> Units { get; set; } = new HashSet<IncidentUnit>();

        public virtual ICollection<IncidentNote> Notes { get; set; } = new HashSet<IncidentNote>();
    }
}
