namespace FormatTEST.DTOs;

public class SubmissionDto
{
    public int SubmissionId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string AssignmentTitle { get; set; } = string.Empty;
    public string RepositoryUrl { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int? Score { get; set; }
    public string? Feedback { get; set; }
}