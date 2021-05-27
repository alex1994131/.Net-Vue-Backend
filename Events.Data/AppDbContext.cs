using Events.Api.Models.APTs;
using Events.Api.Models.General;
using Events.Api.Models.Incidents;
using Events.Api.Models.Tasks;
using Events.Api.Models.UserManagement;
using Events.Core.Models.APTs;
using Events.Core.Models.Employees;
using Events.Core.Models.General;
using Events.Core.Models.Incidents;
using Events.Core.Models.Tasks;
using Events.Core.Models.UserManagement;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Events.Core.Models.Logging;
using Events.Core.Models;
using Events.Core.Models.Notifications;

namespace Events.Data
{
    public class AppDbContext :IdentityDbContext<EUser, ERole,long>
    {
        public readonly object Taskview;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<EUser> Employees { get; set; }

        public DbSet<Department> Departments { set; get; }
        public DbSet<OrganizationContact> OrganizationContacts { get; set; }

        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<APT> Apts { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<OwnerDetails> OwnerDetails { get; set; }

        public DbSet<Incident> Incidents { get; set; }

        public DbSet<EntityAssignment> EntityAssignments { get; set; }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<IncidentComment> IncidentsComments { get; set; }

        public DbSet<ChangeLog> ChangeLogs { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Section> Sections { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Saverity> Saverities { get; set; }
        public DbSet<Urgancey> Urganceys { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<TaskType> TaskType { get; set; }
        public DbSet<EUser> Users { get; set; }

        public DbSet<NotificationView> VNotifications { get; set; }
        
        public DbSet<TaskEmpsRel> TaskEmpsRel { get; set; }

        public DbSet<UserActivity> UserActivity { get; set; }

        public DbSet<IpAddress> IpAddress { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<CloseReport> CloseReports { get; set; }

        public DbSet<Taskview> Taskviews { get; set; }

        public DbSet<IncidentView> IncidentViews { get; set; }

        [System.Obsolete]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Query<Taskview>().ToView("taskview").HasNoKey();
            modelBuilder.Query<IncidentView>().ToView("IncidentView").HasNoKey() ;
            modelBuilder.Query<NotificationView>().ToView("NotificationView").HasNoKey();

            

            modelBuilder.Entity<OrgsIncidentRel>().HasKey(o =>
              new { o.IncidentId, o.OrganizationId });

            modelBuilder.Entity<TaskComment>().HasKey(o =>
  new { o.TaskId, o.CommentId });

            modelBuilder.Entity<CommentAttachment>().HasKey(o =>
new { o.commentId, o.attachmentId });


            modelBuilder.Entity<IncidentComment>().HasKey(o =>
  new { o.IncidentId, o.CommentId });


            modelBuilder.Entity<OriginCountry>()
                .HasKey(bc => new { bc.CountryId, bc.APTId });


            modelBuilder.Entity<TargetedCountry>()
                .HasKey(bc => new { bc.CountryId, bc.APTId });

            modelBuilder.Entity<TargetedSector>()
                .HasKey(bc => new { bc.SectorId, bc.AptId });

            modelBuilder.Entity<TaskEmpsRel>()
                .HasKey(bc => new { bc.EUserId, bc.TaskId });

            modelBuilder.Entity<AptAttachment>()
                .HasKey(bc => new { bc.AttachmentId, bc.APTId });

            modelBuilder.Entity<TaskAttachments>()
                .HasKey(bc => new { bc.AttachmentId, bc.TaskId });

            modelBuilder.Entity<IncidentAttachment>()
            .HasKey(bc => new { bc.AttachmentId, bc.IncidentId });

            modelBuilder.Entity<OrgsIncidentRel>()
             .HasKey(bc => new { bc.OrganizationId, bc.IncidentId });

            modelBuilder.Entity<ReportAttachment>()
                .HasKey(bc => new { bc.attachmentId, bc.closeReportId });

            base.OnModelCreating(modelBuilder);


        }




    }
    
}
