namespace MovieCatalog.DTO
{
    public class MoviesPagedListDTO
    {
        public List<MovieElementDTO>? movies { get; set; }
        public PageInfoDTO pageInfo { get; set; }
    }
}
