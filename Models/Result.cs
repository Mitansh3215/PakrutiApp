namespace PakrutiApp.Models
{
    public class Result
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;

        public string PrakritiType { get; set; } = null!;
        public string DietRecommendation { get; set; } = null!;
        public string LifestyleTips { get; set; } = null!;
    }
}
  