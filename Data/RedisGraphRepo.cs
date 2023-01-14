using System.Text.Json;
using graphAPI.Models;
using StackExchange.Redis;

namespace graphAPI.Data
{
  public class RedisGraphRepo : IGraphRepo
  {
    private readonly IConnectionMultiplexer _redis;

    public RedisGraphRepo(IConnectionMultiplexer redis)
    {
      _redis = redis;  
    }

    public void SetGraph(Graph graph)
    {
      if(graph == null)
        throw new ArgumentOutOfRangeException(nameof(graph));

      var db = _redis.GetDatabase();

      if(graph.Nodes != null && graph.Edges != null){
        var nodes = new HashEntry[graph.Nodes.Count()];
        var edges = new HashEntry[graph.Edges.Count()];

        for (int i = 0; i < graph.Nodes.Count(); i++)
        {
          var n = graph.Nodes.ElementAt(i);

          if(!n.Id!.Contains("node:")){
            string newId = $"node:{Guid.NewGuid().ToString()}";
            foreach (var e in graph.Edges)
            {
              if(e.From == n.Id){
                e.From = newId;
              }
              if(e.To == n.Id){
                e.To = newId;
              }
            }
            n.Id = newId;
            n.Title += 1;
          }
          nodes.SetValue(new HashEntry(n.Id, JsonSerializer.Serialize(n)), i);
        }

        for (int i = 0; i < graph.Edges.Count(); i++)
        {
          var e = graph.Edges.ElementAt(i);

          if(!e.Id!.Contains("edge:")){
            e.Id = $"edge:{Guid.NewGuid().ToString()}";
          }
          edges.SetValue(new HashEntry(e.Id, JsonSerializer.Serialize(e)), i);
        }

        foreach (var key in db.HashKeys("nodes")) db.HashDelete("nodes", key);

        foreach (var key in db.HashKeys("edges")) db.HashDelete("edges", key);

        db.HashSet("nodes", nodes);
        db.HashSet("edges", edges);
      }
    }

    public Graph GetGraph()
    {
      var db = _redis.GetDatabase();

      var nodesHash = db.HashGetAll("nodes");
      var edgesHash = db.HashGetAll("edges");

      var nodes = Array.ConvertAll(nodesHash, n => JsonSerializer.Deserialize<Node>(n.Value)).ToList();
      var edges = Array.ConvertAll(edgesHash, e => JsonSerializer.Deserialize<Edge>(e.Value)).ToList();

      var graph = new Graph();

      graph.Nodes = nodes!;
      graph.Edges = edges!;

      return graph;
    }

    public Dictionary<string, string> GetMatrix() 
    {
      var edgesHash = _redis.GetDatabase().HashGetAll("edges");

      var edges = Array.ConvertAll(edgesHash, e => JsonSerializer.Deserialize<Edge>(e.Value)).ToList();

      var matrix = new Dictionary<string, string>();

      foreach (var edge in edges)
      {
        if(edge?.From != null && edge.To != null){
          var row = edge.From;
          var column = edge.To;

          matrix[$"{row},{column}"] = edge.Id!;
        }
      }

      return matrix;
    }

    private List<string> GetChildrens(Node node)
    {
      var result = new List<string>();
      var edgesHash = _redis.GetDatabase().HashGetAll("edges");
      var edges = Array.ConvertAll(edgesHash, e => JsonSerializer.Deserialize<Edge>(e.Value)).ToList();


      foreach (var edge in edges)
      {
        if(edge?.From == node.Id){
          result.Add(edge?.To!);
        }
      }

      return result;
    }

    public List<string> WaveBypass(string start, string finish)
    {
      var result = new List<string>();
      var list = new List<string>();

      
      var wave = GetChildrens();
      return result;
    }
  }
}