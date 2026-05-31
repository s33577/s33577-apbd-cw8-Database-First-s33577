using FormatTEST.Data;
using FormatTEST.DTOs;
using FormatTEST.Models;
using Microsoft.AspNetCore.Mvc;
using FormatTEST.Services;
using Kolokwium.Exceptions;

namespace FormatTEST.Controllers;


[ApiController]
[Route("api/[controller]")]
public class SubmissionsController : ControllerBase
{
    private readonly IDbService _service;
    public SubmissionsController(IDbService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubmission([FromBody] CreateSubmissionDto dto)    {
        try
        {
            var result = await _service.CreateSubmissionAsync(dto);
            return Created("/api/submissions/{result.SubmissionId}", result);
        }
        catch (NotFoundException e)
        {
            return BadRequest(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
        
    }
    
    
    [HttpPut("{idSubmission}/grade")]
    public async Task<IActionResult> GradeSubmission(int idSubmission, [FromBody] GradeSubmissionDto dto)
    {
        try
        {
            var result = await _service.GradeSubmissionAsync(idSubmission, dto);
            return Ok(result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    
    [HttpDelete("{idSubmission}")]
    public async Task<IActionResult> DeleteSubmission(int idSubmission)
    {
        try
        {
            await _service.DeleteSubmissionAsync(idSubmission);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (BadRequestException e)
        {
            return BadRequest(e.Message);
        }
    }
    
}