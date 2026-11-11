   using Microsoft.EntityFrameworkCore;
   using Pschool.API.Models;

   namespace Pschool.API.Data;

   public class PschoolDbContext : DbContext
   {
    // Represents the database. Each DbSet below is one table.
       public PschoolDbContext(DbContextOptions<PschoolDbContext> options)
           : base(options)
       {
       }

       public DbSet<Parent> Parents { get; set; }
       public DbSet<Student> Students { get; set; }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
        // Restrict: a parent who still has students cannot be deleted,
           // so we never end up with students that have no parent.
           modelBuilder.Entity<Student>()
               .HasOne(student => student.Parent)
               .WithMany(parent => parent.Students)
               .HasForeignKey(student => student.ParentId)
               .OnDelete(DeleteBehavior.Restrict);
       }
   }