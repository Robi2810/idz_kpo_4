using System.ComponentModel.DataAnnotations;

namespace Api.Requests.DIsh;

public class DeleteDishRequest
{
    [Required] public long Id { get; set; }
}