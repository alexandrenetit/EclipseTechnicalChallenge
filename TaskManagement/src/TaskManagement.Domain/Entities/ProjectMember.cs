using TaskManagement.Domain.Entities.Base;

namespace TaskManagement.Domain.Entities
{
    /// <summary>
    /// Represents a member of a project
    /// </summary>
    public class ProjectMember : Entity<Guid>
    {
        public Guid ProjectId { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime JoinedAt { get; private set; }

        private ProjectMember()
        { }

        public ProjectMember(Guid projectId, Guid userId)
        {
            ProjectId = projectId;
            UserId = userId;
            JoinedAt = DateTime.UtcNow;
        }
    }
}