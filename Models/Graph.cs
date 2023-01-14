using System.ComponentModel.DataAnnotations;

namespace graphAPI.Models
{
  public class Graph{
    public IEnumerable<Node>? Nodes { get; set;}
    public IEnumerable<Edge>? Edges { get; set;}
  }
}