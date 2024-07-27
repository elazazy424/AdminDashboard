using AdminDashboard.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminDashboard.DAL.Data.Config
{
	public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.ToTable("Users");
			builder.Property(p => p.FirstName).HasMaxLength(50);
			builder.Property(p => p.LastName).HasMaxLength(50);
			builder.Property(p => p.Address).HasMaxLength(100);
			builder.Property(p => p.CreatedAt).HasDefaultValueSql("GETDATE()");
			//computed column
			builder.Property(p => p.DisplayName)
				   .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");
		}
	}
}
