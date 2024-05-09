using probny.Models;

namespace probny.Repositories;

public interface IAnimalRepository
{
    Task<AnimalDTO> GetAnimal(int id);
    Task<bool> CheckAnimal(int id);
    Task<bool> CheckOwner(int id);
    Task<bool> CheckProcedure(int id);
    Task<int> AddAnimal(AddAnimalDTO animal);
    Task AddAnimalWithProcedures(AddAnimalWithProceduresDTO animal);
    Task AddProcedureAnimal(int animalId, ProcedureWithDate procedure);

}