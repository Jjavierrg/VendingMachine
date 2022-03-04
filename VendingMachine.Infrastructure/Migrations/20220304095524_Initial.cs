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
                    { 1, "Product 1" },
                    { 2, "Product 2" },
                    { 3, "Product 3" },
                    { 4, "Product 4" },
                    { 5, "Product 5" },
                    { 6, "Product 6" },
                    { 7, "Product 7" },
                    { 8, "Product 8" },
                    { 9, "Product 9" },
                    { 10, "Product 10" }
                });

            migrationBuilder.InsertData(
                table: "Slots",
                columns: new[] { "Id", "Price", "ProductId", "Quantity" },
                columnTypes: new[] { "int", "int", "int", "int" },
                values: new object[,]{
                    { 1, 11, 1, 1 },
                    { 2, 22, 2, 2 },
                    { 3, 33, 3, 3 },
                    { 4, 44, 4, 4 },
                    { 5, 55, 5, 5 },
                    { 6, 66, 6, 6 },
                    { 7, 77, 7, 7 },
                    { 8, 88, 8, 8 },
                    { 9, 99, 9, 9 },
                    { 10, 100, 10, 10 }
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
