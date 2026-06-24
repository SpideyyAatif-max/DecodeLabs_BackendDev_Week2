using Microsoft.AspNetCore.Mvc;
using UserVaultApi.Data;
using UserVaultApi.Models;

namespace UserVaultApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly UserStore _store;

    public UsersController(UserStore store)
    {
        _store = store;
    }

    /// <summary>GET /api/users — returns every user in the vault.</summary>
    [HttpGet]
    public ActionResult<IEnumerable<User>> GetAll()
    {
        return Ok(_store.GetAll());
    }

    /// <summary>GET /api/users/{id} — returns a single user by id.</summary>
    [HttpGet("{id:int}")]
    public ActionResult<User> GetById(int id)
    {
        var user = _store.GetById(id);
        if (user is null)
        {
            return NotFound(new { message = $"User with id {id} was not found." });
        }
        return Ok(user);
    }

    /// <summary>POST /api/users — creates a new user.</summary>
    [HttpPost]
    public ActionResult<User> Create([FromBody] UserCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email))
        {
            return BadRequest(new { message = "Name and Email are required." });
        }

        var created = _store.Add(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>PUT /api/users/{id} — fully updates an existing user.</summary>
    [HttpPut("{id:int}")]
    public ActionResult<User> Update(int id, [FromBody] UserUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email))
        {
            return BadRequest(new { message = "Name and Email are required." });
        }

        var updated = _store.Update(id, dto);
        if (updated is null)
        {
            return NotFound(new { message = $"User with id {id} was not found." });
        }
        return Ok(updated);
    }

    /// <summary>DELETE /api/users/{id} — removes a user from the vault.</summary>
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var deleted = _store.Delete(id);
        if (!deleted)
        {
            return NotFound(new { message = $"User with id {id} was not found." });
        }
        return NoContent();
    }
}
