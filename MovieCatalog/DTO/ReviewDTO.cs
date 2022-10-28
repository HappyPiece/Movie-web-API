namespace MovieCatalog.DTO
{
    public class ReviewDTO
    {
        // mandatory fields >>>
        public Guid id { get; set; }
        public int rating { get; set; }
        // <<< mandatory fields 

        public string? reviewText { get; set; }
        public bool isAnonymous { get; set; }
        public DateTime createDateTime { get; set; }
        public UserShortDTO? author { get; set; }
    }
}
