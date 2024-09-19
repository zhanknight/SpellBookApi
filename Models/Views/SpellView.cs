namespace SpellBookApi.Models.Views;

public class SpellView
{
    public Guid Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public int ReagentCount { get { return Reagents.Count; } }   
    public ICollection<ReagentView> Reagents { get; set; } = new List<ReagentView>();
}
