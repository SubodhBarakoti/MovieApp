
using Common.Enums;

namespace Common.ViewModels
{
    public class DisplayMoviesViewModel
    {
        public IEnumerable<MovieViewModel> Movies { get; set; }
        public PagerViewModel pager { get; set; }
        public Guid? GenreId { get; set; }
        public MovieSortBy SortBy { get; set; }
    }
}
