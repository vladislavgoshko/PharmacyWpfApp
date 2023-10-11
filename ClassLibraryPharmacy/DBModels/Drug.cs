namespace ClassLibraryPharmacy.DBModels;

public partial class Drug
{
    public int DrugId { get; set; }

    public string DrugName { get; set; } = null!;

    public string MainForm { get; set; } = null!;

    public string PharmaForm { get; set; } = null!;

    public string Dosage { get; set; } = null!;

    public string CurtForm { get; set; } = null!;

    public int Packing { get; set; }

    public string Manufacturer { get; set; } = null!;

    public string Producer { get; set; } = null!;

    public string Mnn { get; set; } = null!;

    public string Ftg { get; set; } = null!;

    public virtual ICollection<InstructionsDrug> InstructionsDrugs { get; set; } = new List<InstructionsDrug>();
    public override string ToString()
    {
        return $"DrugId: {DrugId}, " +
               $"DrugName: {DrugName}, " +
               $"MainForm: {MainForm}, " +
               $"PharmaForm: {PharmaForm}, " +
               $"Dosage: {Dosage}, " +
               $"CurtForm: {CurtForm}, " +
               $"Packing: {Packing}, " +
               $"Manufacturer: {Manufacturer}, " +
               $"Producer: {Producer}, " +
               $"Mnn: {Mnn}, " +
               $"Ftg: {Ftg}";
    }
}
