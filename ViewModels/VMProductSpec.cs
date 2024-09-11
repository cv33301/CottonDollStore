using DbCottonDollStore.Models;

namespace CottonDollStore.ViewModels
{
    public class VMProductSpec
    {
        public List<Store>? Store { get; set; }
        public List<User>? User { get; set; }
        public List<Product>? Products { get; set; }
        public List<ProductSpec>? Spec { get; set; }
        public List<Category>? Category { get; set; }

        public List<Category>? CategoryID_2Navigation { get; set; }

        public List<Category>? CategoryID_3Navigation { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }

    }
}
