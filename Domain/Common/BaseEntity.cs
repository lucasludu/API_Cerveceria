using System.ComponentModel.DataAnnotations;

namespace Domain.Common
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsActive { get; set; } = true;
        public string? CreatedBy { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
