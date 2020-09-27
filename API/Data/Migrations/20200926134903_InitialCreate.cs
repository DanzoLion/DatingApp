using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)                                           // Up 'adds' table initialised new table create function
        {
            migrationBuilder.CreateTable(
                name: "Users",                                                                                                                              // creates table called users
                columns: table => new                                                                                                              // creates columns Id / Users
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)                                        // Id column an int
                        .Annotation("Sqlite:Autoincrement", true),                                                            // primary key so will auto increment for us // by convension Id is primary key 
                    UserName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)                             // Down 'drops' the table and removes it // remove table function
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
