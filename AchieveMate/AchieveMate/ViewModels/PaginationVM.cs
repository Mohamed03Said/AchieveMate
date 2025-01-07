namespace AchieveMate.ViewModels
{
    public class PaginationVM<Instance>
    {
        public Instance instance { get; set; }

        public int Page { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;
    }
}
