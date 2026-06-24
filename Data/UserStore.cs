using System.Collections.Concurrent;
using UserVaultApi.Models;

namespace UserVaultApi.Data;

/// <summary>
/// Simple thread-safe in-memory "vault" for users.
/// Registered as a Singleton so data persists for the lifetime of the running app
/// (it resets when the app restarts — swap this for EF Core + a real DB if you need persistence).
/// </summary>
public class UserStore
{
    private readonly ConcurrentDictionary<int, User> _users = new();
    private int _nextId = 0;

    public UserStore()
    {
        // Seed a couple of sample users so GET requests return something immediately.
        Add(new UserCreateDto { Name = "Ada Lovelace", Email = "ada@example.com", Password = "vault123" });
        Add(new UserCreateDto { Name = "Alan Turing", Email = "alan@example.com", Password = "vault456" });
    }

    public IEnumerable<User> GetAll() => _users.Values.OrderBy(u => u.Id);

    public User? GetById(int id) => _users.TryGetValue(id, out var user) ? user : null;

    public User Add(UserCreateDto dto)
    {
        var id = Interlocked.Increment(ref _nextId);
        var user = new User
        {
            Id = id,
            Name = dto.Name,
            Email = dto.Email,
            Password = dto.Password,
            CreatedAt = DateTime.UtcNow
        };
        _users[id] = user;
        return user;
    }

    public User? Update(int id, UserUpdateDto dto)
    {
        if (!_users.TryGetValue(id, out var existing))
        {
            return null;
        }

        existing.Name = dto.Name;
        existing.Email = dto.Email;
        existing.Password = dto.Password;
        existing.UpdatedAt = DateTime.UtcNow;

        _users[id] = existing;
        return existing;
    }

    public bool Delete(int id) => _users.TryRemove(id, out _);
}
