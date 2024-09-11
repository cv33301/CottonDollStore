using DbCottonDollStore.Models;

namespace CottonDollStore.ViewModels
{
    public class VMStoreProducts
    {
        public List<Store>? Store { get; set; }
        public List<User>? User { get; set; }
        public List<Product>? Products { get; set; }
        public List<ProductSpec>? Spec { get; set; }
        public List<OrderDetail>? OrderDetail { get; set; }
    }
}
