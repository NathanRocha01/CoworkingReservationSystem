using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize] 
public class ReservationsController : ControllerBase
{
    private readonly ReservationService _reservationService;

    public ReservationsController(ReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetById()
    {
        int id = int.Parse(User.FindFirst("id")?.Value);
        var result = await _reservationService.GetUserReservationsAsync(id);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReservationRequest request)
    {
        int userID = int.Parse(User.FindFirst("id")?.Value);
        var result = await _reservationService.CreateReservationAsync(request, userID);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        int userID = int.Parse(User.FindFirst("id")?.Value);
        var result = await _reservationService.CancelReservationAsync(id, userID);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}