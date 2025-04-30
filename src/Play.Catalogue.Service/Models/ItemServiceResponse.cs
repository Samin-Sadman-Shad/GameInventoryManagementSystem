using System.Net;

namespace Play.Catalogue.Service.Models
{
    public class ItemServiceResponse<ItemDto>:ItemServiceResponse
    {

        public Guid? RecordId { get; set; }
        public ItemDto Record { get; set; }
        public List<ItemDto>? Records { get; set; }
    }

    public class ItemServiceResponse
    {
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
