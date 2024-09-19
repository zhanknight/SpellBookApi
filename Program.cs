using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpellBookApi.Contexts;
using SpellBookApi.Models;
using System.Reflection.Metadata.Ecma335;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SpellBookContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("SpellBook"));
});

var app = builder.Build();

// Map the endpoints

app.MapGet("/spells", async (SpellBookContext context) =>
{ 
    var results = await context.Spells
    .Include(r => r.Reagents)
    .ToListAsync(); 
    return results.Select(s => s.ToView()).ToList();
});

app.MapGet("/reagents", async (SpellBookContext context) =>
{
    var results = await context.Reagents.ToListAsync();
    return results.Select(r => r.ToView()).ToList();
});

app.MapGet("/spells/{spellId:Guid}", async (Guid spellId, SpellBookContext context) =>
{
    var result = await context.Spells
    .Include(r => r.Reagents)
    .FirstOrDefaultAsync(i => i.Id == spellId);
    return result.ToView();

});

app.MapGet("/reagents/{reagentId:Guid}", async (Guid reagentId, SpellBookContext context) =>
{
    var result = await context.Reagents.FindAsync(reagentId);
    return result.ToView();
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
