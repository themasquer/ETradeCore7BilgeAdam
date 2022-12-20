﻿#nullable disable

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Business.Models
{
	public class AccountRegisterModel // register (uygulamaya kayıt) view'ı için herhangi bir entity üzerinden oluşturmadığımız model
	{
		[Required(ErrorMessage = "{0} is required!")]
		[MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
		[MaxLength(15, ErrorMessage = "{0} must be maximum {1} characters!")]
		[DisplayName("User Name")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "{0} is required!")]
		[MinLength(5, ErrorMessage = "{0} must be minimum {1} characters!")]
		[MaxLength(10, ErrorMessage = "{0} must be maximum {1} characters!")]
		public string Password { get; set; }
	}
}
