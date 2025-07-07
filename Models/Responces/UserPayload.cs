using System.ComponentModel.DataAnnotations.Schema;

namespace project2025.Models.Responces
{
    public class UserPayload
    {
        public List<UserDto> Users { get; set; }
    }


    public class UserDto
    {
        public int id { get; set; }

        public string? name { get; set; }

        public string? email_id { get; set; }
    }
}
