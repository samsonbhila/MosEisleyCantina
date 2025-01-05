using MosEisleyCantinaAPI.Models.DTOs;
using MosEisleyCantinaAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using MosEisleyCantina.Models.Entities;

namespace MosEisleyCantinaAPI.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, AppDbContext context)
        {
            if (context.Dishes.Any())
            {
                return;
            }
            //seeding dishes
            var dishes = new List<DishDTO>
            {
                new DishDTO { Name = "Taco", Description = "A delicious taco with ground beef, lettuce, cheese, and salsa.", Price = 115.99m, ImageUrl = "https://www.archanaskitchen.com/images/archanaskitchen/1-Author/sneha-archanaskitchen.com/Classic_Mexican_Taco_Recipe_With_Refried_Beans__Fresh_Summer_Salad.jpg" },
                new DishDTO { Name = "Pizza", Description = "A cheesy pizza with pepperoni and a crispy crust.", Price = 112.99m, ImageUrl = "https://riotfest.org/2016/10/21/28-photos-pepperoni-pizza/" },
                new DishDTO { Name = "Burger", Description = "A juicy burger with lettuce, tomato, and cheese.", Price = 118.99m, ImageUrl = "https://thebigmansworld.com/wp-content/uploads/2022/11/healthy-burgers-1024x1536.jpg" },
                new DishDTO { Name = "Pasta", Description = "Pasta with marinara sauce and fresh basil.", Price = 119.99m, ImageUrl = "https://slowcookergourmet.net/wp-content/uploads/2024/05/Slow-Cooker-Buffalo-Chicken-Pasta-17-of-27.jpg" },
                new DishDTO { Name = "Sushi", Description = "Fresh sushi rolls with fish, rice, and seaweed.", Price = 115.99m, ImageUrl = "https://www.thedailymeal.com/img/gallery/the-complete-list-of-sushi-rolls-ranked/intro-1666963587.webp" },
                new DishDTO { Name = "Salad", Description = "A healthy salad with mixed greens, cucumbers, and a vinaigrette dressing.", Price = 79.99m, ImageUrl = "https://hellolittlehome.com/wp-content/uploads/2022/08/garden-salad-22.jpg" },
                new DishDTO { Name = "Steak", Description = "A perfectly cooked steak with mashed potatoes and gravy.", Price = 118.99m, ImageUrl = "https://www.eatwell101.com/wp-content/uploads/2020/10/Garlic-Butter-Steak-recipe-roasted-in-Oven.jpg" },
                new DishDTO { Name = "Chicken Wings", Description = "Spicy chicken wings with a side of ranch dressing.", Price = 110.99m, ImageUrl = "https://www.wholesomeyum.com/wp-content/uploads/2022/12/wholesomeyum-Baked-Whole-Chicken-Wings-15.jpg" },
                new DishDTO { Name = "Ramen", Description = "A bowl of ramen with broth, noodles, and toppings.", Price = 113.99m, ImageUrl = "https://www.kitchensanctuary.com/wp-content/uploads/2020/12/Quick-Chicken-Ramen-Tall-FS-8.webp" },
                new DishDTO { Name = "Ice Cream", Description = "Vanilla ice cream topped with chocolate syrup and sprinkles.", Price = 34.99m, ImageUrl = "https://www.mashed.com/img/gallery/14-most-popular-ice-cream-flavors-in-the-us-and-where-they-came-from/intro-1688671251.webp" }
            };

            var dishEntities = dishes.Select(dish => new Dish
            {
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                ImageUrl = dish.ImageUrl,
                Rating = 0,
                RatingCount = 0
            }).ToList();

            context.Dishes.AddRange(dishEntities);

          //seeding drinks
            var drinks = new List<DrinkDTO>
            {
                new DrinkDTO { Name = "Coke", Description = "A refreshing Coca-Cola.", Price = 12.99m, ImageUrl = "https://assets.woolworths.com.au/images/1005/93167.jpg?impolicy=wowsmkqiema&w=600&h=600" },
                new DrinkDTO { Name = "Pepsi", Description = "A cool and crisp Pepsi.", Price = 12.89m, ImageUrl = "https://twigscafe.com/wp-content/uploads/2021/06/selection-of-different-coffee-type.jpg" },
                new DrinkDTO { Name = "Lemonade", Description = "A sweet and tart lemonade.", Price = 13.49m, ImageUrl = "https://twigscafe.com/wp-content/uploads/2021/06/selection-of-different-coffee-type.jpg" },
                new DrinkDTO { Name = "Iced Tea", Description = "A refreshing iced tea with lemon.", Price = 12.79m, ImageUrl = "" },
                new DrinkDTO { Name = "Orange Juice", Description = "Freshly squeezed orange juice.", Price = 13.99m, ImageUrl = "https://assets.woolworths.com.au/images/1005/93167.jpg?impolicy=wowsmkqiema&w=600&h=600" },
                new DrinkDTO { Name = "Water", Description = "Pure refreshing water.", Price = 11.99m, ImageUrl = "https://assets.woolworths.com.au/images/1005/93167.jpg?impolicy=wowsmkqiema&w=600&h=600" },
                new DrinkDTO { Name = "Milk", Description = "A glass of cold milk.", Price = 12.49m, ImageUrl = "https://assets.woolworths.com.au/images/1005/93167.jpg?impolicy=wowsmkqiema&w=600&h=600" },
                new DrinkDTO { Name = "Coffee", Description = "Freshly brewed coffee.", Price = 13.49m, ImageUrl = "https://assets.woolworths.com.au/images/1005/93167.jpg?impolicy=wowsmkqiema&w=600&h=600" },
                new DrinkDTO { Name = "Hot Chocolate", Description = "A warm cup of hot chocolate.", Price = 13.99m, ImageUrl = "https://twigscafe.com/wp-content/uploads/2021/06/selection-of-different-coffee-type.jpg" },
                new DrinkDTO { Name = "Juice Box", Description = "A box of fruit juice.", Price = 12.49m, ImageUrl = "https://twigscafe.com/wp-content/uploads/2021/06/selection-of-different-coffee-type.jpg" }
            };

            var drinkEntities = drinks.Select(drink => new Drink
            {
                Name = drink.Name,
                Description = drink.Description,
                Price = drink.Price,
                ImageUrl = drink.ImageUrl,
                Rating = 0, 
                RatingCount = 0 
            }).ToList();

            context.Drinks.AddRange(drinkEntities);

            await context.SaveChangesAsync();

            await context.SaveChangesAsync();
        }
    }
}
