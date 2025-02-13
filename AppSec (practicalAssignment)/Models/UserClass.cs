using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AppSec__practicalAssignment_.Models
{
    public class UserClass : IdentityUser
    {
		[Required]
		[DataType(DataType.Text)]
		public string FirstName { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string LastName { get; set; }

		[Required]
		[DataType(DataType.CreditCard)]
		public string CreditCardNo { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string BillingAdd { get; set; }

		[Required]
		[DataType(DataType.Text)]	
		public string ShipAdd { get; set; }

		public byte[]? userImagePath { get; set; }
	}
}
