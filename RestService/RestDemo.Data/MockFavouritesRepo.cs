using System.Collections.Generic;
using System.Linq;

namespace RestDemo.Data
{
    public class MockFavouritesRepo : IFavouritesRepository
    {
        private readonly IList<FavouriteSpecies> _store;

        public MockFavouritesRepo()
        {
            _store = new List<FavouriteSpecies>();
        }

        public IList<FavouriteSpecies> GetFavourites()
        {
            return _store.ToList();
        }

        public void AddFavourite(FavouriteSpecies newFavourite)
        {
            if (_store.Contains(newFavourite)) return;
            _store.Add(newFavourite);
        }
    }
}