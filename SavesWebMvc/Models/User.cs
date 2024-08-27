using System.ComponentModel.DataAnnotations;

namespace SalesWebMvc.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
    }
}
