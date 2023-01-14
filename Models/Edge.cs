using System.ComponentModel.DataAnnotations;

namespace graphAPI.Models
{
  public class Edge
  {
    [Required]
    public string? Id { get; set; }

    [Required]
    public string? From { get; set; }

    [Required]
    public string? To { get; set; }
  }
}