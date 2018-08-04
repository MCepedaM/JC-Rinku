using System.ComponentModel.DataAnnotations;

namespace Rinku.Domain
{
    public interface IEntity
    {
        [Key]
        int Id { get; set; }
    }
}
