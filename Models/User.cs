using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project2025.Models
{
    [Table("userdetails")]
    public class User
    {
        [Key]
        [Column("id")]
        public int id { get; set; }

        [Column("username")]
        public string? name { get; set; }

        [Column("Password")]
        public string password { get; set; }

        [Column("is_deleted")]
        public int is_deleted { get; set; }

        [Column("email_id")]
        public string? email_id { get; set; }
    }
}
