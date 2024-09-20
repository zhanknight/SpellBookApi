namespace SpellBookApi.Models.Creates;

public class SpellCreate
{
    public required string Name { get; set; } = string.Empty;
    public ICollection<Guid> Reagents { get; set; } = new List<Guid>();
}
