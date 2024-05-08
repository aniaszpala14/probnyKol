using Microsoft.AspNetCore.Mvc;
using probny.Repositories;

namespace probny.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnimalController : ControllerBase
{
    private IAnimalRepository _animalRepository;

    public AnimalController(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAnimal(int id)
    {
        
    }
}