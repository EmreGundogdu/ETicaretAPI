using ETicaretAPI.Domain.Entities.Common;

namespace ETicaretAPI.Domain.Entities
{
    public class CompletedOrder : BaseEntity
    {
        public int OrderId { get; set; }
        public Order Order{ get; set; }
    }
}
