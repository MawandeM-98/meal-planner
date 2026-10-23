using System.Net;
using System.Net.Http.Json;
using Kitchen.Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Kitchen.Tests;

public class MealsEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient client;

    public MealsEndpointTests(WebApplicationFactory<Program> factory)
    {
        client = factory.CreateClient();
    }

    [Fact]
    public async Task GetMeals_ReturnsTheBestPlan()
    {
        var response = await client.GetAsync("/api/meals");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var plan = await response.Content.ReadFromJsonAsync<MealPlan>();

        Assert.NotNull(plan);
        Assert.Equal(12, plan.TotalFed);
        Assert.Equal(5, plan.Dishes.Sum(d => d.Count));
    }

    [Fact]
    public async Task GetMeals_UsesTheFieldNamesTheFrontendReads()
    {
        var json = await client.GetStringAsync("/api/meals");

        Assert.Contains("\"totalFed\":12", json);
        Assert.Contains("\"dishes\":", json);
        Assert.Contains("\"fed\":", json);
        Assert.Contains("\"used\":", json);
        Assert.Contains("\"leftovers\":", json);
    }
}