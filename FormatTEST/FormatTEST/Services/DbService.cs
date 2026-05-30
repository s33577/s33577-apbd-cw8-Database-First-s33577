using FormatTEST.DTOs;
using Microsoft.EntityFrameworkCore;
using FormatTEST.DTOs;
using FormatTEST.Models;
using FormatTEST.Services;
using FormatTEST.Data;
using Kolokwium.Exceptions;

namespace FormatTEST.Services;

public class DbService : IDbService
{
    private readonly UniversityTasksDbContext _context;
    public DbService(UniversityTasksDbContext context)
    {
        _context = context;
    }


    public async Task<List<CourseDto>> GetCoursesAsync(bool activeOnly)
    {
        var query = _context.Courses.AsNoTracking();

        if (activeOnly)
        {
            query = query.Where(c =>  c.IsActive);
        }


        return await query.Select(c => new CourseDto
            {
                CourseId = c.CourseId,
                Code = c.Code,
                Name = c.Name,
                Credits = c.Credits,
                AssignmentCount = c.Assignments.Count
            })
            .ToListAsync();
        
        

    }

    public async Task<List<AssignmentDto>> GetCourseAssignmentsAsync(int courseId, bool publishedOnly)
    {
        if (!await _context.Courses.AnyAsync(c => c.CourseId == courseId))
            throw new NotFoundException($"Course with ID {courseId} not found.");

        var query = _context.Assignments.AsNoTracking().Where(a => a.CourseId == courseId);

        if (publishedOnly)
            query = query.Where(a => a.IsPublished);

        return await query
            .Select(a => new AssignmentDto
            {
                AssignmentId = a.AssignmentId,
                Title = a.Title,
                DueDate = a.DueDate,
                MaxPoints = a.MaxPoints,
                IsPublished = a.IsPublished,
                SubmissionCount = a.Submissions.Count
            })
            .ToListAsync();
    }

    public Task<StudentDashboardDto> GetStudentDashboardAsync(int studentId)
    {
        throw new NotImplementedException();
    }

    public Task<SubmissionDto> CreateSubmissionAsync(CreateSubmissionDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<SubmissionDto> GradeSubmissionAsync(int submissionId, GradeSubmissionDto dto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteSubmissionAsync(int submissionId)
    {
        throw new NotImplementedException();
    }
}