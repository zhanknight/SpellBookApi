using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpellBookApi.Contexts;
using SpellBookApi.Models;
using SpellBookApi.Models.Creates;
using SpellBookApi.Models.Entities;
using SpellBookApi.Models.Views;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SpellBookContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("SpellBook"));
});

var app = builder.Build();

// Map the GET endpoints
#region GetEndpoints

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
}).WithName("GetSpell");

app.MapGet("/reagents/{reagentId:Guid}", async Task<Results<NotFound, Ok<ReagentView>>> 
    ([FromRoute] Guid reagentId, SpellBookContext context) =>
{
    var result = await context.Reagents.FindAsync(reagentId);

    if (result == null)
    {
        return TypedResults.NotFound();
    }

    return TypedResults.Ok<ReagentView>(result.ToView());
}).WithName("GetReagent");
#endregion GetEndpoints

// Map the POST endpoints
#region PostEndpoints
app.MapPost("/reagents", async Task<CreatedAtRoute<ReagentView>>
    ([FromBody] ReagentCreate newReagent, SpellBookContext context) =>
{
    var x = context.Add(newReagent.ToEntity());
    await context.SaveChangesAsync();
    // validate doesn't exist already

    return TypedResults.CreatedAtRoute<ReagentView>(x.Entity.ToView(),"GetReagent", new {reagentId = x.Entity.Id});
});

app.MapPost("/spells", async Task<CreatedAtRoute<SpellView>> 
    ([FromBody] SpellCreate newSpell, SpellBookContext context) =>
{
    var x = context.Add(newSpell.ToEntity());
    foreach (var reagentId in newSpell.Reagents)
    {
        var reagent = await context.Reagents.FindAsync(reagentId);
        if (reagent != null)
        {
            x.Entity.Reagents.Add(reagent);
        }
    }
    await context.SaveChangesAsync();
    // validate doesn't exist already
    // validate that at least one existing reagent was included

    return TypedResults.CreatedAtRoute<SpellView>(x.Entity.ToView(), "GetSpell", new { SpellId = x.Entity.Id });
});
#endregion PostEndpoints

// Map the PUT endpoints
#region PutEndpoints
app.MapPut("/spells/{spellId:Guid}", async Task<Results<NotFound, NoContent>>
    ([FromRoute] Guid spellId, [FromBody] SpellCreate updatedSpell, SpellBookContext context) =>
{
    var spellExists = await context.Spells
    .Include(r => r.Reagents)
    .FirstOrDefaultAsync(i => i.Id == spellId);

    if (spellExists == null)
    {
        return TypedResults.NotFound();
    }

    spellExists.Reagents.Clear();
    foreach (var r in updatedSpell.Reagents)
    {
        var reagent = await context.Reagents.FindAsync(r);
        if (reagent != null)
        {
            spellExists.Reagents.Add(reagent);
        }
    }
    spellExists.Name = updatedSpell.Name;

    await context.SaveChangesAsync();

    return TypedResults.NoContent();
});

app.MapPut("/reagents/{reagentId:Guid}", async Task<Results<NotFound, NoContent>>
    ([FromRoute] Guid reagentId, [FromBody] ReagentCreate updatedReagent, SpellBookContext context) =>
{
    var reagentExists = await context.Reagents.FindAsync(reagentId);
    if (reagentExists == null)
    {
        return TypedResults.NotFound();
    }

    reagentExists.Name = updatedReagent.Name;
    await context.SaveChangesAsync();

    return TypedResults.NoContent();
});
#endregion PutEndpoints

// Map the DELETE endpoints
#region DeleteEndpoints

app.MapDelete("/spells/{spellId:Guid}", async Task<Results<NotFound, NoContent>>
    ([FromRoute] Guid spellId, SpellBookContext context) =>
{
    var spell = await context.Spells.FindAsync(spellId);
    if (spell == null)
    {
        return TypedResults.NotFound();
    }

    context.Remove(spell);
    await context.SaveChangesAsync();

    return TypedResults.NoContent();
});

app.MapDelete("/reagents/{reagentId:Guid}", async Task<Results<NotFound, NoContent>>
    ([FromRoute] Guid reagentId, SpellBookContext context) =>
{

    // Could potentially refuse to delete if any existing spells use this reagent
    // context.Spells.Where(s => s.Reagents.Any(r => r.Id == reagentId)).ToList();
    // if that's not an empty list, send a bad request response 

    var reagent = await context.Reagents.FindAsync(reagentId);
    if (reagent == null)
    {
        return TypedResults.NotFound();
    }

    context.Remove(reagent);
    await context.SaveChangesAsync();

    return TypedResults.NoContent();
});

#endregion DeleteEndpoints

app.UseHttpsRedirection();

//We're just learning and testing here, let's wipe/rebuild the SQLite DB on each run
using (var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var content = scope.ServiceProvider.GetRequiredService<SpellBookContext>();
    content.Database.EnsureDeleted();
    content.Database.Migrate();
}

app.Run();