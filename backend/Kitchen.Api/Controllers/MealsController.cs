using Kitchen.Api.Data;
using Kitchen.Api.Models;
using Kitchen.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kitchen.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MealsController : ControllerBase
{
    [HttpGet]
    public MealPlan Get()
    {
        return MealPlanner.Plan(RecipeBook.GetRecipes(), RecipeBook.GetPantry());
    }
}