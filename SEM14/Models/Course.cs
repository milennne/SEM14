namespace SEM14.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Credit { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }

    }
}
