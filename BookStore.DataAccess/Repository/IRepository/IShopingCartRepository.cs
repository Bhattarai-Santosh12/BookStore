using BookStore.Models;

namespace BookStore.DataAccess.Repository.IRepository
{
    public interface IShopingCartRepository : IRepository<ShoppingCart>
    {
        void Update(ShoppingCart obj);
    }
}
