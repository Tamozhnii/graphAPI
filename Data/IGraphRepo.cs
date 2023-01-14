using graphAPI.Models;

namespace graphAPI.Data
{
  public interface IGraphRepo
  {
    void SetGraph(Graph graph);
    Graph? GetGraph();
  }
}