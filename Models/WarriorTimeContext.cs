using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace warriorTime.Models
{
    public partial class WarriorTimeContext : DbContext
    {
        public WarriorTimeContext()
        {
        }

        public WarriorTimeContext(DbContextOptions<WarriorTimeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Coach> Coaches { get; set; } = null!;
        public virtual DbSet<Cour> Cours { get; set; } = null!;
        public virtual DbSet<Discipline> Disciplines { get; set; } = null!;
        public virtual DbSet<Etudiant> Etudiants { get; set; } = null!;
        public virtual DbSet<Inscrit> Inscrits { get; set; } = null!;
        public virtual DbSet<Salle> Salles { get; set; } = null!;
        public virtual DbSet<Typecour> Typecours { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;user=root;database=warriortime", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.36-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("latin1_swedish_ci")
                .HasCharSet("latin1");

            modelBuilder.Entity<Coach>(entity =>
            {
                entity.HasKey(e => e.IdCoach)
                    .HasName("PRIMARY");

                entity.ToTable("coach");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.IdCoach)
                    .HasColumnType("int(11)")
                    .HasColumnName("idCoach");

                entity.Property(e => e.Mail)
                    .HasMaxLength(150)
                    .HasColumnName("mail");

                entity.Property(e => e.Mdp)
                    .HasMaxLength(32)
                    .HasColumnName("mdp");

                entity.Property(e => e.Nom)
                    .HasMaxLength(50)
                    .HasColumnName("nom");

                entity.Property(e => e.Prenom)
                    .HasMaxLength(50)
                    .HasColumnName("prenom");

                entity.Property(e => e.Tel)
                    .HasMaxLength(10)
                    .HasColumnName("tel");
            });

            modelBuilder.Entity<Cour>(entity =>
            {
                entity.HasKey(e => e.IdCours)
                    .HasName("PRIMARY");

                entity.ToTable("cours");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.HasIndex(e => e.IdCoach, "fk_cours_coach");

                entity.HasIndex(e => e.IdDiscipline, "fk_cours_discipline");

                entity.HasIndex(e => e.IdsalleDeClasse, "fk_cours_salle");

                entity.HasIndex(e => e.IdTypeCours, "fk_typeCours_cours");

                entity.Property(e => e.IdCours)
                    .HasColumnType("int(11)")
                    .HasColumnName("idCours");

                entity.Property(e => e.DateCours).HasColumnName("dateCours");

                entity.Property(e => e.Duree)
                    .HasColumnType("int(11)")
                    .HasColumnName("duree");

                entity.Property(e => e.IdCoach)
                    .HasColumnType("int(11)")
                    .HasColumnName("idCoach");

                entity.Property(e => e.IdDiscipline)
                    .HasColumnType("int(11)")
                    .HasColumnName("idDiscipline");

                entity.Property(e => e.IdTypeCours)
                    .HasColumnType("int(11)")
                    .HasColumnName("idTypeCours");

                entity.Property(e => e.IdsalleDeClasse)
                    .HasColumnType("int(11)")
                    .HasColumnName("idsalleDeClasse");

                entity.Property(e => e.LimiteEtudiant)
                    .HasColumnType("int(11)")
                    .HasColumnName("limiteEtudiant");

                entity.Property(e => e.Pour)
                    .HasMaxLength(100)
                    .HasColumnName("pour");

                entity.Property(e => e.Statut)
                    .HasMaxLength(100)
                    .HasColumnName("statut");

                entity.HasOne(d => d.IdCoachNavigation)
                    .WithMany(p => p.Cours)
                    .HasForeignKey(d => d.IdCoach)
                    .HasConstraintName("fk_cours_coach");

                entity.HasOne(d => d.IdDisciplineNavigation)
                    .WithMany(p => p.Cours)
                    .HasForeignKey(d => d.IdDiscipline)
                    .HasConstraintName("fk_cours_discipline");

                entity.HasOne(d => d.IdTypeCoursNavigation)
                    .WithMany(p => p.Cours)
                    .HasForeignKey(d => d.IdTypeCours)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_typeCours_cours");

                entity.HasOne(d => d.IdsalleDeClasseNavigation)
                    .WithMany(p => p.Cours)
                    .HasForeignKey(d => d.IdsalleDeClasse)
                    .HasConstraintName("fk_cours_salle");
            });

            modelBuilder.Entity<Discipline>(entity =>
            {
                entity.HasKey(e => e.IdDiscipline)
                    .HasName("PRIMARY");

                entity.ToTable("discipline");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.IdDiscipline)
                    .HasColumnType("int(11)")
                    .HasColumnName("idDiscipline");

                entity.Property(e => e.Discipline1)
                    .HasMaxLength(200)
                    .HasColumnName("discipline");

                entity.Property(e => e.Equipement)
                    .HasColumnType("int(11)")
                    .HasColumnName("equipement")
                    .HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<Etudiant>(entity =>
            {
                entity.HasKey(e => e.IdEtudiant)
                    .HasName("PRIMARY");

                entity.ToTable("etudiant");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.HasIndex(e => e.Email, "Email")
                    .IsUnique();

                entity.Property(e => e.IdEtudiant)
                    .HasColumnType("int(11)")
                    .HasColumnName("idEtudiant");

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.Mdp)
                    .HasMaxLength(32)
                    .HasColumnName("mdp");

                entity.Property(e => e.Nom)
                    .HasMaxLength(100)
                    .HasColumnName("nom");

                entity.Property(e => e.Prenom)
                    .HasMaxLength(100)
                    .HasColumnName("prenom");

                entity.Property(e => e.Telephone)
                    .HasMaxLength(10)
                    .HasColumnName("telephone");
            });

            modelBuilder.Entity<Inscrit>(entity =>
            {
                entity.HasKey(e => new { e.IdCours, e.IdEtudiant })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("inscrit");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.HasIndex(e => e.IdEtudiant, "fk_inscrit_etudiant");

                entity.Property(e => e.IdCours)
                    .HasColumnType("int(50)")
                    .HasColumnName("idCours");

                entity.Property(e => e.IdEtudiant)
                    .HasColumnType("int(50)")
                    .HasColumnName("idEtudiant");

                entity.Property(e => e.StudentStatus).HasColumnName("studentStatus");

                entity.HasOne(d => d.IdCoursNavigation)
                    .WithMany(p => p.Inscrits)
                    .HasForeignKey(d => d.IdCours)
                    .HasConstraintName("fk_inscrit_cours");

                entity.HasOne(d => d.IdEtudiantNavigation)
                    .WithMany(p => p.Inscrits)
                    .HasForeignKey(d => d.IdEtudiant)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_inscrit_etudiant");
            });

            modelBuilder.Entity<Salle>(entity =>
            {
                entity.HasKey(e => e.IdsalleDeClasse)
                    .HasName("PRIMARY");

                entity.ToTable("salle");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.IdsalleDeClasse)
                    .HasColumnType("int(11)")
                    .HasColumnName("idsalleDeClasse");

                entity.Property(e => e.Adresse)
                    .HasMaxLength(100)
                    .HasColumnName("adresse");

                entity.Property(e => e.Capacite)
                    .HasMaxLength(100)
                    .HasColumnName("capacite");

                entity.Property(e => e.Nom)
                    .HasMaxLength(100)
                    .HasColumnName("nom");

                entity.Property(e => e.Ville)
                    .HasMaxLength(100)
                    .HasColumnName("ville");
            });

            modelBuilder.Entity<Typecour>(entity =>
            {
                entity.HasKey(e => e.IdTypeCours)
                    .HasName("PRIMARY");

                entity.ToTable("typecours");

                entity.HasCharSet("utf8")
                    .UseCollation("utf8_general_ci");

                entity.Property(e => e.IdTypeCours)
                    .HasColumnType("int(11)")
                    .HasColumnName("idTypeCours");

                entity.Property(e => e.LibelleCours)
                    .HasMaxLength(100)
                    .HasColumnName("libelleCours");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
