using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using GymDomain.Model;

namespace GymInfrastructure;

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
    
    public DbSet<BodyParameter> BodyParameters { get; set; }
    public DbSet<ProgressHistory> ProgressHistories { get; set; }
    public DbSet<PhotoEntry> PhotoEntries { get; set; }
    public DbSet<TrainingWeight> TrainingWeights { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=GYMDB;User Id=sa;Password=Katya_len#19305;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exercise__57791249084948B9");

            entity.Property(e => e.Id).HasColumnName("ExercisesID");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Meals__CB2787401B946237");

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
            entity.HasKey(e => e.Id).HasName("PK__Nutritio__013DA35DE5AFDE4E");

            entity.ToTable("NutritionPlan");

            entity.Property(e => e.Id).HasColumnName("NutritionPlanID");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.NutritionPlans)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Nutrition__UserI__3A81B327");
        });

        modelBuilder.Entity<NutritionPlanMeal>(entity =>
        {
            entity.HasKey(e => new { e.NutritionPlanId, e.MealsId }).HasName("PK__Nutritio__1D8FDB29C19836F1");

            entity.ToTable("NutritionPlan_Meals");

            entity.Property(e => e.NutritionPlanId).HasColumnName("NutritionPlanID");
            entity.Property(e => e.MealsId).HasColumnName("MealsID");

            entity.HasOne(d => d.Meals).WithMany(p => p.NutritionPlanMeals)
                .HasForeignKey(d => d.MealsId)
                .HasConstraintName("FK__Nutrition__Meals__403A8C7D");

            entity.HasOne(d => d.NutritionPlan).WithMany(p => p.NutritionPlanMeals)
                .HasForeignKey(d => d.NutritionPlanId)
                .HasConstraintName("FK__Nutrition__Nutri__3F466844");
        });

        modelBuilder.Entity<ProgressTracking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Progress__4BFF4BABC44B65EC");

            entity.ToTable("ProgressTracking");

            entity.Property(e => e.Id).HasColumnName("ProgressTrackingID");
            entity.Property(e => e.Circumferences)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.ProgressTrackings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ProgressT__UserI__4BAC3F29");
        });

        modelBuilder.Entity<TrainingPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Training__80FCC59314B3A981");

            entity.ToTable("TrainingPlan");

            entity.Property(e => e.Id).HasColumnName("TrainingPlanID");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.TrainingPlans)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__TrainingP__UserI__4316F928");
        });

        modelBuilder.Entity<TrainingPlanExercise>(entity =>
        {
            entity.HasKey(e => new { e.TrainingPlanId, e.ExercisesId }).HasName("PK__Training__058B54B778F7C4DD");

            entity.ToTable("TrainingPlan_Exercises");

            entity.Property(e => e.TrainingPlanId).HasColumnName("TrainingPlanID");
            entity.Property(e => e.ExercisesId).HasColumnName("ExercisesID");

            entity.HasOne(d => d.Exercises).WithMany(p => p.TrainingPlanExercises)
                .HasForeignKey(d => d.ExercisesId)
                .HasConstraintName("FK__TrainingP__Exerc__48CFD27E");

            entity.HasOne(d => d.TrainingPlan).WithMany(p => p.TrainingPlanExercises)
                .HasForeignKey(d => d.TrainingPlanId)
                .HasConstraintName("FK__TrainingP__Train__47DBAE45");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__1788CCACF95D5112");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105340A9351BE").IsUnique();

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
        
        modelBuilder.Ignore<Entity>();
    }
   

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
