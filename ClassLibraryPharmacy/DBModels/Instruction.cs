namespace ClassLibraryPharmacy.DBModels;

public partial class Instruction
{
    public int InstructionId { get; set; }

    public string InstructionName { get; set; } = null!;

    public string InstructionDescription { get; set; } = null!;

    public virtual ICollection<InstructionsDrug> InstructionsDrugs { get; set; } = new List<InstructionsDrug>();
}
