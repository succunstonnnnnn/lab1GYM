using lab1GYM.Model;
using Microsoft.EntityFrameworkCore;

namespace gymmm;

public partial class GYMDbContext : DbContext
{
    public GYMDbContext()
    {
    }

    public GYMDbContext(DbContextOptions<GYMDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Exercise> Exercises { get; set; }

    public virtual DbSet<Meal> Meals { get; set; }

    public virtual DbSet<NutritionPlan> NutritionPlans { get; set; }

    public virtual DbSet<NutritionPlanMeal> NutritionPlanMeals { get; set; }

    public virtual DbSet<ProgressTracking> ProgressTrackings { get; set; }

    public virtual DbSet<TrainingPlan> TrainingPlans { get; set; }

    public virtual DbSet<TrainingPlanExercise> TrainingPlanExercises { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=GYM;User Id=sa;Password=Katya_len#19305;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.ExercisesId).HasName("PK__Exercise__577912494905E586");

            entity.Property(e => e.ExercisesId).HasColumnName("ExercisesID");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TypeId).HasColumnName("TypeID");
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Meals__CB278740F46F3AEA");

            entity.Property(e => e.Id).HasColumnName("MealsID");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NutritionPlan>(entity =>
        {
            entity.HasKey(e => e.NutritionPlanId).HasName("PK__Nutritio__013DA35D04734093");

            entity.ToTable("NutritionPlan");

            entity.Property(e => e.NutritionPlanId).HasColumnName("NutritionPlanID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("My Plan");
            entity.Property(e => e.Id).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.NutritionPlans)
                .HasForeignKey(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Nutrition__UserI__3F466844");
        });

        modelBuilder.Entity<NutritionPlanMeal>(entity =>
        {
            entity.HasKey(e => new { e.NutritionPlanId, e.Id }).HasName("PK__Nutritio__1D8FDB295749E137");

            entity.ToTable("NutritionPlan_Meals");

            entity.Property(e => e.NutritionPlanId).HasColumnName("NutritionPlanID");
            entity.Property(e => e.Id).HasColumnName("MealsID");
            entity.Property(e => e.Quantity).HasDefaultValue(100);

            entity.HasOne(d => d.Meals).WithMany(p => p.NutritionPlanMeals)
                .HasForeignKey(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Nutrition__Meals__5535A963");

            entity.HasOne(d => d.NutritionPlan).WithMany(p => p.NutritionPlanMeals)
                .HasForeignKey(d => d.NutritionPlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Nutrition__Nutri__5441852A");
        });

        modelBuilder.Entity<ProgressTracking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Progress__4BFF4BABB9FAD7CE");

            entity.ToTable("ProgressTracking");

            entity.Property(e => e.Id).HasColumnName("ProgressTrackingID");
            entity.Property(e => e.Circumferences)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Id).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.ProgressTrackings)
                .HasForeignKey(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProgressT__UserI__5165187F");
        });

        modelBuilder.Entity<TrainingPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Training__80FCC5934FBBBCE9");

            entity.ToTable("TrainingPlan");

            entity.Property(e => e.Id).HasColumnName("TrainingPlanID");
            entity.Property(e => e.Id).HasColumnName("TypeID");
            entity.Property(e => e.Id).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.TrainingPlans)
                .HasForeignKey(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TrainingP__UserI__3A81B327");
        });

        modelBuilder.Entity<TrainingPlanExercise>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.ExercisesId }).HasName("PK__Training__058B54B7183D502E");

            entity.ToTable("TrainingPlan_Exercises");

            entity.Property(e => e.Id).HasColumnName("TrainingPlanID");
            entity.Property(e => e.ExercisesId).HasColumnName("ExercisesID");

            entity.HasOne(d => d.Exercises).WithMany(p => p.TrainingPlanExercises)
                .HasForeignKey(d => d.ExercisesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TrainingP__Exerc__59063A47");

            entity.HasOne(d => d.TrainingPlan).WithMany(p => p.TrainingPlanExercises)
                .HasForeignKey(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TrainingP__Train__5812160E");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__1788CCAC2E881AA2");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534043F30FC").IsUnique();

            entity.Property(e => e.Id).HasColumnName("UserID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
