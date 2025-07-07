namespace project2025.Models.Responces
{
    public class GetXOResponse
    {
        public int statuscode { get; set; }
        public string message { get; set; }
        public List<xogame_table> data { get; set; }
    }
}
