using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Infrastructure.Migrations
{
    public partial class Add_LeaveTypeTable_LeaveTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeaveTypes",
                columns: table => new
                {
                    LeaveTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaveTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveTypes", x => x.LeaveTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Leaves",
                columns: table => new
                {
                    LeaveID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LeaveTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leaves", x => x.LeaveID);
                    table.ForeignKey(
                        name: "FK_Leaves_AspNetUsers_UserID",
                        column: x => x.UserID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Leaves_LeaveTypes_LeaveTypeID",
                        column: x => x.LeaveTypeID,
                        principalTable: "LeaveTypes",
                        principalColumn: "LeaveTypeID");
                });

            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "LeaveTypeID", "LeaveTypeName" },
                values: new object[,]
                {
                    { new Guid("35dd578a-33ef-4b6f-8afc-f289445da9b5"), "Vacation" },
                    { new Guid("9bdfe5c1-fd15-4c47-b294-cd6e66cc4d06"), "Sick leave" },
                    { new Guid("a1abac03-5946-49b0-963c-d8a2c7392e1f"), "Sabbatical" },
                    { new Guid("a5415b00-3248-4d5b-8e41-02c6b1f21303"), "Annual leave" },
                    { new Guid("ba1ab040-825b-4f5d-bc53-49ed2075c1f8"), "Maternity / Paternity" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_LeaveTypeID",
                table: "Leaves",
                column: "LeaveTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_UserID",
                table: "Leaves",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Leaves");

            migrationBuilder.DropTable(
                name: "LeaveTypes");
        }
    }
}
