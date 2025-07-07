using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project2025.Models
{
    [Table("participant_master")]
    public class Participation
    {
        [Key]
        [Column("pr_id")]
        public int pr_id { get; set; }

        [Column("group_id")]
        public int group_id { get; set; }

        [Column("user_id")]
        public int user_id { get; set; }

        [Column("is_deleted")]
        public int is_deleted { get; set; }
    }
}
