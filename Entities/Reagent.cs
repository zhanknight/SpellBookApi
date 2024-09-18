using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SpellBookApi.Entities;

public class Reagent
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    public ICollection<Spell> Spells { get; set; } = new List<Spell>();

    public Reagent()
    { }

    [SetsRequiredMembers]
    public Reagent(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
}
