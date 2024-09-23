using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpellBookApi.Contexts;
using SpellBookApi.Models;
using SpellBookApi.Models.Creates;
using SpellBookApi.Models.Entities;
using SpellBookApi.Models.Views;

namespace SpellBookApi.RouteHandlers;

public class ReagentsRouteHandlers
{

    public static async Task<Ok<IEnumerable<ReagentView>>> GetReagents
        (SpellBookContext context)
    {
        var results = await context.Reagents.ToListAsync();

        return TypedResults.Ok<IEnumerable<ReagentView>>(results.Select(r => r.ToView()).ToList());
    }

    public static async Task<Results<NotFound, Ok<ReagentView>>> GetReagent
        ([FromRoute] Guid reagentId, SpellBookContext context)
    {
        var result = await context.Reagents.FindAsync(reagentId);

        if (result == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result.ToView());
    }

    public static async Task<CreatedAtRoute<ReagentView>> CreateReagent
        ([FromBody] ReagentCreate newReagent, SpellBookContext context)
    {
        var x = context.Add(newReagent.ToEntity());
        await context.SaveChangesAsync();
        // validate doesn't exist already

        return TypedResults.CreatedAtRoute(x.Entity.ToView(), "GetReagent", new { reagentId = x.Entity.Id });
    }

    public static async Task<Results<NotFound, NoContent>> UpdateReagent
        ([FromRoute] Guid reagentId, [FromBody] ReagentCreate updatedReagent, SpellBookContext context, ILogger<Reagent> logger)
    {
        var reagentExists = await context.Reagents.FindAsync(reagentId);
        if (reagentExists == null)
        {
            return TypedResults.NotFound();
        }

        reagentExists.Name = updatedReagent.Name;
        await context.SaveChangesAsync();

        logger.LogWarning("Reagent {reagentId} was modified", reagentId);

        return TypedResults.NoContent();
    }

    public static async Task<Results<NotFound, NoContent>> DeleteReagent
        ([FromRoute] Guid reagentId, SpellBookContext context, ILogger<Reagent> logger)
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

        logger.LogWarning("Reagent {reagentId} was deleted", reagentId);

        return TypedResults.NoContent();
    }
}
