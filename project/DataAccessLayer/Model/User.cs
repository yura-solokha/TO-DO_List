namespace DataAccessLayer.Model
{
    public partial class User : EntityBase
    {
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
