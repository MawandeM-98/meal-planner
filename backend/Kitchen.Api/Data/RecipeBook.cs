using Kitchen.Api.Models;

namespace Kitchen.Api.Data;


public static class RecipeBook
{
    public static List<Recipe> GetRecipes()
    {
        return new List<Recipe>
        {
            new() { Name = "Burger", Feeds = 1, Ingredients = new() { ["Meat"] = 1, ["Lettuce"] = 1, ["Tomato"] = 1, ["Cheese"] = 1, ["Dough"] = 1 } },
            new() { Name = "Pie", Feeds = 1, Ingredients = new() { ["Dough"] = 2, ["Meat"] = 2 } },
            new() { Name = "Sandwich", Feeds = 1, Ingredients = new() { ["Dough"] = 1, ["Cucumber"] = 1 } },
            new() { Name = "Pasta", Feeds = 2, Ingredients = new() { ["Dough"] = 2, ["Tomato"] = 1, ["Cheese"] = 2, ["Meat"] = 1 } },
            new() { Name = "Salad", Feeds = 3, Ingredients = new() { ["Lettuce"] = 2, ["Tomato"] = 2, ["Cucumber"] = 1, ["Cheese"] = 2, ["Olives"] = 1 } },
            new() { Name = "Pizza", Feeds = 4, Ingredients = new() { ["Dough"] = 3, ["Tomato"] = 2, ["Cheese"] = 3, ["Olives"] = 1 } },
        };
    }

    public static Dictionary<string, int> GetPantry()
    {
        return new Dictionary<string, int>
        {
            ["Cucumber"] = 2,
            ["Olives"] = 2,
            ["Lettuce"] = 3,
            ["Meat"] = 6,
            ["Tomato"] = 6,
            ["Cheese"] = 8,
            ["Dough"] = 10,
        };
    }
}