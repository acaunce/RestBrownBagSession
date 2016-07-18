using System.Collections.Generic;

namespace RestDemo.Data
{
    public interface IFavouritesRepository
    {
        IList<FavouriteSpecies> GetFavourites();
        void AddFavourite(FavouriteSpecies newFavourite);
    }
}