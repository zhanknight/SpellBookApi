using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SpellBookApi.Contexts;
using SpellBookApi.RouteHandlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SpellBookContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("SpellBook"));
});

builder.Services.AddProblemDetails();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x => 
    { 
        x.Authority = "https://localhost"; 
        x.Audience = "SpellBookApi";
    });
builder.Services.AddAuthorization();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdmin", policy => policy.RequireRole("admin"))
    .AddPolicy("RequireCreate", policy => policy.RequireClaim("permission", "create"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("jwt",
        new()
        {
            Name = "Authorization",
            Description = "JWT Bearer Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            In = ParameterLocation.Header
        });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "jwt"
                    }
                },
                new string[] { }
            }
        });
});

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
    .RequireAuthorization("RequireCreate");
spellsEndpoints.MapPut("/{spellId:Guid}", SpellsRouteHandlers.UpdateSpell)
    .RequireAuthorization("RequireAdmin");
spellsEndpoints.MapDelete("/{spellId:Guid}", SpellsRouteHandlers.DeleteSpell)
    .RequireAuthorization("RequireAdmin");

reagentsEndpoints.MapGet("", ReagentsRouteHandlers.GetReagents)
    .AllowAnonymous();
reagentsEndpoints.MapGet("/{reagentId:Guid}", ReagentsRouteHandlers.GetReagent)
    .WithName("GetReagent");
reagentsEndpoints.MapPost("", ReagentsRouteHandlers.CreateReagent)
    .RequireAuthorization("RequireCreate");
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