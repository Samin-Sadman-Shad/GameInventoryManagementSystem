using System.Net;

namespace Play.Inventory.Service.Models
{
    public class InventoryItemServiceResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }

    public class InventoryItemServiceResponse<T> : InventoryItemServiceResponse
    {
        public Guid RecordId { get; set; }
        public T? Record { get; set; }
        public IList<T>? Records { get; set; }
    }
}
