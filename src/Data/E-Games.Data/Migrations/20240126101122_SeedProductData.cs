using E_Games.Data.Data.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Games.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedProductData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var random = new Random();
            var enumLength = Enum.GetNames(typeof(Platforms)).Length;

            string[] gameNames = new string[]
            {
                "The Witcher 3: Wild Hunt", "Red Dead Redemption 2", "Grand Theft Auto V", "The Legend of Zelda",
                "Dark Souls III", "The Elder Scrolls V: Skyrim", "Sekiro: Shadows Die Twice", "Bloodborne",
                "Mass Effect 2", "Hollow Knight", "The Last of Us Part II", "God of War",
                "Final Fantasy VII", "Horizon Zero Dawn", "Minecraft", "Cyberpunk 2077",
                "Super Mario", "Counter-Strike: Global Offensive", "Resident Evil", "Ghost of Tsushima",
                "NieR: Automata", "Death Stranding", "Demon's Souls", "Star Wars Jedi: Fallen Order",
                "Control", "Bioshock Infinite", "Shadow of the Colossus", "Dragon Age: Inquisition",
                "FIFA 23", "Fallout: New Vegas"
            };

            object[,] games = new object[gameNames.Length, 4];

            for (int i = 0; i < gameNames.Length; i++)
            {
                games[i, 0] = gameNames[i];
                games[i, 1] = i % enumLength;
                games[i, 2] = DateTime.UtcNow.AddDays(-random.Next(0, 365 * 10));
                games[i, 3] = random.Next(1, 6);
            }

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Name", "Platform", "DateCreated", "TotalRating" },
                values: games
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string[] gameNames = new string[]
            {
                "The Witcher 3: Wild Hunt", "Red Dead Redemption 2", "Grand Theft Auto V", "The Legend of Zelda",
                "Dark Souls III", "The Elder Scrolls V: Skyrim", "Sekiro: Shadows Die Twice", "Bloodborne",
                "Mass Effect 2", "Hollow Knight", "The Last of Us Part II", "God of War",
                "Final Fantasy VII", "Horizon Zero Dawn", "Minecraft", "Cyberpunk 2077",
                "Super Mario", "Counter-Strike: Global Offensive", "Resident Evil", "Ghost of Tsushima",
                "NieR: Automata", "Death Stranding", "Demon's Souls", "Star Wars Jedi: Fallen Order",
                "Control", "Bioshock Infinite", "Shadow of the Colossus", "Dragon Age: Inquisition",
                "FIFA 23", "Fallout: New Vegas"
            };

            foreach (var gameName in gameNames)
            {
                migrationBuilder.DeleteData(
                    table: "Products",
                    keyColumn: "Name",
                    keyValue: gameName
                );
            }
        }
    }
}
