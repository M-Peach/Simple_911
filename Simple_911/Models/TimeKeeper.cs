using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple_911.Models
{
    public class TimeKeeper
    {
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset TCreated { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset? TDispatched { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset? TEnroute { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset? TOnscene { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset? TTransporting { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset? THospital { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset? TInService { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset? TOOS { get; set; }

        [DataType(DataType.DateTime)]
        public DateTimeOffset? TAssist { get; set; }

        [NotMapped]
        public string Dispatched { get { return TDispatched.Value.ToString("MM/dd/yyyy ~ H:mm EST"); } }

        [Display(Name = "Incident")]
        public int IncidentId { get; set; }

    }
}
