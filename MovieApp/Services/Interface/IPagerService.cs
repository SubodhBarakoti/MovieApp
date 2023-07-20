namespace Services.Interface
{
    public interface IPagerService
    {
        int CurrentPage { get; set; }
        int EndPage { get; set; }
        int PageSize { get; set; }
        int StartPage { get; set; }
        int TotalItems { get; set; }
        int TotalPages { get; set; }

        void PagerInitialize(int totalItems, int page, int pageSize = 10);
    }
}