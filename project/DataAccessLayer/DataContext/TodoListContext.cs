using DataAccessLayer.Model;
using log4net;
using Microsoft.EntityFrameworkCore;
using Task = DataAccessLayer.Model.Task;

namespace DataAccessLayer.DataContext
{
    public class TodoListContext : DbContext
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TodoListContext()
        {
        }

        public TodoListContext(DbContextOptions<TodoListContext> options)
           : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Task> Tasks { get; set; } = null!;

        public virtual DbSet<Category> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
               .HasKey(e => e.Id)
               .HasName("Users_pkey");

            modelBuilder.Entity<User>()
                .Property(e => e.Id)
                    .UseIdentityAlwaysColumn()
                    .HasColumnName("id");

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");

            modelBuilder.Entity<User>()
                .Property(e => e.Login)
                .HasMaxLength(20)
                .HasColumnName("login");

            modelBuilder.Entity<User>()
                .Property(e => e.FirstName)
                .HasMaxLength(40)
                .HasColumnName("firstname");

            modelBuilder.Entity<User>()
                .Property(e => e.LastName)
                .HasMaxLength(40)
                .HasColumnName("lastname");

            modelBuilder.Entity<Category>()
                .HasKey(e => e.Id)
                .HasName("Categories_pk");

            modelBuilder.Entity<Category>()
                .Property(e => e.Id)
                    .UseIdentityAlwaysColumn()
                    .HasColumnName("id");

            modelBuilder.Entity<Category>()
                .Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("name");

            modelBuilder.Entity<Category>()
                .HasMany(c => c.Tasks)
                .WithOne(t => t.Category)
                .HasForeignKey(t => t.CategoryId)
                .HasConstraintName("CategoryFK");

            modelBuilder.Entity<Task>()
                .HasKey(e => e.Id)
                .HasName("Tasks_pkey");

            modelBuilder.Entity<Task>()
                .Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");

            modelBuilder.Entity<Task>()
                .Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");

            modelBuilder.Entity<Task>()
                .Property(e => e.IsDone)
                .HasColumnName("isdone");

            modelBuilder.Entity<Task>()
                .Property(e => e.Priority)
                .HasColumnName("priority");

            modelBuilder.Entity<Task>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.CategoryId)
                .HasConstraintName("CategoryFK");

            log.Debug(string.Format("Created a user", this, DateTime.Now));

            base.OnModelCreating(modelBuilder);
        }
    }
}
