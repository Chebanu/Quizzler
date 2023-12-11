using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlashCards.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: false),
                    LanguageName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.LanguageId);
                });

            migrationBuilder.CreateTable(
                name: "Level",
                columns: table => new
                {
                    LevelId = table.Column<Guid>(type: "uuid", nullable: false),
                    LevelName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Level", x => x.LevelId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Word",
                columns: table => new
                {
                    WordId = table.Column<Guid>(type: "uuid", nullable: false),
                    WordText = table.Column<string>(type: "text", nullable: true),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: false),
                    LevelId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Word", x => x.WordId);
                    table.ForeignKey(
                        name: "FK_Word_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "LanguageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Word_Level_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Level",
                        principalColumn: "LevelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Translation",
                columns: table => new
                {
                    TranslationId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceWordId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetWordId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translation", x => x.TranslationId);
                    table.ForeignKey(
                        name: "FK_Translation_Word_SourceWordId",
                        column: x => x.SourceWordId,
                        principalTable: "Word",
                        principalColumn: "WordId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Translation_Word_TargetWordId",
                        column: x => x.TargetWordId,
                        principalTable: "Word",
                        principalColumn: "WordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Language",
                columns: new[] { "LanguageId", "LanguageName" },
                values: new object[,]
                {
                    { new Guid("14e4d292-a580-4a4d-9b0c-ed49a85179d2"), "Spanish" },
                    { new Guid("33526ce8-8842-49d6-8780-ba61be8069ab"), "French" },
                    { new Guid("54eae254-972b-4ce1-b08b-192f1ac2567f"), "English" },
                    { new Guid("8703ae8f-2886-46e9-a0ec-4a28b29ceb5a"), "German" },
                    { new Guid("92965b17-28a6-4336-b13f-d1ff1c1b1444"), "Italian" },
                    { new Guid("c840e6bf-2cfb-4eaf-8322-95bc9f70b975"), "Russian" }
                });

            migrationBuilder.InsertData(
                table: "Level",
                columns: new[] { "LevelId", "LevelName" },
                values: new object[,]
                {
                    { new Guid("026d2af9-62f1-441f-ab29-23ad4a1aae97"), "B2" },
                    { new Guid("47538f2b-04ee-4bcd-bc2e-810d462081bd"), "B1" },
                    { new Guid("4802a577-ed4c-4646-ad2b-b9604ceac574"), "A2" },
                    { new Guid("495996f4-ee53-4769-a942-832a706b8178"), "A1" },
                    { new Guid("ebd87f8f-2dfa-482d-bbb8-17d6ba5d2637"), "C2" },
                    { new Guid("f3323772-4d35-4d27-a1a3-f86453c5e6df"), "C1" }
                });

            migrationBuilder.InsertData(
                table: "Word",
                columns: new[] { "WordId", "ImageUrl", "LanguageId", "LevelId", "WordText" },
                values: new object[,]
                {
                    { new Guid("11237266-725e-45b2-b426-7abee30acfde"), "", new Guid("54eae254-972b-4ce1-b08b-192f1ac2567f"), new Guid("495996f4-ee53-4769-a942-832a706b8178"), "Hello" },
                    { new Guid("b977b0c1-1284-4f3a-8329-9040965386ad"), "", new Guid("c840e6bf-2cfb-4eaf-8322-95bc9f70b975"), new Guid("495996f4-ee53-4769-a942-832a706b8178"), "Привет" },
                    { new Guid("cd2225d1-a9d1-454f-af85-eac853bbd45c"), "", new Guid("92965b17-28a6-4336-b13f-d1ff1c1b1444"), new Guid("495996f4-ee53-4769-a942-832a706b8178"), "Ciao" },
                    { new Guid("df279714-8907-4800-ac56-2ea35b84f467"), "", new Guid("33526ce8-8842-49d6-8780-ba61be8069ab"), new Guid("495996f4-ee53-4769-a942-832a706b8178"), "Bonjour" },
                    { new Guid("f96065c6-6d30-430a-8c65-3ce715d6574c"), "", new Guid("8703ae8f-2886-46e9-a0ec-4a28b29ceb5a"), new Guid("495996f4-ee53-4769-a942-832a706b8178"), "Guten Tag" },
                    { new Guid("fe8dcbad-1001-4d63-8eca-a97bef5ec6ec"), "", new Guid("14e4d292-a580-4a4d-9b0c-ed49a85179d2"), new Guid("495996f4-ee53-4769-a942-832a706b8178"), "Hola" }
                });

            migrationBuilder.InsertData(
                table: "Translation",
                columns: new[] { "TranslationId", "SourceWordId", "TargetWordId" },
                values: new object[,]
                {
                    { new Guid("1ebac63e-6f19-4e3c-88d0-383083be1336"), new Guid("11237266-725e-45b2-b426-7abee30acfde"), new Guid("b977b0c1-1284-4f3a-8329-9040965386ad") },
                    { new Guid("45657d21-a8ef-4ea6-87a9-208a201b4f98"), new Guid("f96065c6-6d30-430a-8c65-3ce715d6574c"), new Guid("fe8dcbad-1001-4d63-8eca-a97bef5ec6ec") },
                    { new Guid("bdabcef1-c344-47d3-a4a8-e23fc03a1200"), new Guid("b977b0c1-1284-4f3a-8329-9040965386ad"), new Guid("11237266-725e-45b2-b426-7abee30acfde") },
                    { new Guid("ee963a82-4a19-49e4-84aa-8939ea4b9237"), new Guid("11237266-725e-45b2-b426-7abee30acfde"), new Guid("df279714-8907-4800-ac56-2ea35b84f467") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translation_SourceWordId_TargetWordId",
                table: "Translation",
                columns: new[] { "SourceWordId", "TargetWordId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Translation_TargetWordId",
                table: "Translation",
                column: "TargetWordId");

            migrationBuilder.CreateIndex(
                name: "IX_Word_LanguageId",
                table: "Word",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Word_LevelId",
                table: "Word",
                column: "LevelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Translation");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Word");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "Level");
        }
    }
}
