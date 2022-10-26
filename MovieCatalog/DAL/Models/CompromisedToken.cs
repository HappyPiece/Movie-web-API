namespace MovieCatalog.DAL.Models
{
    public class CompromisedToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Token { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
