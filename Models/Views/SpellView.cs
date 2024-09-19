namespace SpellBookApi.Models.Views;

public class SpellView
{
    public Guid Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public ICollection<ReagentView> Reagents { get; set; } = new List<ReagentView>();
}
