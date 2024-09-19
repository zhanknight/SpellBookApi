using SpellBookApi.Models.Entities;
using SpellBookApi.Models.Views;

namespace SpellBookApi.Models;

public static class ModelMappingExtensions
{

    public static SpellView ToView(this Spell spell)
    {
        return new SpellView
        { 
            Name = spell.Name,
            Id = spell.Id,
            Reagents = spell.Reagents.Select(r => r.ToView()).ToList()
        };

    }

    public static ReagentView ToView(this Reagent reagent)
    {
        return new ReagentView
        {
            Name = reagent.Name,
            Id = reagent.Id,
        };
    }
}
