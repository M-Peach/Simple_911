using System.ComponentModel.DataAnnotations;

namespace Simple_911.Models
{
    public class Priority
    {
        public int Id { get; set; }

        [Display(Name = "Priority Name")]
        public string Name { get; set; }
    }
}
