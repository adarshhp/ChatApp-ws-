using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project2025.Models
{
    [Table("fortess_game")]
    public class Fortess
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("x")]
        public int X { get; set; }
        [Column("y")]
        public int Y { get; set; }
        [Column("count")]
        public int Count { get; set; }
    }
}
