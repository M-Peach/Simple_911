using Microsoft.AspNetCore.Mvc.Rendering;

namespace Simple_911.Models.ViewModels
{
    public class ManageUserRolesViewModel
    {
        public SimpleUser User { get; set; }

        public MultiSelectList Roles { get; set; }

        public List<string> SelectedRoles { get; set; }
    }
}
