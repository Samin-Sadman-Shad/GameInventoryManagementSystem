using Play.Catalogue.Service.Entities;

namespace Play.Catalogue.Service.Contracts
{
    /// <summary>
    /// Declare methods for Item Entity specifically which are not present in the Generic Repository
    /// </summary>
    public interface IItemRepository:IGenericRepository<Item>
    {
    }
}
