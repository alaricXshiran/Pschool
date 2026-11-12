   using Microsoft.EntityFrameworkCore;
   using Pschool.API.Models;

   namespace Pschool.API.Data;

   // Represents the database. Each DbSet below is one table.
   public class PschoolDbContext : DbContext
   {
       public PschoolDbContext(DbContextOptions<PschoolDbContext> options)
           : base(options)
       {
       }

       public DbSet<Parent> Parents { get; set; }
       public DbSet<Student> Students { get; set; }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
           // Column sizes for Parents. These match the limits in ParentRequest.
           modelBuilder.Entity<Parent>(parent =>
           {
               parent.Property(p => p.FirstName).HasMaxLength(50);
               parent.Property(p => p.LastName).HasMaxLength(50);
               parent.Property(p => p.Username).HasMaxLength(50);
               parent.Property(p => p.Email).HasMaxLength(100);
               parent.Property(p => p.HomeAddress).HasMaxLength(200);
               parent.Property(p => p.Phone1).HasMaxLength(20);
               parent.Property(p => p.WorkPhone).HasMaxLength(20);
               parent.Property(p => p.HomePhone).HasMaxLength(20);
           });

           // Column sizes for Students. These match the limits in StudentRequest.
           modelBuilder.Entity<Student>(student =>
           {
               student.Property(s => s.FirstName).HasMaxLength(50);
               student.Property(s => s.LastName).HasMaxLength(50);
           });

           // Restrict: a parent who still has students cannot be deleted,
           // so we never end up with students that have no parent.
           modelBuilder.Entity<Student>()
               .HasOne(student => student.Parent)
               .WithMany(parent => parent.Students)
               .HasForeignKey(student => student.ParentId)
               .OnDelete(DeleteBehavior.Restrict);
       }
   }