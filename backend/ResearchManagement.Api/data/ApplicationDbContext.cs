using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ResearchManagement.Api.models;

namespace ResearchManagement.Api.data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ResearchTopic> ResearchTopics { get; set; }
        public DbSet<ProgressReport> ProgressReports { get; set; }
        public DbSet<FinalReport> FinalReports { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<SystemConfig> SystemConfigs { get; set; }
        public DbSet<ComplianceRecord> ComplianceRecords { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<TopicReviewAssignment> TopicReviewAssignments { get; set; }
        public DbSet<Issue> Issues { get; set; } // Thêm DbSet cho bảng Issues

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Đảm bảo uniqueness cho email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

        }

    }

    
}