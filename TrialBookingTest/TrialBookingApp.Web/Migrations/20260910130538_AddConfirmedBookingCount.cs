using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrialBookingApp.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddConfirmedBookingCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConfirmedBookingCount",
                table: "TrialClasses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmedBookingCount",
                table: "TrialClasses");
        }
    }
}
