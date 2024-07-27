namespace AdminDashboard.Dtos.Auth
{
	public class LoginServiceResponseDto
	{
		public string NewToken { get; set; }
        public UserInfoResultDto UserInfo { get; set; }
    }
}
