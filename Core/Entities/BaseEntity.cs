using System.ComponentModel.DataAnnotations;

namespace Tournament.Api.Core.Entities;
public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
}
