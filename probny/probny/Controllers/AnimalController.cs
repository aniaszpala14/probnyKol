using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using probny.Models;
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
        if (!await _animalRepository.CheckAnimal(id))
            return NotFound($"Animal with given ID - {id} doesn't exist");

        var animal = await _animalRepository.GetAnimal(id);
        return Ok(animal);
    }
    // Version with implicit transaction
        [HttpPost]
        public async Task<IActionResult> AddAnimal(AddAnimalWithProceduresDTO animal)
        {
            if (!await _animalRepository.CheckOwner(animal.OwnerId))
                return NotFound($"Owner doesn't exist");

            foreach (var procedure in animal.Procedures)
            {
                if (!await _animalRepository.CheckProcedure(procedure.ProcedureId))
                    return NotFound($"Procedure doesn't exist");
            }

            await _animalRepository.AddAnimalWithProcedures(animal);

            return Created(Request.Path.Value ?? "api/animals", animal);
        }
        
        // Version with transaction scope
        [HttpPost]
        [Route("with-scope")]
        public async Task<IActionResult> AddAnimalV2(AddAnimalWithProceduresDTO animal)
        {

            if (!await _animalRepository.CheckOwner(animal.OwnerId))
                return NotFound("Owner doesn't exist");

            foreach (var procedure in animal.Procedures)
            {
                if (!await _animalRepository.CheckProcedure(procedure.ProcedureId))
                    return NotFound($"Procedure doesn't exist");
            }

            using(TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var id = await _animalRepository.AddAnimal(new AddAnimalDTO()
                {
                    Name = animal.Name,
                    Type = animal.Type,
                    AdmissionDate = animal.AdmissionDate,
                    OwnerId = animal.OwnerId
                });

                foreach (var procedure in animal.Procedures)
                {
                    await _animalRepository.AddProcedureAnimal(id, procedure);
                }

                scope.Complete();
            }

            return Created(Request.Path.Value ?? "api/animals", animal);
        }
    }