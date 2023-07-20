namespace Common.ViewModels
{
    public class DiscussionViewModel
    {
        public Guid Id { get; set; }
        public string DiscussionText { get; set; }
        public string UserName { get; set; }
        public DateTime? Created { get; set; }
    }
}
