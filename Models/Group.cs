using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project2025.Models
{
    [Table("group_master")]
    public class Group
    {
        [Key]
        [Column("group_ids")]
        public int group_ids { get; set; }

        [Column("group_name")]
        public string ?group_name { get; set; }

        [Column("created_by")]
        public int created_by { get; set; }

        [Column("is_deleted")]
        public int is_deleted { get; set; }

    }
}
