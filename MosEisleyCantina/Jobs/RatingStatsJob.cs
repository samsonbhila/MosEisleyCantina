using MosEisleyCantinaAPI.Repositories.Interfaces;

namespace MosEisleyCantina.Jobs
{
    public class RatingStatsJob
    {
        private readonly IDishRepository _dishRepository;
        private readonly IDrinkRepository _drinkRepository;

        public RatingStatsJob(IDishRepository dishRepository, IDrinkRepository drinkRepository)
        {
            _dishRepository = dishRepository;
            _drinkRepository = drinkRepository;
        }

        public async Task CalculateTopRatedItemsAsync()
        {
            var topDishes = await _dishRepository.GetTopRatedAsync(5);
            var topDrinks = await _drinkRepository.GetTopRatedAsync(5);
        }
    }

}
