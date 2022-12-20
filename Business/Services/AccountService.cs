using AppCore.Results;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess.Enums;

namespace Business.Services
{
	public interface IAccountService // IAccountService'i IService'ten implemente etmiyoruz çünkü bu servis UserService enjeksiyonu üzerinden login ve register işlerini yapacak,
									 // CRUD işlemlerinin hepsini yapmayacak, bu yüzden Login ve Register methodlarını içerisinde tanımlıyoruz
	{
		Result Login(AccountLoginModel accountLoginModel, UserModel userModel); // kullanıcıların kullanıcı girişi için
        // accountLoginModel view üzerinden kullanıcıdan aldığımız verilerdir,
        // userModel ise accountLoginModel'deki doğru verilere göre kullanıcıyı veritabanından çektikten sonra method içerisinde atayacağımız ve
        // referans tip olduğu için de Login methodunu çağırdığımız yerde kullanabileceğimiz sonuç kullanıcı objesidir,
        // böylelikle Login methodundan hem login işlem sonucunu Result olarak hem de işlem başarılıysa kullanıcı objesini UserModel objesi olarak dönebiliyoruz

        Result Register(AccountRegisterModel accountRegisterModel); // kullanıcıların yeni kullanıcı kaydı için
	}

	public class AccountService : IAccountService
	{
		private readonly IUserService _userService; // CRUD işlemlerini yaptığımız UserService objesini bu servise enjekte ediyoruz ki Query methodu üzerinden Login,
													// Add methodu üzerinden de Register işlemleri yapabilelim

		public AccountService(IUserService userService)
		{
			_userService = userService;
		}

		public Result Login(AccountLoginModel accountLoginModel, UserModel userModel) // kullanıcı girişi
		{
			// önce accountLoginModel üzerinden kullanıcının girmiş olduğu kullanıcı adı ve şifreye sahip aktif kullanıcı sorgusu üzerinden veriyi çekip userModel'a atıyoruz,
			// kullanıcı adı ve şifre hassas veri olduğu için trim'lemiyoruz ve büyük küçük harf duyarlılığını da ortadan kaldırmıyoruz
			userModel = _userService.Query().SingleOrDefault(u => u.UserName == accountLoginModel.UserName && u.Password == accountLoginModel.Password && u.IsActive);

			if (userModel is null) // eğer böyle bir kullanıcı bulunamadıysa
				return new ErrorResult("Invalid user name or password!"); // kullanıcı adı veya şifre hatalı sonucunu dönüyoruz
			
			// burada kullanıcı bulunmuş demektir dolayısıyla referans tip olduğu için hem userModel'i sorgulanan kullanıcı objesi
			// hem de işlem sonucunu SuccessResult objesi olarak methoddan dönüyoruz
			return new SuccessResult(); 
		}

		public Result Register(AccountRegisterModel accountRegisterModel) // yeni kullanıcı kaydı
		{
			var user = new UserModel()
			{
				IsActive = true, // istenirse burada olduğu gibi tüm kayıt yapan kullanıcılar aktif yapılabilir veya aktif yapılmayarak örneğin e-posta gönderimi sağlanıp
								 // gönderilen link'e tıklandıktan sonra kullanıcının aktiflik durumu aktif olarak güncellenebilir

				Password = accountRegisterModel.Password,
				UserName = accountRegisterModel.UserName,

                RoleId = (int)Roles.User, // Roles enum'ı üzerinden RoleId'yi atamak hem veritabanındaki rol tablosundaki id'ler güncellenirse bu enum üzerinden
										  // kolayca bu değişikliğin uygulanabilmesini hem de her bir rolün id'si neydi diye veritabanındaki tabloya
										  // sürekli bakılmasından kurtulmamızı sağlar
            };

			return _userService.Add(user); // UserService'teki Add methodu bize sonuç döndüğünden ve bu sonucu dönerek Register methodunu çağırdığımız yerde kullanabileceğimizden
										   // UserService'teki Add methodundan dönen sonucu Register methodu sonucu olarak dönebiliriz	
		}
	}
}
