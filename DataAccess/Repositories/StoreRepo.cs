using AppCore.DataAccess.EntityFramework.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;

namespace DataAccess.Repositories
{
    public abstract class StoreRepoBase : RepoBase<Store>
    {
        protected StoreRepoBase(ETradeContext dbContext) : base(dbContext)
        {
        }
    }

    public class StoreRepo : StoreRepoBase
    {
        public StoreRepo(ETradeContext dbContext) : base(dbContext)
        {
        }
    }
}
