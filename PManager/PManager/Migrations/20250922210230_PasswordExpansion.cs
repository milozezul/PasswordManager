using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PManager.Migrations
{
    /// <inheritdoc />
    public partial class PasswordExpansion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Value",
                table: "Passwords",
                type: "varbinary(max)",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Passwords",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Passwords");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Value",
                table: "Passwords",
                type: "varbinary(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");
        }
    }
}
