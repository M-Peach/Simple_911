using System.ComponentModel.DataAnnotations;

namespace Simple_911.Models
{
    public class CallType
    {
        public int Id { get; set; }

        [Display(Name = "Type Name")]
        public string Name { get; set; }

        [Display(Name = "Type Name")]
        public string Description { get; set; }
    }
}
