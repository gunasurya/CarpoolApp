using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarPoolService.Models.DBModels
{
    [Table("Users")]
    public partial class User
    {
        [Key]
        public int Id { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Image { get; set; }
    }
}
