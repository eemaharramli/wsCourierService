using System.ComponentModel.DataAnnotations;

namespace wsCourierService.Models
{
    public class Courier
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Available";
    }
}
