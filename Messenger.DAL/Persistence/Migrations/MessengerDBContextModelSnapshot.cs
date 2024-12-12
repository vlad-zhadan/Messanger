﻿// <auto-generated />
using System;
using Messenger.DAL.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Messenger.DAL.Migrations
{
    [DbContext(typeof(MessengerDBContext))]
    partial class MessengerDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Messenger.DAL.Entities.Chat", b =>
                {
                    b.Property<int>("ChatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ChatId"));

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("bit");

                    b.Property<int>("MaxParticipants")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("ChatId");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("Messenger.DAL.Entities.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MessageId"));

                    b.Property<int?>("ContentId")
                        .HasColumnType("int");

                    b.Property<int>("MessageOwnerId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<DateTime>("TimeSent")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimeStatusChanged")
                        .HasColumnType("datetime2");

                    b.HasKey("MessageId");

                    b.HasIndex("MessageOwnerId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Messenger.DAL.Entities.MessageReceiver", b =>
                {
                    b.Property<int>("MessageReceiverId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MessageReceiverId"));

                    b.Property<int>("MessageID")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeRead")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserReceiverId")
                        .HasColumnType("int");

                    b.HasKey("MessageReceiverId");

                    b.HasIndex("MessageID");

                    b.HasIndex("UserReceiverId");

                    b.ToTable("MessageReceivers");
                });

            modelBuilder.Entity("Messenger.DAL.Entities.Profile", b =>
                {
                    b.Property<int>("ProfileId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProfileId"));

                    b.Property<string>("Bio")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ConnectionId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("LastSeen")
                        .HasColumnType("datetime2");

                    b.Property<string>("PhotoBlobId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ProfileId");

                    b.HasIndex("Tag")
                        .IsUnique();

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("Messenger.DAL.Entities.UserContact", b =>
                {
                    b.Property<int>("ContactId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ContactId"));

                    b.Property<int>("ContactProfileId")
                        .HasColumnType("int");

                    b.Property<int>("PersonalProfileId")
                        .HasColumnType("int");

                    b.HasKey("ContactId");

                    b.HasIndex("ContactProfileId");

                    b.HasIndex("PersonalProfileId");

                    b.ToTable("UserContacts");
                });

            modelBuilder.Entity("Messenger.DAL.Entities.UserOfChat", b =>
                {
                    b.Property<int>("UserOfChatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserOfChatId"));

                    b.Property<int>("ChatId")
                        .HasColumnType("int");

                    b.Property<int>("ProfileId")
                        .HasColumnType("int");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("UserOfChatId");

                    b.HasIndex("ChatId");

                    b.HasIndex("ProfileId");

                    b.ToTable("UserOfChats");
                });

            modelBuilder.Entity("Messenger.DAL.Entities.Message", b =>
                {
                    b.HasOne("Messenger.DAL.Entities.UserOfChat", "MessageOwner")
                        .WithMany("Messages")
                        .HasForeignKey("MessageOwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MessageOwner");
                });

            modelBuilder.Entity("Messenger.DAL.Entities.MessageReceiver", b =>
                {
                    b.HasOne("Messenger.DAL.Entities.Message", "Message")
                        .WithMany("Receivers")
                        .HasForeignKey("MessageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Messenger.DAL.Entities.UserOfChat", "UserReceiver")
                        .WithMany("MessageReceivers")
                        .HasForeignKey("UserReceiverId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Message");

                    b.Navigation("UserReceiver");
                });

            modelBuilder.Entity("Messenger.DAL.Entities.UserContact", b =>
                {
                    b.HasOne("Messenger.DAL.Entities.Profile", "ContactProfile")
                        .WithMany("ContactUserContacts")
                        .HasForeignKey("ContactProfileId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Messenger.DAL.Entities.Profile", "PersonalProfile")
                        .WithMany("PersonalUserContacts")
                        .HasForeignKey("PersonalProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContactProfile");

                    b.Navigation("PersonalProfile");
                });

            modelBuilder.Entity("Messenger.DAL.Entities.UserOfChat", b =>
                {
                    b.HasOne("Messenger.DAL.Entities.Chat", "Chat")
                        .WithMany("UsersOfChat")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Messenger.DAL.Entities.Profile", "Profile")
                        .WithMany("UserOfChats")
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Messenger.DAL.Entities.Chat", b =>
                {
                    b.Navigation("UsersOfChat");
                });

            modelBuilder.Entity("Messenger.DAL.Entities.Message", b =>
                {
                    b.Navigation("Receivers");
                });

            modelBuilder.Entity("Messenger.DAL.Entities.Profile", b =>
                {
                    b.Navigation("ContactUserContacts");

                    b.Navigation("PersonalUserContacts");

                    b.Navigation("UserOfChats");
                });

            modelBuilder.Entity("Messenger.DAL.Entities.UserOfChat", b =>
                {
                    b.Navigation("MessageReceivers");

                    b.Navigation("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
