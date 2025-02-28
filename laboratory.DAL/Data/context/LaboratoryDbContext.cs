using laboratory.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laboratory.DAL.Data.context
{
    public class LaboratoryDbContext : DbContext
    {
        public LaboratoryDbContext(DbContextOptions<LaboratoryDbContext> options) : base(options)
        {
        }

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Experiment> Experiments { get; set; } //
        public DbSet<Material> Materials { get; set; } //
        public DbSet<ExperimentMaterial> ExperimentMaterials { get; set; } //
        public DbSet<Request> Requests { get; set; }
        public DbSet<StudentRequest> StudentRequests { get; set; }
        public DbSet<Group> Groups { get; set; } //

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // العلاقات بين الجداول
            modelBuilder.Entity<Doctor>()
                .HasIndex(d => d.Email)
                .IsUnique();

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Email)
                .IsUnique();

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Doctor)
                .WithMany()
                .HasForeignKey(r => r.DoctorId);

            modelBuilder.Entity<Student>()
                .HasOne(r => r.Group)
                .WithMany()
                .HasForeignKey(r => r.GroupId);


            modelBuilder.Entity<Request>()
                .HasOne(r => r.Experiment)
                .WithMany()
                .HasForeignKey(r => r.ExperimentId);

            modelBuilder.Entity<StudentRequest>()
                .HasOne(sr => sr.Student)
                .WithMany()
                .HasForeignKey(sr => sr.StudentId);
        }
    }
}

