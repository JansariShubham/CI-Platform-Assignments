using System;
using System.Collections.Generic;
using CIPlatform.entities.DataModels;
using Microsoft.EntityFrameworkCore;

namespace CIPlatform.repository.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<CmsPage> CmsPages { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<FavouriteMission> FavouriteMissions { get; set; }

    public virtual DbSet<GoalMission> GoalMissions { get; set; }

    public virtual DbSet<Mission> Missions { get; set; }

    public virtual DbSet<MissionApplication> MissionApplications { get; set; }

    public virtual DbSet<MissionDocument> MissionDocuments { get; set; }

    public virtual DbSet<MissionInvite> MissionInvites { get; set; }

    public virtual DbSet<MissionTheme> MissionThemes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-M52MJI3;Initial Catalog=CI_Platform_db;Encrypt=False;Trusted_Connection=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__admin__43AA41416CD891D4");

            entity.ToTable("admin");

            entity.HasIndex(e => e.Email, "UQ__admin__AB6E6164940663B4").IsUnique();

            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.BannerId).HasName("PK__banner__10373C342E5BF682");

            entity.ToTable("banner");

            entity.Property(e => e.BannerId)
                .ValueGeneratedNever()
                .HasColumnName("banner_id");
            entity.Property(e => e.BannerImage).HasColumnName("banner_image");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.SortOrder).HasColumnName("sort_order");
            entity.Property(e => e.TextDesc)
                .HasColumnType("text")
                .HasColumnName("text_desc");
            entity.Property(e => e.TextTitle)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("text_title");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__city__031491A821C290C2");

            entity.ToTable("city");

            entity.Property(e => e.CityId)
                .ValueGeneratedNever()
                .HasColumnName("city_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__city__country_id__4316F928");
        });

        modelBuilder.Entity<CmsPage>(entity =>
        {
            entity.HasKey(e => e.CmsPageId).HasName("PK__cms_page__B46D5B52A627859E");

            entity.ToTable("cms_page");

            entity.Property(e => e.CmsPageId)
                .ValueGeneratedNever()
                .HasColumnName("cms_page_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("slug");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__comment__E79576874051D806");

            entity.ToTable("comment");

            entity.Property(e => e.CommentId)
                .ValueGeneratedNever()
                .HasColumnName("comment_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.Comments)
                .HasForeignKey(d => d.MissionId)
                .HasConstraintName("FK__comment__mission__5441852A");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__comment__user_id__534D60F1");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__country__7E8CD0556E07F9C8");

            entity.ToTable("country");

            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Iso)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("iso");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<FavouriteMission>(entity =>
        {
            entity.HasKey(e => e.FavouriteMissionId).HasName("PK__favourit__94E4D8CAA650F1D8");

            entity.ToTable("favourite_mission");

            entity.Property(e => e.FavouriteMissionId)
                .ValueGeneratedNever()
                .HasColumnName("favourite_mission_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.FavouriteMissions)
                .HasForeignKey(d => d.MissionId)
                .HasConstraintName("FK__favourite__missi__5BE2A6F2");

            entity.HasOne(d => d.User).WithMany(p => p.FavouriteMissions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__favourite__user___5AEE82B9");
        });

        modelBuilder.Entity<GoalMission>(entity =>
        {
            entity.HasKey(e => e.GoalMissionId).HasName("PK__goal_mis__358E02C72CD68FCF");

            entity.ToTable("goal_mission");

            entity.Property(e => e.GoalMissionId)
                .ValueGeneratedNever()
                .HasColumnName("goal_mission_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.GoalObjectiveText)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("goal_objective_text");
            entity.Property(e => e.GoalValue).HasColumnName("goal_value");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.GoalMissions)
                .HasForeignKey(d => d.MissionId)
                .HasConstraintName("FK__goal_miss__missi__5EBF139D");
        });

        modelBuilder.Entity<Mission>(entity =>
        {
            entity.HasKey(e => e.MissionId).HasName("PK__mission__B5419AB27DCD62FE");

            entity.ToTable("mission");

            entity.Property(e => e.MissionId)
                .ValueGeneratedNever()
                .HasColumnName("mission_id");
            entity.Property(e => e.Availability).HasColumnName("availability");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.MissionType).HasColumnName("mission_type");
            entity.Property(e => e.OrgDetails)
                .HasColumnType("text")
                .HasColumnName("org_details");
            entity.Property(e => e.OrgName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("org_name");
            entity.Property(e => e.ShortDesc)
                .HasColumnType("text")
                .HasColumnName("short_desc");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.ThemeId).HasColumnName("theme_id");
            entity.Property(e => e.Title)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.City).WithMany(p => p.Missions)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__mission__city_id__4F7CD00D");

            entity.HasOne(d => d.Country).WithMany(p => p.Missions)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__mission__country__5070F446");

            entity.HasOne(d => d.Theme).WithMany(p => p.Missions)
                .HasForeignKey(d => d.ThemeId)
                .HasConstraintName("FK__mission__theme_i__4E88ABD4");
        });

        modelBuilder.Entity<MissionApplication>(entity =>
        {
            entity.HasKey(e => e.MissionApplicationId).HasName("PK__mission___DF92838A75D0CFAF");

            entity.ToTable("mission_application");

            entity.Property(e => e.MissionApplicationId)
                .ValueGeneratedNever()
                .HasColumnName("mission_application_id");
            entity.Property(e => e.AppliedAt).HasColumnName("applied_at");
            entity.Property(e => e.ApprovalStatus).HasColumnName("approval_status");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionApplications)
                .HasForeignKey(d => d.MissionId)
                .HasConstraintName("FK__mission_a__missi__628FA481");

            entity.HasOne(d => d.User).WithMany(p => p.MissionApplications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__mission_a__user___619B8048");
        });

        modelBuilder.Entity<MissionDocument>(entity =>
        {
            entity.HasKey(e => e.MissionDocumentId).HasName("PK__mission___E80E0CC8FDA0B784");

            entity.ToTable("mission_document");

            entity.Property(e => e.MissionDocumentId)
                .ValueGeneratedNever()
                .HasColumnName("mission_document_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.DocumentName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("document_name");
            entity.Property(e => e.DocumentPath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("document_path");
            entity.Property(e => e.DocumentType)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("document_type");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionDocuments)
                .HasForeignKey(d => d.MissionId)
                .HasConstraintName("FK__mission_d__missi__656C112C");
        });

        modelBuilder.Entity<MissionInvite>(entity =>
        {
            entity.HasKey(e => e.MissionInviteId).HasName("PK__mission___A97ED158A719DF7B");

            entity.ToTable("mission_invite");

            entity.Property(e => e.MissionInviteId)
                .ValueGeneratedNever()
                .HasColumnName("mission_invite_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.FromUserId).HasColumnName("from_user_id");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.ToUserId).HasColumnName("to_user_id");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

            entity.HasOne(d => d.FromUser).WithMany(p => p.MissionInviteFromUsers)
                .HasForeignKey(d => d.FromUserId)
                .HasConstraintName("FK__mission_i__from___693CA210");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionInvites)
                .HasForeignKey(d => d.MissionId)
                .HasConstraintName("FK__mission_i__missi__68487DD7");

            entity.HasOne(d => d.ToUser).WithMany(p => p.MissionInviteToUsers)
                .HasForeignKey(d => d.ToUserId)
                .HasConstraintName("FK__mission_i__to_us__6A30C649");
        });

        modelBuilder.Entity<MissionTheme>(entity =>
        {
            entity.HasKey(e => e.MissionThemeId).HasName("PK__mission___4925C5AC41AA5B8D");

            entity.ToTable("mission_theme");

            entity.Property(e => e.MissionThemeId)
                .ValueGeneratedNever()
                .HasColumnName("mission_theme_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("(N'1')")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370FA69BCE60");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E61642C48AFF5").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.Avatar).HasColumnName("avatar");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");
            entity.Property(e => e.Department)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("department");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("employee_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.LinkedInUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("linked_in_url");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
            entity.Property(e => e.ProfileText)
                .HasColumnType("text")
                .HasColumnName("profile_text");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("(N'1')")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            entity.Property(e => e.WhyIVolunteer)
                .HasColumnType("text")
                .HasColumnName("why_i_volunteer");

            entity.HasOne(d => d.City).WithMany(p => p.Users)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__users__city_id__48CFD27E");

            entity.HasOne(d => d.Country).WithMany(p => p.Users)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__users__country_i__49C3F6B7");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
