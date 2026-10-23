using Kitchen.Api.Data;
using Kitchen.Api.Services;

namespace Kitchen.Tests;

public class MealPlannerTests
{
    [Fact]
    public void FeedsTwelvePeopleWithTheGivenIngredients()
    {
        var plan = MealPlanner.Plan(RecipeBook.GetRecipes(), RecipeBook.GetPantry());

        Assert.Equal(12, plan.TotalFed);
    }

    [Fact]
    public void OnATieCooksTheFewestDishes()
    {
        var plan = MealPlanner.Plan(RecipeBook.GetRecipes(), RecipeBook.GetPantry());

        Assert.Equal(5, plan.Dishes.Sum(d => d.Count));
    }

    [Fact]
    public void DishesAddUpToTheTotal()
    {
        var plan = MealPlanner.Plan(RecipeBook.GetRecipes(), RecipeBook.GetPantry());

        Assert.Equal(plan.TotalFed, plan.Dishes.Sum(d => d.Fed));
    }

    [Fact]
    public void NeverUsesMoreThanThePantryHolds()
    {
        var plan = MealPlanner.Plan(RecipeBook.GetRecipes(), RecipeBook.GetPantry());

        Assert.All(plan.Leftovers.Values, left => Assert.True(left >= 0));
    }

    [Fact]
    public void FeedsNobodyWhenThePantryIsEmpty()
    {
        var plan = MealPlanner.Plan(RecipeBook.GetRecipes(), new Dictionary<string, int>());

        Assert.Equal(0, plan.TotalFed);
        Assert.Empty(plan.Dishes);
    }
}