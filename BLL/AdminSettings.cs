namespace BLL
{
    public static class AdminSettings
    {
        public const string Name = "admin";
        public const string Email = "mdsadid2003@gmail.com";
        public const string Password = "012345";

        public static bool IsAdminEmail(string email)
        {
            if (email == null || email.Length == 0)
            {
                return false;
            }

            return email.Trim().ToLower() == Email.ToLower();
        }
    }
}
