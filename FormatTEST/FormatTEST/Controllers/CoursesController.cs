using FormatTEST.Data;
using FormatTEST.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FormatTEST.Controllers;

public class CoursesController : ControllerBase
{
    private readonly UniversityTasksDbContext _context;

    public CoursesController(UniversityTasksDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<IActionResult> getCourses([FromQuery] bool activeOnly = true)
    {
        
    }

    [HttpGet("{idCourse}/assignments")]

    public async Task<IActionResult> GetCourseAssignments(int idCourse, [FromQuery] bool publishedOnly = true)
    {


        var courseExists = await _context.Courses.AnyAsync(c => c.CourseId == idCourse);

        if (!courseExists)
        {
            return NotFound("Course not found");
        }
        
        var query = _context.Assignments.AsNoTracking().Where(a => a.CourseId == idCourse);

        if (publishedOnly)
        {
            query = query.Where(a => a.IsPublished);
        }
        
        var assigments = await query.Select(a => new AssignmentDto()
        {
            AssignmentId =  a.AssignmentId,
            Title =  a.Title,
            DueDate =  a.DueDate,
            MaxPoints =  a.MaxPoints,
            IsPublished =  a.IsPublished,
            SubmissionCount =  a.Submissions.Count
        }).ToListAsync();
        return Ok(assigments);
    }
    
}