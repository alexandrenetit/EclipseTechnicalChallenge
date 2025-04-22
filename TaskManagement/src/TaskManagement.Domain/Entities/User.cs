using TaskManagement.Domain.Entities.Base;

namespace TaskManagement.Domain.Entities;

/// <summary>
/// Represents a user in the system
/// </summary>
public class User : Entity<Guid>
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public bool IsManager { get; private set; }

    private User() { }

    public User(Guid id, string name, string email, bool isManager = false)
    {
        Id = id;
        Name = name;
        Email = email;
        IsManager = isManager;
    }
}