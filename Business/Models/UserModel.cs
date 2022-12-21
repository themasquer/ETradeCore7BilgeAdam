#nullable disable

using AppCore.Records.Bases;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
	public class UserModel : RecordBase
	{
		#region Entity'den Kopyalanan Özellikler
		[Required(ErrorMessage = "{0} is required!")]
		[MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
		[MaxLength(15, ErrorMessage = "{0} must be maximum {1} characters!")]
		[DisplayName("User Name")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "{0} is required!")]
		[MinLength(5, ErrorMessage = "{0} must be minimum {1} characters!")]
		[MaxLength(10, ErrorMessage = "{0} must be maximum {1} characters!")]
		public string Password { get; set; }

		[DisplayName("Active")]
		public bool IsActive { get; set; }

		[DisplayName("Role")]
		public int RoleId { get; set; }
		#endregion



		#region View'larda Gösterim için Kullanacağımız Özellikler
		[DisplayName("Role")]
		public string RoleNameDisplay { get; set; }
		#endregion
	}
}
