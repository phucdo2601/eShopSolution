using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eShopSolution.Data.Migrations
{
    public partial class SeedIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 2, 20, 21, 33, 936, DateTimeKind.Local).AddTicks(3758),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 2, 2, 20, 1, 27, 769, DateTimeKind.Local).AddTicks(4966));

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[] { new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"), "a1149635-79fd-4a85-bfd3-47385b2883a1", "Administrator role", "admin", "admin" });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[] { new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"), new Guid("8d04dce2-969a-435d-bba4-df3f325983dc") });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Dob", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"), 0, "dd21a8c7-fb0b-4d0e-aa03-d6017986734d", new DateTime(2000, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "ngocphucdo2601@gmail.com", true, "Phuc", "Do", false, null, "ngocphucdo2601@gmail.com", "admin", "AQAAAAEAACcQAAAAEI7WhoD11slmgPMdcl6kRJV6i5T7p5keupBRg/u0aCVQkfRIijvpTaZBL8DjIjS0Tw==", null, false, "", false, "admin" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 2, 2, 20, 21, 33, 950, DateTimeKind.Local).AddTicks(9238));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"));

            migrationBuilder.DeleteData(
                table: "AppUserRoles",
                keyColumns: new[] { "UserId", "RoleId" },
                keyValues: new object[] { new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"), new Guid("8d04dce2-969a-435d-bba4-df3f325983dc") });

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: new Guid("69bd714f-9576-45ba-b5b7-f00649be00de"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 2, 20, 1, 27, 769, DateTimeKind.Local).AddTicks(4966),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2023, 2, 2, 20, 21, 33, 936, DateTimeKind.Local).AddTicks(3758));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2023, 2, 2, 20, 1, 27, 788, DateTimeKind.Local).AddTicks(8553));
        }
    }
}
