using System.ComponentModel.DataAnnotations.Schema;

namespace project2025.Models.Responces
{
    public class RequestAcessPayload
    {
        public int user_id { get; set; }
        public int group_id { get; set; }
    }
}
