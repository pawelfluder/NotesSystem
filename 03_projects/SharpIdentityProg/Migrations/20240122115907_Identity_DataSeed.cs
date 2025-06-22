using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharpIdentityProg.Migrations
{
    /// <inheritdoc />
    public partial class Identity_DataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[]
                {
                    "Id",
                    "AccessFailedCount",
                    "ConcurrencyStamp",
                    "Email",
                    "EmailConfirmed",
                    "LockoutEnabled",
                    "LockoutEnd",
                    "NormalizedEmail",
                    "NormalizedUserName",
                    "PasswordHash",
                    "PhoneNumber",
                    "PhoneNumberConfirmed",
                    "SecurityStamp",
                    "TwoFactorEnabled",
                    "UserName"
                },
                values: new object[]
                {
                    "ce8613d0-eb5a-44e4-b163-ed82953efaed",
                    0,
                    "38d8260b-c9d8-4987-9613-bb6d4395d5ae",
                    "admin@gmail.com",
                    false,
                    true,
                    null,
                    "ADMIN@GMAIL.COM",
                    "ADMIN@GMAIL.COM",
                    "AQAAAAIAAYagAAAAEISyf++HdEdybtYX1hqN1fvsyTu+ZoNe+gJUrYmE9pQ3NJGv4gT5qyfcqEu6eeZm5Q==",
                    null,
                    false,
                    "LZQDPMF7MNX6HKQHE6OUPQT33W2SV7RA",
                    false,
                    "admin@gmail.com"
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ce8613d0-eb5a-44e4-b163-ed82953efaed");
        }
    }
}
