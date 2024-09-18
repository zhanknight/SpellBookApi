using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SpellBookApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reagents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reagents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Spells",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spells", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReagentSpell",
                columns: table => new
                {
                    ReagentsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SpellsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReagentSpell", x => new { x.ReagentsId, x.SpellsId });
                    table.ForeignKey(
                        name: "FK_ReagentSpell_Reagents_ReagentsId",
                        column: x => x.ReagentsId,
                        principalTable: "Reagents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReagentSpell_Spells_SpellsId",
                        column: x => x.SpellsId,
                        principalTable: "Spells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Reagents",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0c4dc798-b38b-4a1c-905c-a9e76dbef17b"), "Nightshade" },
                    { new Guid("7a2fbc72-bb33-49de-bd23-c78fceb367fc"), "Mandrake Root" },
                    { new Guid("937b1ba1-7969-4324-9ab5-afb0e4d875e6"), "Ginseng" },
                    { new Guid("b5f336e2-c226-4389-aac3-2499325a3de9"), "Spiders Silk" },
                    { new Guid("c19099ed-94db-44ba-885b-0ad7205d5e40"), "Garlic" },
                    { new Guid("c22bec27-a880-4f2a-b380-12dcd99c61fe"), "Sulfurous Ash" },
                    { new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), "Black Pearl" },
                    { new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), "Blood Moss" }
                });

            migrationBuilder.InsertData(
                table: "Spells",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("98929bd4-f099-41eb-a994-f1918b724b5a"), "Summon" },
                    { new Guid("b512d7cf-b331-4b54-8dae-d1228d128e8d"), "Greater Heal" },
                    { new Guid("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06"), "Flamestrike" },
                    { new Guid("fd630a57-2352-4731-b25c-db9cc7601b16"), "Portal" },
                    { new Guid("fe462ec7-b30c-4987-8a8e-5f7dbd8e0cfa"), "Teleport" }
                });

            migrationBuilder.InsertData(
                table: "ReagentSpell",
                columns: new[] { "ReagentsId", "SpellsId" },
                values: new object[,]
                {
                    { new Guid("0c4dc798-b38b-4a1c-905c-a9e76dbef17b"), new Guid("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06") },
                    { new Guid("7a2fbc72-bb33-49de-bd23-c78fceb367fc"), new Guid("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06") },
                    { new Guid("937b1ba1-7969-4324-9ab5-afb0e4d875e6"), new Guid("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06") },
                    { new Guid("b5f336e2-c226-4389-aac3-2499325a3de9"), new Guid("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06") },
                    { new Guid("c19099ed-94db-44ba-885b-0ad7205d5e40"), new Guid("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06") },
                    { new Guid("c22bec27-a880-4f2a-b380-12dcd99c61fe"), new Guid("98929bd4-f099-41eb-a994-f1918b724b5a") },
                    { new Guid("c22bec27-a880-4f2a-b380-12dcd99c61fe"), new Guid("b512d7cf-b331-4b54-8dae-d1228d128e8d") },
                    { new Guid("c22bec27-a880-4f2a-b380-12dcd99c61fe"), new Guid("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06") },
                    { new Guid("c22bec27-a880-4f2a-b380-12dcd99c61fe"), new Guid("fd630a57-2352-4731-b25c-db9cc7601b16") },
                    { new Guid("c22bec27-a880-4f2a-b380-12dcd99c61fe"), new Guid("fe462ec7-b30c-4987-8a8e-5f7dbd8e0cfa") },
                    { new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), new Guid("b512d7cf-b331-4b54-8dae-d1228d128e8d") },
                    { new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), new Guid("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06") },
                    { new Guid("d28888e9-2ba9-473a-a40f-e38cb54f9b35"), new Guid("fd630a57-2352-4731-b25c-db9cc7601b16") },
                    { new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), new Guid("98929bd4-f099-41eb-a994-f1918b724b5a") },
                    { new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), new Guid("b512d7cf-b331-4b54-8dae-d1228d128e8d") },
                    { new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), new Guid("eacc5169-b2a7-41ad-92c3-dbb1a5e7af06") },
                    { new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), new Guid("fd630a57-2352-4731-b25c-db9cc7601b16") },
                    { new Guid("da2fd609-d754-4feb-8acd-c4f9ff13ba96"), new Guid("fe462ec7-b30c-4987-8a8e-5f7dbd8e0cfa") },
                    { new Guid("e0017fe1-773f-4a59-9730-9489833c6e8e"), new Guid("fd630a57-2352-4731-b25c-db9cc7601b16") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReagentSpell_SpellsId",
                table: "ReagentSpell",
                column: "SpellsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReagentSpell");

            migrationBuilder.DropTable(
                name: "Reagents");

            migrationBuilder.DropTable(
                name: "Spells");
        }
    }
}
