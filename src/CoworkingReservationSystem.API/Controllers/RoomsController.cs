using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly RoomService _roomService;

    public RoomsController(RoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _roomService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("available")]
    public async Task<IActionResult> Available(DateTime start, DateTime end)
    {
        if (end <= start) return BadRequest("Intervalo inválido");
    
        var rooms = await _roomService. GetAvailableAsync(start, end);
        // Projeção leve para DTO
        return Ok(rooms.Select(r => new { r.Id, r.Name, r.Capacity }));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RoomRequest dto)
    {
        var result = await _roomService.CreateRoomAsync(dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RoomRequest dto)
    {
        var result = await _roomService.UpdateRoomAsync(id, dto);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _roomService.DeleteRoomAsync(id);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}