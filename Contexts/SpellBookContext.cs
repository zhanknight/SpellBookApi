using Microsoft.EntityFrameworkCore;
using SpellBookApi.Models.Entities;

namespace SpellBookApi.Contexts;

public class SpellBookContext : DbContext
{
    public SpellBookContext(DbContextOptions<SpellBookContext> options) : base(options)
    {
    }

    public DbSet<Spell> Spells { get; set; }
    public DbSet<Reagent> Reagents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Reagent>().HasData(
            new(Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), "Black Pearl"),
            new(Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), "Blood Moss"),
            new(Guid.Parse("c19099ed-94db-44ba-885b-0ad7205d5e40"), "Garlic"),
            new(Guid.Parse("0c4dc798-b38b-4a1c-905c-a9e76dbef17b"), "Nightshade"),
            new(Guid.Parse("937b1ba1-7969-4324-9ab5-afb0e4d875e6"), "Ginseng"),
            new(Guid.Parse("7a2fbc72-bb33-49de-bd23-c78fceb367fc"), "Mandrake Root"),
            new(Guid.Parse("b5f336e2-c226-4389-aac3-2499325a3de9"), "Spiders Silk"),
            new(Guid.Parse("c22bec27-a880-4f2a-b380-12dcd99c61fe"), "Sulfurous Ash")
            );

        _ = modelBuilder.Entity<Spell>().HasData(
           new(Guid.Parse("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"),
            "Flamestrike"),
           new(Guid.Parse("fe462ec7-b30c-4987-8a8e-5f7dbd8e0cfa"),
            "Teleport"),
           new(Guid.Parse("b512d7cf-b331-4b54-8dae-d1228d128e8d"),
           "Greater Heal"),
           new(Guid.Parse("fd630a57-2352-4731-b25c-db9cc7601b16"),
           "Portal"),
           new(Guid.Parse("98929bd4-f099-41eb-a994-f1918b724b5a"),
           "Summon"));

        _ = modelBuilder
            .Entity<Spell>()
            .HasMany(d => d.Reagents)
            .WithMany(i => i.Spells)
            .UsingEntity(e => e.HasData(
                new { SpellsId = Guid.Parse("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"), ReagentsId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35") },
                new { SpellsId = Guid.Parse("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"), ReagentsId = Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96") },
                new { SpellsId = Guid.Parse("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"), ReagentsId = Guid.Parse("c19099ed-94db-44ba-885b-0ad7205d5e40") },
                new { SpellsId = Guid.Parse("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"), ReagentsId = Guid.Parse("0c4dc798-b38b-4a1c-905c-a9e76dbef17b") },
                new { SpellsId = Guid.Parse("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"), ReagentsId = Guid.Parse("937b1ba1-7969-4324-9ab5-afb0e4d875e6") },
                new { SpellsId = Guid.Parse("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"), ReagentsId = Guid.Parse("7a2fbc72-bb33-49de-bd23-c78fceb367fc") },
                new { SpellsId = Guid.Parse("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"), ReagentsId = Guid.Parse("b5f336e2-c226-4389-aac3-2499325a3de9") },
                new { SpellsId = Guid.Parse("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"), ReagentsId = Guid.Parse("c22bec27-a880-4f2a-b380-12dcd99c61fe") },
                new { SpellsId = Guid.Parse("fe462ec7-b30c-4987-8a8e-5f7dbd8e0cfa"), ReagentsId = Guid.Parse("c22bec27-a880-4f2a-b380-12dcd99c61fe") },
                new { SpellsId = Guid.Parse("fe462ec7-b30c-4987-8a8e-5f7dbd8e0cfa"), ReagentsId = Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96") },
                new { SpellsId = Guid.Parse("b512d7cf-b331-4b54-8dae-d1228d128e8d"), ReagentsId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35") },
                new { SpellsId = Guid.Parse("b512d7cf-b331-4b54-8dae-d1228d128e8d"), ReagentsId = Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96") },
                new { SpellsId = Guid.Parse("b512d7cf-b331-4b54-8dae-d1228d128e8d"), ReagentsId = Guid.Parse("c22bec27-a880-4f2a-b380-12dcd99c61fe") },
                new { SpellsId = Guid.Parse("fd630a57-2352-4731-b25c-db9cc7601b16"), ReagentsId = Guid.Parse("d28888e9-2ba9-473a-a40f-e38cb54f9b35") },
                new { SpellsId = Guid.Parse("fd630a57-2352-4731-b25c-db9cc7601b16"), ReagentsId = Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96") },
                new { SpellsId = Guid.Parse("fd630a57-2352-4731-b25c-db9cc7601b16"), ReagentsId = Guid.Parse("e0017fe1-773f-4a59-9730-9489833c6e8e") },
                new { SpellsId = Guid.Parse("fd630a57-2352-4731-b25c-db9cc7601b16"), ReagentsId = Guid.Parse("c22bec27-a880-4f2a-b380-12dcd99c61fe") },
                new { SpellsId = Guid.Parse("98929bd4-f099-41eb-a994-f1918b724b5a"), ReagentsId = Guid.Parse("da2fd609-d754-4feb-8acd-c4f9ff13ba96") },
                new { SpellsId = Guid.Parse("98929bd4-f099-41eb-a994-f1918b724b5a"), ReagentsId = Guid.Parse("c22bec27-a880-4f2a-b380-12dcd99c61fe") }
                ));

        base.OnModelCreating(modelBuilder);
    }
}

