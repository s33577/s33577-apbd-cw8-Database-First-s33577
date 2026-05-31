using FormatTEST.Data;
using FormatTEST.DTOs;
using FormatTEST.Services;
using Kolokwium.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FormatTEST.Controllers;


[ApiController]
[Route("api/[controller]")]

public class CoursesController : ControllerBase
{
    private readonly IDbService _db;
    public CoursesController(IDbService db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult> GetCourses([FromQuery] bool activeOnly = true)
    {
        var courses = await _db.GetCoursesAsync(activeOnly);
        return Ok(courses);
    }

    [HttpGet("{idCours}/assigments")]
    public async Task<IActionResult> GetCoursesAssigmentsAsync(int idCourse, [FromQuery] bool publishedOnly = true)
    {
        try
        {
            var assigments = await _db.GetCourseAssignmentsAsync(idCourse, publishedOnly);
            return Ok(assigments);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        
    }
    
    
    
}