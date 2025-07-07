using System.ComponentModel.DataAnnotations.Schema;

namespace project2025.Models.Responces
{
    public class AddXO
    {
        public string box_no { get; set; }
        public string box_value { get; set; }
        public int? userid { get; set; }
    }
}
