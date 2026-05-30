namespace FormatTEST.Models;


public partial class Student
{
    public string FullName => $"{FirstName} {LastName}";

    public bool hasAcademicEmail()
    {
        return Email.EndsWith("@students.exmaple.edu", StringComparison.OrdinalIgnoreCase);
    }
}