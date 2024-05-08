namespace probny.Models;

public class AddAnimalDTO
{
    public string Name { get; set; }
    public string Type { get; set; }
    public DateTime AdmissionDate { get; set; }
    public int OwnerId { get; set; }
}