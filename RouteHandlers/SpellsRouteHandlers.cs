using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpellBookApi.Contexts;
using SpellBookApi.Models;
using SpellBookApi.Models.Creates;
using SpellBookApi.Models.Entities;
using SpellBookApi.Models.Views;

namespace SpellBookApi.RouteHandlers;

public static class SpellsRouteHandlers
{

    public static async Task<Ok<IEnumerable<SpellView>>> GetSpells
        (SpellBookContext context)
    {
        var results = await context.Spells
        .Include(r => r.Reagents)
        .ToListAsync();

        return TypedResults.Ok<IEnumerable<SpellView>>(results.Select(s => s.ToView()).ToList());
    }

    public static async Task<Results<NotFound, Ok<SpellView>>> GetSpell
        ([FromRoute] Guid spellId, SpellBookContext context)
    {
        var result = await context.Spells
        .Include(r => r.Reagents)
        .FirstOrDefaultAsync(i => i.Id == spellId);

        if (result == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(result.ToView());
    }

    public static async Task<CreatedAtRoute<SpellView>> CreateSpell
        ([FromBody] SpellCreate newSpell, SpellBookContext context)
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

        return TypedResults.CreatedAtRoute(x.Entity.ToView(), "GetSpell", new { SpellId = x.Entity.Id });
    }

    public static async Task<Results<NotFound, NoContent>> UpdateSpell
        ([FromRoute] Guid spellId, [FromBody] SpellCreate updatedSpell, SpellBookContext context, ILogger<Spell> logger)
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

        logger.LogWarning("Spell {spellId} was modified", spellId);

        return TypedResults.NoContent();
    }

    public static async Task<Results<NotFound, NoContent>> DeleteSpell
        ([FromRoute] Guid spellId, SpellBookContext context, ILogger<Spell> logger)
    {
        var spell = await context.Spells.FindAsync(spellId);
        if (spell == null)
        {
            return TypedResults.NotFound();
        }

        context.Remove(spell);
        await context.SaveChangesAsync();

        logger.LogWarning("Spell {spellId} was deleted", spellId);

        return TypedResults.NoContent();
    }
}
