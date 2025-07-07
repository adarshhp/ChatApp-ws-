using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project2025.Models
{
    [Table("xo_game")]
    public class xogame_table
    {
        [Key]
        [Column("xo_id")]
        public int xo_id { get; set; }
        [Column("box_no")]
        public string box_no { get; set; }

        [Column("box_value")]
        public string box_value { get; set; }

        [Column("userid")]
        public int? userid { get; set; }
    }
}
