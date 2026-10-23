namespace Kitchen.Api.Models;

public class MealPlan
{
    public int TotalFed { get; set; }
    public List<DishCount> Dishes { get; set; } = new();
    public Dictionary<string, int> Used { get; set; } = new();
    public Dictionary<string, int> Leftovers { get; set; } = new();
}

public class DishCount
{
    public string Name { get; set; } = "";
    public int Count { get; set; }
    public int Fed { get; set; }
}