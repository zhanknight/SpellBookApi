using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SpellBookApi.Entities;

public class Spell
{

    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    public ICollection<Reagent> Reagents { get; set; } = new List<Reagent>();

    public Spell()
    {
    }

    [SetsRequiredMembers]
    public Spell(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

}
