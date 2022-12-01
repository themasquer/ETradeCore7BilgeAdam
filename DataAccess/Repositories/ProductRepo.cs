using AppCore.DataAccess.EntityFramework.Bases;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public abstract class ProductRepoBase : RepoBase<Product> // ProductRepoBase Product tipi üzerinden RepoBase'den miras alan ve Product CRUD işlemlerini yapacak abstract bir class'tır.
    {
        protected ProductRepoBase(DbContext dbContext) : base(dbContext) // bu abstract class'a dışarıdan new'lenerek gönderilen dbContext objesi RepoBase'in parametreli constructor'ına
                                                                         // gönderilir ki RepoBase'de kullanılabilsin.
        {
        }
    }
}
