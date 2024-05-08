using System.Runtime.InteropServices.JavaScript;

namespace probny.Models;

public class AnimalDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public DateTime AdmissionDate { get; set; }
    public OwnerDTO Owner { get; set; }
    public List<ProcedureDTO> Procedures { get; set; } = null!;
}

public class Procedure_AnimalDTO
{
    public int Procedure_Id { get; set; }
    public int Animal_Id { get; set; }
    public DateTime Date { get; set; }
}

public class OwnerDTO
{
    public int Id { get; set; }
    public string  FirstName { get; set; }
    public string LastName { get; set; }
}

public class ProcedureDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
}