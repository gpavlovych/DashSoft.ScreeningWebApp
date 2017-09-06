namespace Screening.WebAPI.Core.Models.MarksController
{
    public class CreateMarkViewModel
    {
        public string LessonName { get; set; }

        public int StudentId { get; set; }

        public int Value { get; set; }
    }
}
