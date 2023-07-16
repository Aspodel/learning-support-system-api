using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearningSupportSystemAPI;

public class ApplicationDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Announcement> Announcements { get; set; } = null!;
    public DbSet<Class> Classes { get; set; } = null!;
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<CoursePrerequisite> CoursePrerequisites { get; set; } = null!;
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<Discussion> Discussions { get; set; } = null!;
    public DbSet<Grade> Grades { get; set; } = null!;
    public DbSet<GradeColumn> GradeColumns { get; set; } = null!;
    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<Major> Majors { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<Semester> Semesters { get; set; } = null!;
    public DbSet<ToDoList> ToDoList { get; set; } = null!;
    public DbSet<ToDoItem> ToDoItems { get; set; } = null!;
    public DbSet<Answer> Answers { get; set; } = null!;
    public DbSet<Assignment> Assignments { get; set; } = null!;
    public DbSet<Question> Questions { get; set; } = null!;
    public DbSet<Submission> Submissions { get; set; } = null!;
    public DbSet<AnswerSubmission> AnswerSubmissions { get; set; } = null!;
    public DbSet<FileSubmission> FileSubmissions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Administrator>().ToTable(nameof(Administrator) + "s");
        builder.Entity<Student>().ToTable(nameof(Student) + "s");
        builder.Entity<Lecturer>().ToTable(nameof(Lecturer) + "s");


        builder.Entity<User>(entity =>
        {
            entity.Property(e => e.IdCard).IsRequired();

            entity.HasOne(u => u.Department).WithMany(d => d.Users).OnDelete(DeleteBehavior.Restrict);
        });


        builder.Entity<UserRole>(entity =>
        {
            entity.HasOne(ur => ur.Role).WithMany(r => r!.UserRoles).HasForeignKey(ur => ur.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(ur => ur.User).WithMany(u => u!.UserRoles).HasForeignKey(ur => ur.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
        });


        builder.Entity<Announcement>(entity =>
        {
            entity.HasOne(a => a.Sender).WithMany(u => u.Announcements).OnDelete(DeleteBehavior.Restrict);
        });


        builder.Entity<Class>(entity =>
        {
            entity.HasIndex(e => e.ClassCode).IsUnique();

            entity.HasOne(c => c.Course).WithMany(co => co.Classes).OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(c => c.Semester).WithMany(s => s.Classes).OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Lecturer).WithMany(l => l.Classes);

            entity.HasOne(c => c.Room).WithMany(r => r.Classes);
        });


        builder.Entity<Course>(entity =>
        {
            entity.HasIndex(e => e.CourseCode).IsUnique();

            entity.HasOne(c => c.Department).WithMany(d => d.Courses).OnDelete(DeleteBehavior.Cascade);
        });


        builder.Entity<CoursePrerequisite>(entity =>
        {
            entity.HasKey(cp => new { cp.CourseId, cp.PrerequisiteId }); // Configure composite primary key

            entity.HasOne(cp => cp.Course)
                .WithMany(c => c.PrerequisiteFor)
                .HasForeignKey(cp => cp.CourseId) // Configure one-to-many relationship
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(cp => cp.Prerequisite)
                .WithMany(c => c.Prerequisites)
                .HasForeignKey(cp => cp.PrerequisiteId) // Configure one-to-many relationship
                .OnDelete(DeleteBehavior.Restrict);
        });


        builder.Entity<Department>(entity =>
        {
            entity.HasIndex(e => e.ShortName).IsUnique();

            entity.HasOne(d => d.FacultyOffice).WithOne(r => r.Department).OnDelete(DeleteBehavior.Restrict);
        });


        builder.Entity<Grade>(entity =>
        {
            entity.HasOne(g => g.GradeColumn).WithMany(gc => gc.Grades).OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(g => g.Student).WithMany(s => s.Grades).OnDelete(DeleteBehavior.Restrict);
        });


        builder.Entity<GradeColumn>(entity =>
        {
            entity.HasOne(gc => gc.Class).WithMany(c => c.GradeColumns).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Group>(entity =>
        {
            entity.HasOne(gc => gc.Class).WithMany(c => c.Groups).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Major>(entity =>
        {
            entity.HasIndex(e => e.ShortName).IsUnique();

            entity.HasOne(m => m.Department).WithMany(d => d.Majors).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Notification>(entity =>
        {
            entity.HasOne(n => n.Recipient).WithMany(u => u.Notifications).OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Room>(entity =>
        {
            entity.HasIndex(e => e.Code).IsUnique();
        });


        builder.Entity<Student>(entity =>
        {
            entity.HasOne(s => s.Major).WithMany(m => m.Students).OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(s => s.Supervisor).WithMany(l => l.Students).OnDelete(DeleteBehavior.Restrict);
        });


        builder.Entity<StudentClass>(entity =>
        {
            entity.HasKey(sc => new { sc.StudentId, sc.ClassId });

            entity.HasOne(sc => sc.Student)
                .WithMany(s => s.RegisteredClasses)
                .HasForeignKey(sc => sc.StudentId);

            entity.HasOne(sc => sc.Class)
                .WithMany(c => c.Students)
                .HasForeignKey(sc => sc.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(sc => sc.Group)
                .WithMany(g => g.Students)
                .HasForeignKey(sc => sc.GroupId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        builder.Entity<StudentTask>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(sc => sc.Student)
                .WithMany(s => s.Tasks)
                .HasForeignKey(sc => sc.StudentId);

            entity.HasOne(sc => sc.ToDoItem)
                .WithMany(s => s.Students)
                .HasForeignKey(sc => sc.ToDoItemId);
        });

        builder.Entity<ToDoList>(entity =>
        {
            entity.HasOne(t => t.Group)
                .WithMany(g => g.ToDoLists)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<ToDoItem>(entity =>
        {
            entity.HasOne(t => t.ToDoList)
                .WithMany(td => td.Items)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<Assignment>(entity =>
        {
            entity.HasMany(e => e.Submissions).WithOne(s => s.Assignment).HasForeignKey(s => s.AssignmentId);
        });

        builder.Entity<Question>(entity =>
        {
            entity.HasMany(e => e.Answers).WithOne(a => a.Question).HasForeignKey(e => e.QuestionId);

            entity.HasOne(e => e.CorrectAnswer).WithOne().HasForeignKey<Question>(e => e.CorrectAnswerId);
        });


        builder.Entity<Submission>(entity =>
        {
            entity.HasOne(e => e.Student).WithMany().HasForeignKey(e => e.StudentId);

            entity.HasOne(e => e.Group).WithMany().HasForeignKey(e => e.GroupId);
        });

        builder.Entity<AnswerSubmission>(entity =>
        {
            entity.HasOne(e => e.Answer).WithMany().HasForeignKey(e => e.AnswerId);
        });

        builder.Entity<Discussion>(entity =>
        {
            entity.HasOne(e => e.Group).WithOne().HasForeignKey<Discussion>(e => e.GroupId);

            entity.HasOne(e => e.Class).WithMany().HasForeignKey(e => e.ClassId);

            entity.HasOne(e => e.Creator).WithMany().HasForeignKey(e => e.CreatorId);
        });

        builder.Entity<Message>(entity =>
        {
            entity.HasOne(m => m.Discussion).WithMany(d => d.Messages).HasForeignKey(e => e.DiscussionId).OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(m => m.Sender).WithMany().HasForeignKey(e => e.SenderId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
