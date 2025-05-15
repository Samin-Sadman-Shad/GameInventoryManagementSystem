using System.Net.Http.Json;
using static Play.Inventory.Service.DTOs.Dtos;

namespace Play.Inventory.Service.Clients
{
    /// <summary>
    /// Comminicate with external http endpoint of Catalog service
    /// </summary>
    public class CatalogClient
    {
        //need to communicate with external endpoint
        private readonly HttpClient _httpClient;

        public CatalogClient(HttpClient client)
        {
            this._httpClient = client;
        }

        //retrieve items from catalog
        //use route taht to be accessed in the invoked rest api
        //invoke another rest api in another service
        public async Task<IReadOnlyCollection<GetCatalogItemDto>> GetCatalogItemAsync()
        {
            var items = await _httpClient.GetFromJsonAsync<IReadOnlyCollection<GetCatalogItemDto>>("/Items");
            return items;
        }
    }
}
