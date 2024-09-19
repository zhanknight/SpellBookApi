using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpellBookApi.Contexts;
using SpellBookApi.Models;
using SpellBookApi.Models.Views;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SpellBookContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("SpellBook"));
});

var app = builder.Build();

// Map the endpoints

app.MapGet("/spells", async Task<Ok<IEnumerable<SpellView>>> 
    (SpellBookContext context) =>
{ 
    var results = await context.Spells
    .Include(r => r.Reagents)
    .ToListAsync();

    return TypedResults.Ok<IEnumerable<SpellView>>(results.Select(s => s.ToView()).ToList());
});

app.MapGet("/reagents", async Task<Ok<IEnumerable<ReagentView>>> 
    (SpellBookContext context) =>
{
    var results = await context.Reagents.ToListAsync();

    return TypedResults.Ok<IEnumerable<ReagentView>>(results.Select(r => r.ToView()).ToList());
});

app.MapGet("/spells/{spellId:Guid}", async Task<Results<NotFound, Ok<SpellView>>> 
    ([FromRoute] Guid spellId, SpellBookContext context) =>
{
    var result = await context.Spells
    .Include(r => r.Reagents)
    .FirstOrDefaultAsync(i => i.Id == spellId);

    if (result == null)
    {
        return TypedResults.NotFound();
    }

    return TypedResults.Ok<SpellView>( result.ToView());
});

app.MapGet("/reagents/{reagentId:Guid}", async Task<Results<NotFound, Ok<ReagentView>>> 
    ([FromRoute] Guid reagentId, SpellBookContext context) =>
{
    var result = await context.Reagents.FindAsync(reagentId);

    if (result == null)
    {
        return TypedResults.NotFound();
    }

    return TypedResults.Ok<ReagentView>(result.ToView());
});

app.UseHttpsRedirection();

//We're just learning and testing here, let's wipe/rebuild the SQLite DB on each run
using (var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var content = scope.ServiceProvider.GetRequiredService<SpellBookContext>();
    content.Database.EnsureDeleted();
    content.Database.Migrate();
}

app.Run();