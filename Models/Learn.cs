using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project2025.Models
{
    [Table("learntable")]
    public class Learn
    {
        [Key]

        [Column("learn_id")]
        public int learn_id { get; set; }

        [Column("name")]
        public string name { get; set; }

        [Column("message")]
        public string message { get; set; }

    }
}
