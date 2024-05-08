using Microsoft.Data.SqlClient;
using probny.Models;

namespace probny.Repositories;

public class AnimalRepository : IAnimalRepository
{
    //ZAWSZE
    public readonly IConfiguration _configuration;
    public AnimalRepository(IConfiguration configuration)
    { _configuration = configuration; }
    
    public async Task<AnimalDTO> GetAnimal(int id)
    {
        var query = @"SELECT  a.id as AnimalId,a.name as AnimalName,Type,AdmissionDate     ,o.id As OwnerId,FirstName,LastName    ,Date, p.name AS ProcedureName,p.description FROM Animal a
                    JOIN Owner o ON o.id=a.ownerId
                    JOIN Procedure_Animal pa ON pa.Animal_Id=a.id
                    JOIN Procedure p ON pa.Procedure_Id=p.id
                    WHERE a.id=@id";
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);

        await connection.OpenAsync();

        //JAK ODCZYT TO READER
        var reader = await command.ExecuteReaderAsync();

        var animalIdOrdinal = reader.GetOrdinal("AnimalId");
        var animalNameOrdinal = reader.GetOrdinal("AnimalName"); //nowe nazwy
        var animalTypeOrdinal = reader.GetOrdinal("Type");
        var animalADOrdinal = reader.GetOrdinal("AdmissionDate");

        var ownerIdOrdinal = reader.GetOrdinal("OwnerId");
        var ownerFirstNameOrdinal = reader.GetOrdinal("FirstName");
        var ownerLastNameOrdinal = reader.GetOrdinal("LastName");

        var dateOrdinal = reader.GetOrdinal("Date");

        var procedureNameOrdinal = reader.GetOrdinal("ProcedureName");
        var procedureDescriptionOrdinal = reader.GetOrdinal("Description");

        AnimalDTO result = null;
        
        
        while (await reader.ReadAsync())
        {
            if (result is not null)
            {
                result.Procedures.Add(new ProcedureDTO()
                {
                    Date = reader.GetDateTime(dateOrdinal),
                    Name = reader.GetString(procedureNameOrdinal),
                    Description = reader.GetString(procedureDescriptionOrdinal)
                });
            }
            else
            {
                result= new AnimalDTO()
                {
                    Id = reader.GetInt32(animalIdOrdinal),
                    Name = reader.GetString(animalNameOrdinal),
                    Type = reader.GetString(animalTypeOrdinal),
                    AdmissionDate = reader.GetDateTime(animalADOrdinal),
                    Owner = new OwnerDTO()
                    {
                        Id = reader.GetInt32(ownerIdOrdinal),
                        FirstName = reader.GetString(ownerFirstNameOrdinal),
                        LastName = reader.GetString(ownerLastNameOrdinal),
                    },
                    Procedures = new List<ProcedureDTO>()
                    {
                        new ProcedureDTO()
                        {
                            Date = reader.GetDateTime(dateOrdinal),
                            Name = reader.GetString(procedureNameOrdinal),
                            Description = reader.GetString(procedureDescriptionOrdinal)
                        }
                    }
                };
            }
        }

        if (result is null) throw new Exception();
        return result;
    }

    public async Task<bool> CheckAnimal(int id)
    {
        var query = @"SELECT 1 FROM Animal WHERE Id=@ID";
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);
        
        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return result is not null;
    }
    public async Task<bool> CheckOwner(int id)
    {
        var query = @"SELECT 1 FROM Owner WHERE Id=@ID";
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);
        
        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return result is not null;
    }
    public async Task<bool> CheckProcedure(int id)
    {
        var query = @"SELECT 1 FROM Procedure WHERE Id=@ID";
        
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@ID", id);
        
        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return result is not null;
    }
    
    //

    public async Task<int> AddAnimal(AddAnimalDTO animal)
    {
        var insert = @"INSERT INTO Animal VALUES(@Name, @Type, @AdmissionDate, @OwnerId);
					   SELECT @@IDENTITY AS ID;"; //kolejne idki
	    
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
	    
        command.Connection = connection;
        command.CommandText = insert;
	    
        command.Parameters.AddWithValue("@Name", animal.Name);
        command.Parameters.AddWithValue("@Type", animal.Type);
        command.Parameters.AddWithValue("@AdmissionDate", animal.AdmissionDate);
        command.Parameters.AddWithValue("@OwnerId", animal.OwnerId);
	    
        await connection.OpenAsync();
	    
        var id = await command.ExecuteScalarAsync();

        if (id is null) throw new Exception();
	    
        return Convert.ToInt32(id);
    }

    public async Task<int> AddAnimalWithProcedures(AddAnimalWithProceduresDTO animal)
    {
        
    }
}