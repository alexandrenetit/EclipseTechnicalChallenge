using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities.Base;

namespace TaskManagement.Domain.Entities
{
    /// <summary>
    /// Represents a user in the system
    /// </summary>
    public class User : Entity<Guid>
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public bool IsManager { get; private set; }

        private readonly List<Project> _projects = new();
        public IReadOnlyCollection<Project> Projects => _projects.AsReadOnly();

        private User() { } // For EF Core

        public User(Guid id, string name, string email, bool isManager = false)
        {
            Id = id;
            Name = name;
            Email = email;
            IsManager = isManager;
        }
    }
}
