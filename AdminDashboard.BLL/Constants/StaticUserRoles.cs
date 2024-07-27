namespace AdminDashboard.BLL.Constants
{
	public static class StaticUserRoles
	{
		public const string Admin = "Admin";
		public const string User = "User";
		public const string Owner = "Owner";
		public const string Manager = "Manager";

		public const string OwnerAdmin = "Owner,Admin";
		public const string OwnerAdminManager = "Owner,Admin,Manager";
		public const string OwnerAdminManagerUser = "Owner,Admin,Manager,User";
	}
}
