namespace AutoAttend
{
    public class AppSetting
    {
        public NetworkProfile NetworkProfile { get; set; }
        public UserProfile UserProfile { get; set; }
        public int MaxErrorTimes { get; set; }
        public string ChuUrl { get; set; }
    }

    public class NetworkProfile
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }

    public class UserProfile
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}