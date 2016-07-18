using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using RestDemo.Data;

namespace RestService.Controllers
{
    [Route("favourites")]
    public class FavouriteSpeciesController : ApiController
    {
        private readonly IFavouritesRepository _favouritesRepo;

        public FavouriteSpeciesController()
        {
            _favouritesRepo = new MockFavouritesRepo();
        }

        public FavouriteSpeciesController(IFavouritesRepository favouritesRepo)
        {
            _favouritesRepo = favouritesRepo;
        }

        [HttpGet]
        public IList<FavouriteSpecies> GetFavouriteSpecieses()
        {
            return _favouritesRepo.GetFavourites();
        }

        [HttpPost]
        public void AddNewFavourite(FavouriteSpecies newFave)
        {
            _favouritesRepo.AddFavourite(newFave);
        }
    }
}