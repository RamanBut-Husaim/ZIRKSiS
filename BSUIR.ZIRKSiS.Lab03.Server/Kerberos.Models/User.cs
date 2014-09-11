namespace Kerberos.Models
{
    public class User : Entity<int>
    {
        public static class MaxLengthFor
        {
            public const int PasswordHash = 128;
            public const int PasswordSalt = 128;
            public const int Email = 70;
            public const int PasswordMinimum = 8;
        }

        private string _email;

        public string Email
        {
            get { return this._email; }
            private set { this._email = value; }
        }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public bool IsSignedIn { get; set; }

        public User()
        {

        }

        public User(string email)
        {
            this.IsSignedIn = false;
            this.Email = email;
        }

        public override string ToString()
        {
            return string.Format("{0}", this._email);
        }
    }
}
