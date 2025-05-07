namespace Play.Inventory.Service.Models
{
    public class EntityResponse
    {
        public List<string> Errors { get; set; } = new List<string>();
        public bool IsSuccess => Errors.Count == 0;
    }

    public class EntityResponse<T> : EntityResponse
    {
        public T Entity { get; set; }
        public static EntityResponse<T> Success(T entity)
        {
            return new EntityResponse<T> { Entity = entity };
        }

        public static EntityResponse<T> Failure(params string[] errors)
        {
            return new EntityResponse<T> { Errors = errors.ToList() };
        }
    }
}
