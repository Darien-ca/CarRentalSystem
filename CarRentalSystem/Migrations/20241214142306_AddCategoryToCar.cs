using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "40340c2c-3485-4808-93cd-4d9c1a874b2e", "AQAAAAIAAYagAAAAEEI4eyYmU381hYzTF5/IVi7FMHSDKBygKGuSYd0rMj5DmkGsL+JiixTxWuzyoO6G9A==", "62e3eb85-39f9-4416-a182-a0239ea0b35d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Cars");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "77d5c9bc-b8ce-4c52-8118-de43684da836", "AQAAAAIAAYagAAAAEI2iZygN3hd2FsYuo0tUPT2Q/rGYcnDZWY0v1CPvrHVY2zc4NKfLlQlBisdl8hdp0Q==", "03166217-3c3b-46e5-a5c7-d2e5a88e1620" });
        }
    }
}
