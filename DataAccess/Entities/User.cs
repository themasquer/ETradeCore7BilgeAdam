#nullable disable

using AppCore.Records.Bases;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
	public class User : RecordBase
	{
		[Required]
		[StringLength(15)]
		public string UserName { get; set; }

		[Required]
		[StringLength(10)]
		public string Password { get; set; }

		public bool IsActive { get; set; } // sadece aktif kullanıcıların (IsActive = true) uygulamaya giriş yapmasını sağlamak için bu özellik kullanılır,
										   // kullanıcının uygulama üzerinden aktifliği kaldırıldığında (IsActive = false) artık uygulamaya giriş yapamayacaktır

		public int RoleId { get; set; } // 1 to many ilişki (1 kullanıcının mutlaka 1 rolü olmalı)

		public Role	Role { get; set; }
	}
}
