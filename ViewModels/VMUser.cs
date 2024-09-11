using DbCottonDollStore.Models;

namespace CottonDollStore.ViewModels
{
    public class VMUser
    {
        public List<User>? user {  get; set; }
        public List<Rank>? Rank { get; set; }
        public List<Store>? Store { get; set; }

    }
}
