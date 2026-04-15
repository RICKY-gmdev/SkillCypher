using Microsoft.EntityFrameworkCore;
using SkillCypher.Core.Models;


namespace SkillCypher.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {}

        public DbSet<User> Users{get;set;}
        public DbSet<Applicant> Applicants{get;set;}
        public DbSet<ApplicantCertificate> ApplicantCertificates{get;set;}
        public DbSet<ApplicantSkill> ApplicantSkills{get;set;}
        public DbSet<Application> Applications{get;set;}
        public DbSet<Certificate> Certificates  {get;set;}
        public DbSet<Company> Companies  {get;set;}
        public DbSet<Job> Jobs{get;set;}
        public DbSet<JobCertificate> JobCertificates {get;set;}
        public DbSet<JobMatch> JobMatches {get;set;}
        public DbSet<JobSkill> JobSkills {get;set;}
        public DbSet<Recruiter> Recruiters {get;set;}
        public DbSet<Skill> Skills {get;set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //ApplicationSkill
            modelBuilder.Entity<ApplicantSkill>()
            .HasKey(a=> new {a.ApplicantId, a.SkillId});

            //JobSkill
            modelBuilder.Entity<JobSkill>()
            .HasKey(a=> new{a.JobId, a.SkillId});

            //ApplicantCertificate
            modelBuilder.Entity<ApplicantCertificate>()
            .HasKey(a=> new{a.ApplicantId,a.CertificateId});

            //JobCertificate
            modelBuilder.Entity<JobCertificate>()
            .HasKey(a=> new{a.JobId,a.CertificateId});

            //jobMatch
            modelBuilder.Entity<JobMatch>()
            .HasKey(a=> new{a.ApplicantId,a.JobId});

            //Role
            modelBuilder.Entity<User>()
            .Property(a=> a.Role)
            .HasConversion<string>();

            //Status
            modelBuilder.Entity<Application>()
            .Property(a=> a.Status)
            .HasConversion<string>();


        }


    }
}