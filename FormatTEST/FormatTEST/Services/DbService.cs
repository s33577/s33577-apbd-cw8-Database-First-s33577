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

    public async Task<StudentDashboardDto> GetStudentDashboardAsync(int studentId)
    {
        var dashboard = await _context.Students
            .AsNoTracking()
            .Where(s => s.StudentId == studentId)
            .Select(s => new StudentDashboardDto
            {
                StudentId = s.StudentId,
                IndexNumber = s.IndexNumber,
                FullName = s.FullName, 
                IsActive = s.IsActive,
                Enrollments = s.Enrollments.Select(e => $"{e.Course.Name} ({e.Status})").ToList(),
                Submissions = s.Submissions.Select(sub => new SubmissionDto
                {
                    SubmissionId = sub.SubmissionId,
                    StudentName = s.FullName,
                    AssignmentTitle = sub.Assignment.Title,
                    RepositoryUrl = sub.RepositoryUrl,
                    Status = sub.Status,
                    Score = sub.Score,
                    Feedback = sub.Feedback
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (dashboard == null)
        {
            throw new NotFoundException($"Student with ID {studentId} not found.");
        }

        return dashboard;
    }

    public async Task<SubmissionDto> CreateSubmissionAsync(CreateSubmissionDto dto)
    {
        var student = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == dto.StudentId);
        if (student == null) throw new NotFoundException("Student not found.");
        if (!student.IsActive) throw new BadRequestException("Student is not active.");
        
        var assignment = await _context.Assignments.FirstOrDefaultAsync(a => a.AssignmentId == dto.AssignmentId);
        if (assignment == null) throw new NotFoundException("Assignment not found.");
        if (!assignment.IsPublished) throw new BadRequestException("Assignment is not published.");
        
        
        
        var validEnrollment = await _context.Enrollments
            .AnyAsync(e => e.StudentId == dto.StudentId && e.CourseId == assignment.CourseId 
                                                        && (e.Status == "Active" || e.Status == "Completed"));
        
        if (!validEnrollment) throw new BadRequestException("Student is not enrolled in this course or status is invalid.");

        if (await _context.Submissions.AnyAsync(s =>
                s.StudentId == dto.StudentId && s.AssignmentId == dto.AssignmentId))
        {
            throw new BadRequestException("Student has already submitted this assignment.");
        }
        
        var submission = new Submission
        {
            AssignmentId = dto.AssignmentId,
            StudentId = dto.StudentId,
            RepositoryUrl = dto.RepositoryUrl,
            SubmittedAt = DateTime.Now,
            Status = assignment.IsOverdue(DateTime.Now) ? "Late" : "Submitted"
        };

        _context.Submissions.Add(submission);
        await _context.SaveChangesAsync();

        return new SubmissionDto
        {
            SubmissionId = submission.SubmissionId,
            Status = submission.Status
        };
        
        
    }

    public async Task<SubmissionDto> GradeSubmissionAsync(int submissionId, GradeSubmissionDto dto)
    {
        
        
        
        var submission = await _context.Submissions
            .Include(s => s.Assignment) 
            .FirstOrDefaultAsync(s => s.SubmissionId == submissionId);

        if (submission == null) throw new NotFoundException("Submission not found.");
        if (dto.Score < 0) throw new BadRequestException("Score cannot be lower than 0.");
        if (dto.Score > submission.Assignment.MaxPoints) 
            throw new BadRequestException($"Score cannot exceed the max points of {submission.Assignment.MaxPoints}.");

        submission.Score = dto.Score;
        submission.Feedback = dto.Feedback;
        submission.Status = "Graded";

        await _context.SaveChangesAsync(); 

        return new SubmissionDto
        {
            SubmissionId = submission.SubmissionId,
            Status = submission.Status,
            Score = submission.Score
        };
        
    }

    public async Task DeleteSubmissionAsync(int submissionId)
    {
        
        var submission = await _context.Submissions.FirstOrDefaultAsync(s => s.SubmissionId == submissionId);
        
        if (submission == null) throw new NotFoundException("Submission not found.");
        if (submission.Status == "Graded") throw new BadRequestException("Cannot delete a graded submission.");

        _context.Submissions.Remove(submission);
        await _context.SaveChangesAsync();
    }
}