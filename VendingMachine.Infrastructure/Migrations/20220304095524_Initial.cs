using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendingMachine.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Slots",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Slots_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "dbo",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Slots_ProductId",
                schema: "dbo",
                table: "Slots",
                column: "ProductId");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name" },
                columnTypes: new[] { "int", "nvarchar" },
                values: new object[,]{
                    { 1, "Tea" },
                    { 2, "Espresso" },
                    { 3, "Juice" },
                    { 4, "Chicken soup" }
                });

            migrationBuilder.InsertData(
                table: "Slots",
                columns: new[] { "Id", "Price", "ProductId", "Quantity" },
                columnTypes: new[] { "int", "int", "int", "int" },
                values: new object[,]{
                    { 1, 130, 1, 10 },
                    { 2, 180, 2, 20 },
                    { 3, 180, 3, 20 },
                    { 4, 180, 4, 15 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Slots",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "dbo");
        }
    }
}
