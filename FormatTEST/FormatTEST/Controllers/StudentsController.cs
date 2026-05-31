using FormatTEST.Services;
using Kolokwium.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FormatTEST.Controllers;

[ApiController]
[Route("api/[controller]")]

public class StudentsController : ControllerBase
{
    private readonly IDbService _service;
    public StudentsController(IDbService service)
    {
        _service = service;
    }

    [HttpGet("{idStudent}/dashboard")]
    public async Task<IActionResult> GetDashboardAsync(int idStudent)
    {
        try
        {
            var dashboard = await _service.GetStudentDashboardAsync(idStudent);
            return Ok(dashboard);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        
    }

    
    
    
}