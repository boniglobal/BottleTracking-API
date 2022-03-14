namespace Entities
{
    public class PanelUser: BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public int Type { get; set; }
        public bool Deleted { get; set; }
    }
}
