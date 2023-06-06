using System.ComponentModel.DataAnnotations;

namespace Api.Requests.Order;

public class GetOrderRequest
{
    [Required]
    public long Id { get; set; }
}