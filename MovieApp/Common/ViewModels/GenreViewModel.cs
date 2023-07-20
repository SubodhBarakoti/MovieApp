using System.ComponentModel;

namespace Common.ViewModels
{
    public class GenreViewModel
    {
        public Guid Id { get; set; }
        [DisplayName("Genre Name")]
        public string Name { get; set; }
    }
}
