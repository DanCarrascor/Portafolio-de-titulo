using System.ComponentModel.DataAnnotations;

namespace sistema_contable.models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
