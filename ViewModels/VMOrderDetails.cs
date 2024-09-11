using DbCottonDollStore.Models;

namespace DbCottonDollStore.ViewModels
{
    public class VMOrderDetails
    {
        public List<Store>? Store { get; set; }
        public List<User>? User { get; set; }
        public List<Product>? Products { get; set; }
        public List<ProductSpec>? Spec { get; set; }
        public List<Order>? Orders { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
