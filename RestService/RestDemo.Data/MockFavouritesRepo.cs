using System.Collections.Generic;
using System.Linq;

namespace RestDemo.Data
{
    internal static class FavouritesStore
    {
        internal static IList<FavouriteSpecies> Current = new List<FavouriteSpecies>(); 
    }

    public class MockFavouritesRepo : IFavouritesRepository
    {
        public MockFavouritesRepo()
        {

        }

        public IList<FavouriteSpecies> GetFavourites()
        {
            return FavouritesStore.Current.ToList();
        }

        public void AddFavourite(FavouriteSpecies newFavourite)
        {
            if (FavouritesStore.Current.Contains(newFavourite)) return;
            FavouritesStore.Current.Add(newFavourite);
        }
    }
}