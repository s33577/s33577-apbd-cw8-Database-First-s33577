using FormatTEST.DTOs;

namespace FormatTEST.Services;

public interface IDbService
{
    Task<List<CourseDto>> GetCoursesAsync(bool activeOnly);
    Task<List<AssignmentDto>> GetCourseAssignmentsAsync(int courseId, bool publishedOnly);
    Task<StudentDashboardDto> GetStudentDashboardAsync(int studentId);
    Task<SubmissionDto> CreateSubmissionAsync(CreateSubmissionDto dto);
    Task<SubmissionDto> GradeSubmissionAsync(int submissionId, GradeSubmissionDto dto);
    Task DeleteSubmissionAsync(int submissionId);
}