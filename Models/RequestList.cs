using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project2025.Models
{
    [Table("request_list")]
    public class RequestList
    {
        [Key]

        [Column("access_ids")]
        public int access_id { get; set; }

        [Column("user_id")]
        public int user_id { get; set; }

        [Column("group_id")]
        public int group_id { get; set; }

        [Column("is_deleted")]
        public int? is_deleted { get; set; }

        [Column("approval_status")]
        public int? approval_status { get; set; }
    }
}
