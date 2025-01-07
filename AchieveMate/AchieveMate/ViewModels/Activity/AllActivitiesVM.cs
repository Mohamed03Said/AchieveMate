namespace AchieveMate.ViewModels.Activity
{
    public class AllActivitiesVM
    {
        public List<ActivitiesListVM>? InProgress { get; set; }
        public List<ActivitiesListVM>? ToDo { get; set;}
        public List<ActivitiesListVM>? Finished { get; set; }
    }
}
