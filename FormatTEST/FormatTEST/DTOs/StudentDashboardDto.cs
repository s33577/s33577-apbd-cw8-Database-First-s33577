namespace FormatTEST.DTOs;

public class StudentDashboardDto
{
    public int StudentId { get; set; }
    public string IndexNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List<string> Enrollments { get; set; } = new();
    public List<SubmissionDto> Submissions { get; set; } = new();
}