using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    ChatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MaxParticipants = table.Column<int>(type: "int", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.ChatId);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Tag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ConnectionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhotoBlobId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastSeen = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.ProfileId);
                });

            migrationBuilder.CreateTable(
                name: "UserContacts",
                columns: table => new
                {
                    ContactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonalProfileId = table.Column<int>(type: "int", nullable: false),
                    ContactProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContacts", x => x.ContactId);
                    table.ForeignKey(
                        name: "FK_UserContacts_Profiles_ContactProfileId",
                        column: x => x.ContactProfileId,
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserContacts_Profiles_PersonalProfileId",
                        column: x => x.PersonalProfileId,
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserOfChats",
                columns: table => new
                {
                    UserOfChatId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOfChats", x => x.UserOfChatId);
                    table.ForeignKey(
                        name: "FK_UserOfChats_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "ChatId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOfChats_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageOwnerId = table.Column<int>(type: "int", nullable: false),
                    TimeSent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TimeStatusChanged = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContentId = table.Column<int>(type: "int", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_UserOfChats_MessageOwnerId",
                        column: x => x.MessageOwnerId,
                        principalTable: "UserOfChats",
                        principalColumn: "UserOfChatId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageReceivers",
                columns: table => new
                {
                    MessageReceiverId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageID = table.Column<int>(type: "int", nullable: false),
                    UserReceiverId = table.Column<int>(type: "int", nullable: false),
                    TimeRead = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageReceivers", x => x.MessageReceiverId);
                    table.ForeignKey(
                        name: "FK_MessageReceivers_Messages_MessageID",
                        column: x => x.MessageID,
                        principalTable: "Messages",
                        principalColumn: "MessageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageReceivers_UserOfChats_UserReceiverId",
                        column: x => x.UserReceiverId,
                        principalTable: "UserOfChats",
                        principalColumn: "UserOfChatId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageReceivers_MessageID",
                table: "MessageReceivers",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReceivers_UserReceiverId",
                table: "MessageReceivers",
                column: "UserReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageOwnerId",
                table: "Messages",
                column: "MessageOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Tag",
                table: "Profiles",
                column: "Tag",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserContacts_ContactProfileId",
                table: "UserContacts",
                column: "ContactProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContacts_PersonalProfileId",
                table: "UserContacts",
                column: "PersonalProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOfChats_ChatId",
                table: "UserOfChats",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOfChats_ProfileId",
                table: "UserOfChats",
                column: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageReceivers");

            migrationBuilder.DropTable(
                name: "UserContacts");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "UserOfChats");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}
