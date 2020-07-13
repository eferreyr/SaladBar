using SaladBarWeb.DBModels;

namespace SaladBarWeb.Models
{
    public class TestViewModel2
    {
        public string SchoolName { get; set; }

        public string StudentID { get; set; }

        public int StudentGrade { get; set; }

        public string StudentGender { get; set; }

        //[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true

        public TestViewModel2() { }

        public TestViewModel2(Students students)
        {
            SchoolName = students.School.Name;
            StudentID = students.StudentId;
            StudentGender = students.Gender;
            StudentGrade = (int)students.Grade;
            
        }
    }
}