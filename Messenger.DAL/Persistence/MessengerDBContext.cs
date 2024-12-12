using Messenger.DAL.Entities;
using Messenger.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace Messenger.DAL.Persistence;

public class MessengerDBContext : DbContext
{
    public MessengerDBContext(DbContextOptions<MessengerDBContext> options) : base(options)
    {
        
    }
    
    // public MessengerDBContext()
    // {
    // }
    //
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Messenger;Trusted_Connection=True;");
    //
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Profile>()
            .HasIndex(p => p.Tag)
            .IsUnique();

        modelBuilder.Entity<UserContact>(b =>
        {
            b.HasOne(u => u.PersonalProfile)
                .WithMany(p => p.PersonalUserContacts)
                .HasForeignKey(uc => uc.PersonalProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(uc => uc.ContactProfile)
                .WithMany(p => p.ContactUserContacts)
                .HasForeignKey(uc => uc.ContactProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<UserOfChat>(b =>
        {
            b.HasOne(uoc => uoc.Chat)
                .WithMany(c => c.UsersOfChat)
                .HasForeignKey(uc => uc.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(uoc => uoc.Profile)
                .WithMany(p => p.UserOfChats)
                .HasForeignKey(uc => uc.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Message>()
            .HasOne(m => m.MessageOwner)
            .WithMany(uoc => uoc.Messages)
            .HasForeignKey(m => m.MessageOwnerId);

        modelBuilder.Entity<MessageReceiver>(b =>
        {
            b.HasOne(mr => mr.Message)
                .WithMany(mr => mr.Receivers)
                .HasForeignKey(mr => mr.MessageID)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(mr => mr.UserReceiver)
                .WithMany(uoc => uoc.MessageReceivers)
                .HasForeignKey(mr => mr.UserReceiverId)
                .OnDelete(DeleteBehavior.NoAction);
        });
        
        modelBuilder.Entity<Connection>()
            .HasOne(c => c.Profile)
            .WithMany(p => p.Connections)
            .HasForeignKey(c => c.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<MessageReceiver> MessageReceivers { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<UserContact> UserContacts { get; set; }
    public DbSet<UserOfChat> UserOfChats { get; set; }
    public DbSet<Connection> Connections { get; set; }
    
}