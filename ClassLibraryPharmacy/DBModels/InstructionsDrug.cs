namespace ClassLibraryPharmacy.DBModels;

public partial class InstructionsDrug
{
    public int Id { get; set; }

    public int InstructionId { get; set; }

    public int DrugId { get; set; }

    public virtual Drug Drug { get; set; } = null!;

    public virtual Instruction Instruction { get; set; } = null!;
}
