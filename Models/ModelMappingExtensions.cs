using SpellBookApi.Models.Entities;
using SpellBookApi.Models.Views;
using SpellBookApi.Models.Creates;

namespace SpellBookApi.Models;

public static class ModelMappingExtensions
{

    // Entities to Views

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

    // Creates to Entities

    public static Spell ToEntity(this SpellCreate spell)
    {
        return new Spell
        {
            Name = spell.Name,
            Reagents = new List<Reagent>()
        };
    }

    public static Reagent ToEntity(this ReagentCreate reagent)
    {
        return new Reagent
        {
            Name = reagent.Name
        };
    }

}
