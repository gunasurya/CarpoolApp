using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarPoolService.Models.DBModels
{
    [Table("Cities")]
    public partial class City
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}
