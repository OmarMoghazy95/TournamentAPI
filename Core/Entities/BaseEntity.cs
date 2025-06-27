using System.ComponentModel.DataAnnotations;

namespace Tournament.Api.Core.Entities;
internal abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
}
