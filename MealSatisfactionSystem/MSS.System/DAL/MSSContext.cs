using System.Data.Entity;
using MSS.Data.Entities.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using MSS.Data.Entities;

/// <summary>
/// Holds the context class for connection to our database.
/// </summary>
namespace MSSSystem.DAL
{
    /// <summary>
    /// Maps our Entites to the SQL database.
    /// </summary>
    public partial class MSSContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Creates the database from our entities if it doesn't exist.
        /// </summary>
        public MSSContext()
            : base("name=Survey")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<MSSContext>());
        }

        /// <summary>
        /// Answers maps the Answer table and entity to one another.
        /// </summary>
        public virtual DbSet<Answer> Answers { get; set; }

        /// <summary>
        /// Questions maps the Question table and entity to one another.
        /// </summary>
        public virtual DbSet<Question> Questions { get; set; }

        /// <summary>
        /// QuestionResponses maps the QuestionResponse table and entity to one another.
        /// </summary>
        public virtual DbSet<QuestionResponse> QuestionResponses { get; set; }

        /// <summary>
        /// Responses maps the Response table and entity to one another.
        /// </summary>
        public virtual DbSet<Response> Responses { get; set; }

        /// <summary>
        /// Sites maps the Site table and entity to one another.
        /// </summary>
        public virtual DbSet<Site> Sites { get; set; }

        /// <summary>
        /// Units maps the Unit table and entity to one another.
        /// </summary>
        public virtual DbSet<Unit> Units { get; set; }
        
        /// <summary>
        /// Builds the schema of the database for us, it maps the properties of the relationships between the tables to the entites.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Answer>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Answer>()
                .HasMany(e => e.QuestionResponses)
                .WithRequired(e => e.Answer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Answer>()
                .HasMany(e => e.Questions)
                .WithMany(e => e.Answers)
                .Map(m => m.ToTable("QuestionAnswer").MapLeftKey("AnswerId").MapRightKey("QuestionId"));

            modelBuilder.Entity<Question>()
                .Property(e => e.QuestionText)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .Property(e => e.SubQuestionText)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .Property(e => e.QuestionParameter)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .HasMany(e => e.QuestionResponses)
                .WithRequired(e => e.Question)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Response>()
                .Property(e => e.Age)
                .IsUnicode(false);

            modelBuilder.Entity<Response>()
                .Property(e => e.Gender)
                .HasColumnType("char");

            modelBuilder.Entity<Response>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<Response>()
                .HasMany(e => e.QuestionResponses)
                .WithRequired(e => e.Response)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Site>()
                .Property(e => e.SiteName)
                .IsUnicode(false);

            modelBuilder.Entity<Site>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Site>()
                .Property(e => e.Passcode)
                .IsUnicode(false);

            modelBuilder.Entity<Site>()
                .HasMany(e => e.Units)
                .WithRequired(e => e.Site)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Unit>()
                .Property(e => e.UnitName)
                .IsUnicode(false);

            modelBuilder.Entity<Unit>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Unit>()
                .HasMany(e => e.Responses)
                .WithRequired(e => e.Unit)
                .WillCascadeOnDelete(false);
        }
    }
}
