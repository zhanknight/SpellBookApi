using Microsoft.EntityFrameworkCore;
using SpellBookApi.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SpellBookContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("SpellBook"));
});

var app = builder.Build();

// Map the endpoints

app.MapGet("/spells", async (SpellBookContext context) =>
{ 
    return await context.Spells.ToListAsync(); 
});

app.MapGet("/reagents", async (SpellBookContext context) =>
{
    return await context.Reagents.ToListAsync();
});

app.MapGet("/spells/{spellId:Guid}", async (Guid spellId, SpellBookContext context) =>
{
    return await context.Spells.FindAsync(spellId);
});

app.MapGet("/reagents/{reagentId:Guid}", async (Guid reagentId, SpellBookContext context) =>
{
    return await context.Reagents.FindAsync(reagentId);
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
