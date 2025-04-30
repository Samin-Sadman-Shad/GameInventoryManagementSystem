namespace Play.Catalogue.Service.Models
{
    public class EntityResult<T>
    {
        public T? Entity { get; set; }
        public List<string> Errors { get; set; } = new();
        public bool IsSuccess => Errors.Count == 0;

        public static EntityResult<T> Success(T entity)
        {
            return new EntityResult<T> { Entity = entity };
        }

        public static EntityResult<T> Failure(params string[] errors)
        {
            return new EntityResult<T> { Errors = new List<string>(errors)  };
        }

    }
}
