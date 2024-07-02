namespace PracticeForRevision.Models
{
    public class PaginationInfo
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public string Url { get; set; }
    }
}
