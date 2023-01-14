using graphAPI.Data;
using graphAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace graphAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class GraphController : ControllerBase
  {
    private readonly IGraphRepo _repo;

    public GraphController(IGraphRepo repo)
    {
      _repo = repo;
    }

    [HttpGet]
    public ActionResult<Graph> GetGraph(){
      return Ok(_repo.GetGraph());
    }

    [HttpPost]
    public ActionResult<Graph> SetGraph(Graph graph){
      _repo.SetGraph(graph);
      return CreatedAtAction(nameof(GetGraph), graph);
    }
  }
}