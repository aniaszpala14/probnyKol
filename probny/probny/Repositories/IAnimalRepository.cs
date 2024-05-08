using probny.Models;

namespace probny.Repositories;

public interface IAnimalRepository
{
    Task<AnimalDTO> GetAnimal(int id);
    Task<bool> CheckAnimal(int id);

    Task<int> AddAnimal(AddAnimalDTO animal);
    Task<int> AddAnimalWithProcedures(AddAnimalWithProceduresDTO animal);
}