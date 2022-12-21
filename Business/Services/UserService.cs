using AppCore.Business.Services.Bases;
using AppCore.Results;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess.Entities;
using DataAccess.Repositories;

namespace Business.Services
{
	public interface IUserService : IService<UserModel> // hem kullanıcılar için login ve register işlemlerinin AccountService üzerinden hem de
														// adminlerin kullanıcı listeleme, detay görüntüleme, ekleme, güncelleme ve silme (CRUD)
														// işlemlerini yapacağı UserService için bu servisi IService'ten implemente ediyoruz
	{
	}

	public class UserService : IUserService
	{
		private readonly UserRepoBase _userRepo; // kullanıcı servisi enjekte edilen kullanıcı repository'si üzerinden veritabanında işlemleri gerçekleştirecek

		public UserService(UserRepoBase userRepo)
		{
			_userRepo = userRepo;
		}

		public Result Add(UserModel model)
		{
			if (_userRepo.Exists(u => u.UserName == model.UserName)) // RepoBase'e belirtilen koşul veya koşullara göre kayıt var mı kontrolü için
                                                                     // Exists methodunu eklediğimizden artık kullanabiliriz
                return new ErrorResult("User can't be added because user with the same user name exists!");

			var entity = new User()
			{
				IsActive = model.IsActive,

				UserName = model.UserName, // kullanıcı adı hassas veri olduğundan trim'lemiyoruz yani model üzerinden nasıl geliyorsa onu kullanıyoruz

				Password = model.Password, // şifre hassas veri olduğundan trim'lemiyoruz yani model üzerinden nasıl geliyorsa onu kullanıyoruz,
                                           // eğer istenirse şifre verisini açık olarak veritabanına kaydetmek yerine hash'leyerek şifreli olarak kaydedebiliriz

                RoleId = model.RoleId
			};

			_userRepo.Add(entity);
			return new SuccessResult("User added successfully.");
		}

		public Result Delete(int id)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			_userRepo.Dispose();
		}

		public IQueryable<UserModel> Query()
		{
			return _userRepo.Query(u => u.Role) // User entity'sindeki Role referansı üzerinden rolleri de sorguya dahil et
				.OrderByDescending(u => u.IsActive) // önce kullanıcının aktifliğini azalan sıralıyoruz ki aktif olanları üstte görebilelim
				.ThenBy(u => u.UserName) // sonra aktiflik sıralamasına göre kullanıcı adlarına göre artan sıralıyoruz
				.Select(u => new UserModel()
				{
					Guid = u.Guid,
					Id = u.Id,
					IsActive= u.IsActive,
					Password = u.Password,
					RoleId = u.RoleId,
					UserName = u.UserName,
					RoleNameDisplay = u.Role.Name
				});
		}

		public Result Update(UserModel model)
		{
			throw new NotImplementedException();
		}
	}
}
