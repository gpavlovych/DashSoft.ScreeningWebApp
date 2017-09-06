using System;

namespace Screening.WebAPI.Core.Data
{
    public class Mark
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string Lesson { get; set; }
        public User Student { get; set; }
        public int Value { get; set; }
        public int StudentId { get; set; }
        public string TeacherName { get; set; }
    }
}
