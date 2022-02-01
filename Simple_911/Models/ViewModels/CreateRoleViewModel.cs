using System.ComponentModel.DataAnnotations;

namespace Simple_911.Models.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
