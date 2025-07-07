using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project2025.Models
{
    [Table("useraccesstrack")]
    public class Access
    {
        [Key]

        [Column("access_id")]
        public int? access_id { get; set; }

        [Column("user_id")]
        public int user_id { get; set; }

        [Column("group_id")]
        public int group_id { get; set; }

        [Column("has_access")]
        public int? has_access { get; set; }


    }
}
