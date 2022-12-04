namespace ETicaretAPI.Domain.Entities.Common
{
    public class BaseEntity
    {
        public Guid ID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
