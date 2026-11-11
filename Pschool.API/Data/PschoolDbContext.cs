   using Microsoft.EntityFrameworkCore;
   using Pschool.API.Models;

   namespace Pschool.API.Data;

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
           modelBuilder.Entity<Student>()
               .HasOne(student => student.Parent)
               .WithMany(parent => parent.Students)
               .HasForeignKey(student => student.ParentId)
               .OnDelete(DeleteBehavior.Restrict);
       }
   }