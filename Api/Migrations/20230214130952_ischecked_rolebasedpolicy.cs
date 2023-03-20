using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoDynamicRolePolicy.Migrations
{
    public partial class ischecked_rolebasedpolicy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isChecked",
                table: "RoleBasedPolicy",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isChecked",
                table: "RoleBasedPolicy");
        }
    }
}
