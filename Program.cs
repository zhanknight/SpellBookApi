using Microsoft.EntityFrameworkCore;
using SpellBookApi.Contexts;
using SpellBookApi.RouteHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SpellBookContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("SpellBook"));
});

builder.Services.AddProblemDetails();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

RouteGroupBuilder spellsEndpoints = app.MapGroup("/spells");
RouteGroupBuilder reagentsEndpoints = app.MapGroup("/reagents");

spellsEndpoints.MapGet("", SpellsRouteHandlers.GetSpells);
spellsEndpoints.MapGet("/{spellId:Guid}", SpellsRouteHandlers.GetSpell).WithName("GetSpell");
spellsEndpoints.MapPost("", SpellsRouteHandlers.CreateSpell);
spellsEndpoints.MapPut("/{spellId:Guid}", SpellsRouteHandlers.UpdateSpell);
spellsEndpoints.MapDelete("/{spellId:Guid}", SpellsRouteHandlers.DeleteSpell);

reagentsEndpoints.MapGet("", ReagentsRouteHandlers.GetReagents);
reagentsEndpoints.MapGet("/{reagentId:Guid}", ReagentsRouteHandlers.GetReagent).WithName("GetReagent");
reagentsEndpoints.MapPost("", ReagentsRouteHandlers.CreateReagent);
reagentsEndpoints.MapPut("/{reagentId:Guid}", ReagentsRouteHandlers.UpdateReagent);
reagentsEndpoints.MapDelete("/{reagentId:Guid}", ReagentsRouteHandlers.DeleteReagent);

app.UseHttpsRedirection();

//We're just learning and testing here, let's wipe/rebuild the SQLite DB on each run
using (var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var content = scope.ServiceProvider.GetRequiredService<SpellBookContext>();
    content.Database.EnsureDeleted();
    content.Database.Migrate();
}

app.Run();