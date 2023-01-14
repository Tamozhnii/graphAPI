using System.ComponentModel.DataAnnotations;

namespace graphAPI.Models
{
  public class Node
  {
    [Required]
    public string? Id { get; set; }

    [Required]
    public string? Title { get; set; }

    [Required]
    public string? Manager { get; set; }
  }
}