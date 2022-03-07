using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendingMachine.Infrastructure.Migrations
{
    public partial class AddedWalletsandCoins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coins",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerWalletCoins",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoinId = table.Column<int>(type: "int", nullable: false),
                    NumberOfCoins = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerWalletCoins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerWalletCoins_Coins_CoinId",
                        column: x => x.CoinId,
                        principalSchema: "dbo",
                        principalTable: "Coins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MachineWalletCoins",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoinId = table.Column<int>(type: "int", nullable: false),
                    NumberOfCoins = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineWalletCoins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MachineWalletCoins_Coins_CoinId",
                        column: x => x.CoinId,
                        principalSchema: "dbo",
                        principalTable: "Coins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWalletCoins_CoinId",
                schema: "dbo",
                table: "CustomerWalletCoins",
                column: "CoinId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineWalletCoins_CoinId",
                schema: "dbo",
                table: "MachineWalletCoins",
                column: "CoinId");

            migrationBuilder.InsertData(
                table: "Coins",
                columns: new[] { "Id", "Description", "Value" },
                columnTypes: new[] { "int", "nvarchar", "int" },
                values: new object[,]{
                    { 1, "10 cent", 10 },
                    { 2, "20 cent", 20 },
                    { 3, "50 cent", 50 },
                    { 4, "1 euro", 100 }
                });

            migrationBuilder.InsertData(
                table: "MachineWalletCoins",
                columns: new[] { "Id", "CoinId", "NumberOfCoins" },
                columnTypes: new[] { "int", "int", "int" },
                values: new object[,]{
                    { 1, 1, 100 },
                    { 2, 2, 100 },
                    { 3, 3, 100 },
                    { 4, 4, 100 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerWalletCoins",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MachineWalletCoins",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Coins",
                schema: "dbo");
        }
    }
}
