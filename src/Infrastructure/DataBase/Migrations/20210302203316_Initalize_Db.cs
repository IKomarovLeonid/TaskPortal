using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataBase.Migrations
{
    public partial class Initalize_Db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "generate_results",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    task_id = table.Column<ulong>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    Tasktype = table.Column<int>(name: "Task type", nullable: false),
                    requested_count = table.Column<ulong>(nullable: false),
                    processed_count = table.Column<ulong>(nullable: false),
                    error_count = table.Column<ulong>(nullable: false),
                    updated_time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_generate_results", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "servers",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(64)", nullable: false),
                    activity = table.Column<bool>(nullable: false),
                    state = table.Column<int>(nullable: false),
                    settings = table.Column<string>(type: "json", nullable: true),
                    created_time = table.Column<DateTime>(nullable: false),
                    updated_time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_servers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "settings",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: false),
                    jobs = table.Column<string>(type: "json", nullable: true),
                    state = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "task_configurations",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: false),
                    is_active = table.Column<bool>(nullable: false),
                    state = table.Column<int>(nullable: false),
                    settings = table.Column<string>(type: "json", nullable: true),
                    created_time = table.Column<DateTime>(nullable: false),
                    updated_time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_task_configurations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "accounts_tasks",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    configuration_id = table.Column<ulong>(nullable: false),
                    server_id = table.Column<ulong>(nullable: false),
                    count = table.Column<ulong>(nullable: false),
                    state = table.Column<int>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    result = table.Column<int>(nullable: false),
                    activity = table.Column<bool>(nullable: false),
                    account_name = table.Column<string>(nullable: true),
                    password = table.Column<string>(nullable: true),
                    groups = table.Column<string>(nullable: true),
                    leverage = table.Column<ulong>(nullable: false),
                    min_balance = table.Column<double>(nullable: false),
                    max_balance = table.Column<double>(nullable: false),
                    min_credit = table.Column<double>(nullable: false),
                    max_credit = table.Column<double>(nullable: false),
                    created_time = table.Column<DateTime>(nullable: false),
                    updated_time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts_tasks", x => x.id);
                    table.ForeignKey(
                        name: "FK_accounts_tasks_servers_configuration_id",
                        column: x => x.configuration_id,
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "clean_group_tasks",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    configuration_id = table.Column<ulong>(nullable: false),
                    server_id = table.Column<ulong>(nullable: false),
                    state = table.Column<int>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    result = table.Column<int>(nullable: false),
                    activity = table.Column<bool>(nullable: false),
                    group = table.Column<string>(nullable: true),
                    created_time = table.Column<DateTime>(nullable: false),
                    updated_time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clean_group_tasks", x => x.id);
                    table.ForeignKey(
                        name: "FK_clean_group_tasks_servers_configuration_id",
                        column: x => x.configuration_id,
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pending_orders_tasks",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    configuration_id = table.Column<ulong>(nullable: false),
                    server_id = table.Column<ulong>(nullable: false),
                    state = table.Column<int>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    activity = table.Column<bool>(nullable: false),
                    total_count = table.Column<uint>(nullable: false),
                    per_account_count = table.Column<uint>(nullable: false),
                    group = table.Column<string>(nullable: true),
                    pending_type = table.Column<int>(nullable: false),
                    price = table.Column<decimal>(nullable: false),
                    created_time = table.Column<DateTime>(nullable: false),
                    updated_time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pending_orders_tasks", x => x.id);
                    table.ForeignKey(
                        name: "FK_pending_orders_tasks_servers_configuration_id",
                        column: x => x.configuration_id,
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "positions_tasks",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    configuration_id = table.Column<ulong>(nullable: false),
                    server_id = table.Column<ulong>(nullable: false),
                    count = table.Column<ulong>(nullable: false),
                    state = table.Column<int>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    activity = table.Column<bool>(nullable: false),
                    direction = table.Column<int>(nullable: false),
                    min_price = table.Column<decimal>(nullable: false),
                    max_price = table.Column<decimal>(nullable: false),
                    min_lots = table.Column<decimal>(nullable: false),
                    max_lots = table.Column<decimal>(nullable: false),
                    symbols = table.Column<string>(nullable: true),
                    groups = table.Column<string>(nullable: true),
                    take_profit = table.Column<ulong>(nullable: false),
                    stop_loss = table.Column<ulong>(nullable: false),
                    created_time = table.Column<DateTime>(nullable: false),
                    updated_time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_positions_tasks", x => x.id);
                    table.ForeignKey(
                        name: "FK_positions_tasks_servers_configuration_id",
                        column: x => x.configuration_id,
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ticks_tasks",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    configuration_id = table.Column<ulong>(nullable: false),
                    server_id = table.Column<ulong>(nullable: false),
                    state = table.Column<int>(nullable: false),
                    status = table.Column<int>(nullable: false),
                    result = table.Column<int>(nullable: false),
                    activity = table.Column<bool>(nullable: false),
                    bid = table.Column<decimal>(nullable: false),
                    ask = table.Column<decimal>(nullable: false),
                    spread = table.Column<decimal>(nullable: false),
                    symbols = table.Column<string>(nullable: true),
                    count = table.Column<ulong>(nullable: false),
                    process_time = table.Column<TimeSpan>(nullable: false),
                    created_time = table.Column<DateTime>(nullable: false),
                    updated_time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticks_tasks", x => x.id);
                    table.ForeignKey(
                        name: "FK_ticks_tasks_servers_configuration_id",
                        column: x => x.configuration_id,
                        principalTable: "servers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_tasks_configuration_id",
                table: "accounts_tasks",
                column: "configuration_id");

            migrationBuilder.CreateIndex(
                name: "IX_clean_group_tasks_configuration_id",
                table: "clean_group_tasks",
                column: "configuration_id");

            migrationBuilder.CreateIndex(
                name: "IX_pending_orders_tasks_configuration_id",
                table: "pending_orders_tasks",
                column: "configuration_id");

            migrationBuilder.CreateIndex(
                name: "IX_positions_tasks_configuration_id",
                table: "positions_tasks",
                column: "configuration_id");

            migrationBuilder.CreateIndex(
                name: "IX_ticks_tasks_configuration_id",
                table: "ticks_tasks",
                column: "configuration_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounts_tasks");

            migrationBuilder.DropTable(
                name: "clean_group_tasks");

            migrationBuilder.DropTable(
                name: "generate_results");

            migrationBuilder.DropTable(
                name: "pending_orders_tasks");

            migrationBuilder.DropTable(
                name: "positions_tasks");

            migrationBuilder.DropTable(
                name: "settings");

            migrationBuilder.DropTable(
                name: "task_configurations");

            migrationBuilder.DropTable(
                name: "ticks_tasks");

            migrationBuilder.DropTable(
                name: "servers");
        }
    }
}
