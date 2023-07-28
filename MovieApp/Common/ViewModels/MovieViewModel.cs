namespace Common.ViewModels
{
    public class MovieViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string GenreName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string MovieDuration { get; set; }
        public string ImagePath { get; set; }
        public decimal AverageRating { get; set; }
    }
}
