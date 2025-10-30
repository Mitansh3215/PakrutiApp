namespace PakrutiApp.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public string OptionVata { get; set; } = null!;
        public string OptionPitta { get; set; } = null!;
        public string OptionKapha { get; set; } = null!;
    }
}
