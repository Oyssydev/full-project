namespace ConverterClassLib
{
    public class User
    {
        public int id { get; set; }
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
