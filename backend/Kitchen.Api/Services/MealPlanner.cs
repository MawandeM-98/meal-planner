using Kitchen.Api.Models;

namespace Kitchen.Api.Services;

public static class MealPlanner
{
    public static MealPlan Plan(List<Recipe> recipes, Dictionary<string, int> pantry)
    {
        var left = new Dictionary<string, int>(pantry);
        var counts = new int[recipes.Count];
        var bestCounts = new int[recipes.Count];
        var bestFed = -1;
        var bestDishes = 0;

    
        void Search(int i, int fed, int dishes)
        {
            if (i == recipes.Count)
            {
                
                if (fed > bestFed || (fed == bestFed && dishes < bestDishes))
                {
                    bestFed = fed;
                    bestDishes = dishes;
                    counts.CopyTo(bestCounts, 0);
                }
                return;
            }

            var recipe = recipes[i];
            var made = 0;

            while (true)
            {
                counts[i] = made;
                Search(i + 1, fed + made * recipe.Feeds, dishes + made);

                if (!CanCook(recipe, left)) break;

                foreach (var item in recipe.Ingredients)
                    left[item.Key] -= item.Value;
                made++;
            }

            // Hand back what this recipe took before the caller tries something else.
            if (made > 0)
            {
                foreach (var item in recipe.Ingredients)
                    left[item.Key] += item.Value * made;
            }
            counts[i] = 0;
        }

        Search(0, 0, 0);

        return BuildPlan(recipes, pantry, bestCounts, bestFed);
    }

    static bool CanCook(Recipe recipe, Dictionary<string, int> left)
    {
        return recipe.Ingredients.All(item => left.GetValueOrDefault(item.Key) >= item.Value);
    }

    static MealPlan BuildPlan(List<Recipe> recipes, Dictionary<string, int> pantry, int[] counts, int totalFed)
    {
        var plan = new MealPlan { TotalFed = totalFed };

        foreach (var item in pantry)
            plan.Used[item.Key] = 0;

        for (var i = 0; i < recipes.Count; i++)
        {
            if (counts[i] == 0) continue;

            plan.Dishes.Add(new DishCount { Name = recipes[i].Name, Count = counts[i] });

            foreach (var item in recipes[i].Ingredients)
                plan.Used[item.Key] = plan.Used.GetValueOrDefault(item.Key) + item.Value * counts[i];
        }

        foreach (var item in pantry)
            plan.Leftovers[item.Key] = item.Value - plan.Used[item.Key];

        return plan;
    }
}