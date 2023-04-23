using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Model;
using Task = DataAccessLayer.Model.Task;

namespace DataAccessLayer.DataContext
{
    public class TodoListContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public TodoListContext()
        {
        }
        public TodoListContext(DbContextOptions<TodoListContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Server=localhost;port=5432;user id=postgres;password=111;database=todo_list;");
            }
        }


        public DbSet<Task> Tasks { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");
                entity.HasKey(e => e.Id).HasName("Categories_pk");
                entity.Property(e => e.Id).HasColumnName("id").UseIdentityAlwaysColumn();
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(40);
                entity.HasMany(e => e.Tasks).WithOne(e => e.Category).HasForeignKey(e => e.CategoryId)
                    .HasConstraintName("Tasks_categoryfk_fkey");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Tasks");
                entity.HasKey(e => e.Id).HasName("Tasks_pkey");
                entity.Property(e => e.Id).HasColumnName("id").UseIdentityAlwaysColumn();
                entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(200);
                entity.Property(e => e.IsDone).HasColumnName("isdone");
                entity.Property(e => e.Priority).HasColumnName("priority");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Deadline).HasColumnName("deadline");
                entity.Property(e => e.CategoryId).HasColumnName("categoryfk");
                entity.Property(e => e.ParentId).HasColumnName("parenttaskfk");
                entity.Property(e => e.UserId).HasColumnName("userid");
                entity.HasOne(e => e.User).WithMany(e => e.Tasks).HasForeignKey(e => e.UserId)
                    .HasConstraintName("Tasks_userid_fkey").OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.Category).WithMany(e => e.Tasks).HasForeignKey(e => e.CategoryId)
                    .HasConstraintName("Tasks_categoryfk_fkey");
                entity.HasOne(e => e.ParentTask).WithMany(e => e.Subtasks).HasForeignKey(e => e.ParentId)
                    .HasConstraintName("Tasks_parenttaskfk_fkey").OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}