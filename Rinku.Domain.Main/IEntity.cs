using System.ComponentModel.DataAnnotations;

namespace Rinku.Domain.Main
{
    public interface IEntity
    {
        [Key]
        int Id { get; set; }
    }
}
