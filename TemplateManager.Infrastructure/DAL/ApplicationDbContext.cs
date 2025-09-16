using Microsoft.EntityFrameworkCore;
using TemplateManager.Domain.Entities;

namespace TemplateManager.Infrastructure.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public ApplicationDbContext() : base(new DbContextOptions<ApplicationDbContext>()) { }

        public virtual DbSet<HtmlTemplate> HtmlTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HtmlTemplate>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<HtmlTemplate>()
                .Property(t => t.Name)
                .IsRequired();

            modelBuilder.Entity<HtmlTemplate>()
                .Property(t => t.Content)
                .IsRequired();

            modelBuilder.Entity<HtmlTemplate>().HasData(
                new HtmlTemplate(Guid.Parse("d3f6a1e4-8c2f-4a4d-a8e1-1b2c3d4e5f60"), "Invoice Template", "<h1>Invoice for {{Name}}</h1><p>Date: {{Date}}</p><p>Amount: {{Amount}}</p>"),
                new HtmlTemplate(Guid.Parse("a1b2c3d4-5e6f-7a8b-9c0d-1e2f3a4b5c6d"), "Certificate Template", "<div><h2>Certificate of Completion</h2><p>This certifies that {{Name}} completed the course on {{Date}}</p></div>")
            );
        }
    }
}
