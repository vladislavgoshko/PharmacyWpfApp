using ClassLibraryPharmacy;
using ClassLibraryPharmacy.DBModels;
using Microsoft.EntityFrameworkCore;

namespace BLLPharmacy
{
    public class Service
    {
        private ProjectPharmacyContext _context;
        public Service()
        {
            _context = new ProjectPharmacyContext();
            _context.Database.OpenConnection();
        }
        public List<Drug> getDrugsByName(string filter)
        {
            using (var context = _context)
            {
                return context.Drugs
                    .Where(d => d.DrugName.Contains(filter))
                    .OrderBy(d => d.DrugName)
                    .Take(20)
                    .ToList();
            }
        }
        public List<Drug> getDrugsByMNN(string filter)
        {
            using (var context = _context)
            {
                return context.Drugs
                    .Where(d => d.Mnn == filter)
                    .OrderBy(d => d.DrugName)
                    .Take(20)
                    .ToList();
            }
        }
        public List<Instruction> getDrugInstructionsByDrugID(int drugID)
        {
            using (var context = _context)
            {
                return context.Instructions
                .Where(i => i.InstructionsDrugs.
                    Any(id => id.DrugId == drugID))
                .ToList();
            }
        }
        public Drug CreateDrug(Drug drug)
        {
            using (var context = _context)
            {
                drug.DrugId = context.Drugs.OrderBy(x => x.DrugId).Last().DrugId + 1;
                context.Drugs.Add(drug);
                context.SaveChanges();
                return drug;
            }
        }
        public void UpdateDrug(Drug drug)
        {
            using (var context = _context)
            {
                context.Drugs.Update(drug);
                context.SaveChanges();
            }
        }
        public void UpdateOrCreateInstruction(Drug drug, Instruction instruction)
        {

            if (instruction.InstructionId == -1)
            {
                instruction.InstructionId = _context.Instructions.OrderBy(x => x.InstructionId).Last().InstructionId + 1;
                _context.Instructions.Add(instruction);
                _context.SaveChanges();
                CreateDrugInstruction(drug, instruction);
            }
            else
            {
                var con = new ProjectPharmacyContext();
                con.Instructions.Update(instruction);
                con.SaveChanges();
            }

        }
        public void CreateDrugInstruction(Drug drug, Instruction instruction)
        {

            _context.InstructionsDrugs.Add(new InstructionsDrug()
            {
                Id = _context.InstructionsDrugs.OrderBy(x => x.Id).Last().Id + 1,
                DrugId = drug.DrugId,
                InstructionId = instruction.InstructionId
            });
            _context.SaveChanges();

        }
        public void deleteDrug(Drug drug)
        {
            using (var context = _context)
            {
                context.Drugs.Remove(drug);
                context.SaveChanges();
            }
        }
    }
}
