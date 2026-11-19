namespace SEM14.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int GradeId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public bool IsActive { get; set; }

        public Grade Grade { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
