using System.ComponentModel.DataAnnotations;

namespace Simple_911.Models
{
    public class Status
    {
        public int Id { get; set; }

        [Display(Name = "Status Name")]
        public string Name { get; set; }
    }
}
