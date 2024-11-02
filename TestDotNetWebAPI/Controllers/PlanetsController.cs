using Microsoft.AspNetCore.Mvc;
using TestDotNetWebAPI.Repositories;

namespace TestDotNetWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanetsController : ControllerBase
    {
        private readonly PlanetsRepository _repository;

        public PlanetsController(CharacterRepository repository) { }
        public IActionResult Index()
        {
            return View();
        }
    }
}
