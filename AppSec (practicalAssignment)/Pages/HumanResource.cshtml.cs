using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AppSec__practicalAssignment_.Pages
{
    [Authorize(Policy = "MustBelongToHRDepartment")]
    public class HumanResourceModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
