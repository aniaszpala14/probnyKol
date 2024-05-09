namespace probny.Models;

public class AddAnimalWithProceduresDTO
{
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime AdmissionDate { get; set; }
        public int OwnerId { get; set; }
        public IEnumerable<ProcedureWithDate> Procedures { get; set; } = new List<ProcedureWithDate>();
    }

    
    /// potrzebuje procedury bez id
    
    public class ProcedureWithDate
    {
        public int ProcedureId { get; set; }
        public DateTime Date { get; set; }
    }
