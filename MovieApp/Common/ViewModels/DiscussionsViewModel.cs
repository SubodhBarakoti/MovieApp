namespace Common.ViewModels
{
    public class DiscussionsViewModel
    {
        public IEnumerable<DiscussionViewModel> Discussions { get; set; }
        public PagerViewModel pager { get; set; }
    }
}
