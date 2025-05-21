using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Define the contracts defining messages to exchange between services
namespace Play.Catalog.Contracts.Contracts
{
    /// <summary>
    /// Message used to sent events when a catalog item is created
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="Description"></param>
    public record CatalogItemCreated(Guid Id, string Name, string Description);
    /// <summary>
    /// Message used to sent events when a catalog item is created
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="Description"></param>
    public record CatalogItemUpdated(Guid Id, string Name, string Description);
    /// <summary>
    /// Message used to sent events when a scatalog item is deleted
    /// </summary>
    /// <param name="Id"></param>
    public record CatalogItemDeleted(Guid Id);
}
