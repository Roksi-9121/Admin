namespace DTO
{
    public class User
    {
        public int User_id { get; set; }
        public string User_name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool Is_active { get; set; }
        public bool Is_admin { get; set; }
    }
}
