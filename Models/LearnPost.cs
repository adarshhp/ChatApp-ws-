using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project2025.Models
{
    [Table("learntable")]

    public class LearnPost
    {

        [Key]

        [Column("learn_id")]
        public int? learn_id { get; set; }

        [Column("name")]
        public string name { get; set; }

        [Column("message")]
        public string message { get; set; }

        [Column("is_deleted")]
        public int? is_deleted { get; set; } = 0;

        [Column("group_id")]
        public int group_id { get; set; }
    }
}
