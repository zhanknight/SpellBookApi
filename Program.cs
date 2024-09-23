using Microsoft.EntityFrameworkCore;
using SpellBookApi.Contexts;
using SpellBookApi.RouteHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SpellBookContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("SpellBook"));
});

builder.Services.AddProblemDetails();

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdmin", policy => policy.RequireRole("admin"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

RouteGroupBuilder spellsEndpoints = app.MapGroup("/spells").RequireAuthorization();
RouteGroupBuilder reagentsEndpoints = app.MapGroup("/reagents").RequireAuthorization();

spellsEndpoints.MapGet("", SpellsRouteHandlers.GetSpells)
    .AllowAnonymous();
spellsEndpoints.MapGet("/{spellId:Guid}", SpellsRouteHandlers.GetSpell)
    .WithName("GetSpell");
spellsEndpoints.MapPost("", SpellsRouteHandlers.CreateSpell)
    .RequireAuthorization("RequireAdmin");
spellsEndpoints.MapPut("/{spellId:Guid}", SpellsRouteHandlers.UpdateSpell)
    .RequireAuthorization("RequireAdmin");
spellsEndpoints.MapDelete("/{spellId:Guid}", SpellsRouteHandlers.DeleteSpell)
    .RequireAuthorization("RequireAdmin");

reagentsEndpoints.MapGet("", ReagentsRouteHandlers.GetReagents)
    .AllowAnonymous();
reagentsEndpoints.MapGet("/{reagentId:Guid}", ReagentsRouteHandlers.GetReagent)
    .WithName("GetReagent");
reagentsEndpoints.MapPost("", ReagentsRouteHandlers.CreateReagent)
    .RequireAuthorization("RequireAdmin");
reagentsEndpoints.MapPut("/{reagentId:Guid}", ReagentsRouteHandlers.UpdateReagent)
    .RequireAuthorization("RequireAdmin");
reagentsEndpoints.MapDelete("/{reagentId:Guid}", ReagentsRouteHandlers.DeleteReagent)
    .RequireAuthorization("RequireAdmin");

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();


//We're just learning and testing here, let's wipe/rebuild the SQLite DB on each run
using (var scope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    var content = scope.ServiceProvider.GetRequiredService<SpellBookContext>();
    content.Database.EnsureDeleted();
    content.Database.Migrate();
}

app.Run();