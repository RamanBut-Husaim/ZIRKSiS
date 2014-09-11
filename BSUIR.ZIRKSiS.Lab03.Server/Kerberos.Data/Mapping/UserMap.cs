using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Kerberos.Models;

namespace Kerberos.Data.Mapping
{
    internal sealed class UserMap : EntityTypeConfiguration<User>
	{
		#region Constructors and Destructors

		public UserMap()
		{
			this.HasKey(p => p.Key);
			Property(p => p.Key)
				.IsRequired()
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			Property(p => p.Email)
				.IsRequired()
				.HasMaxLength(User.MaxLengthFor.Email);
			Property(p => p.PasswordHash)
				.IsRequired()
				.HasMaxLength(User.MaxLengthFor.PasswordHash);
			Property(p => p.PasswordSalt)
				.IsRequired()
				.HasMaxLength(User.MaxLengthFor.PasswordSalt);
		}

		#endregion
	}
}