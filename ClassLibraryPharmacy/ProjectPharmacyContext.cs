using ClassLibraryPharmacy.DBModels;
using Microsoft.EntityFrameworkCore;

namespace ClassLibraryPharmacy;

public partial class ProjectPharmacyContext : DbContext
{
    public ProjectPharmacyContext()
    {
    }

    public ProjectPharmacyContext(DbContextOptions<ProjectPharmacyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Drug> Drugs { get; set; }

    public virtual DbSet<Instruction> Instructions { get; set; }

    public virtual DbSet<InstructionsDrug> InstructionsDrugs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=GOSHKOLAPTOP\\GOSHKO;Initial Catalog=projectPharmacy;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Drug>(entity =>
        {
            entity.HasKey(e => e.DrugId).HasName("PK__Drugs__908D66F64AA88DF0");

            entity.Property(e => e.DrugId)
                .ValueGeneratedNever()
                .HasColumnName("DrugID");
            entity.Property(e => e.CurtForm).HasMaxLength(200);
            entity.Property(e => e.Dosage).HasMaxLength(100);
            entity.Property(e => e.DrugName).HasMaxLength(200);
            entity.Property(e => e.Ftg)
                .HasMaxLength(300)
                .HasColumnName("FTG");
            entity.Property(e => e.MainForm).HasMaxLength(100);
            entity.Property(e => e.Manufacturer).HasMaxLength(200);
            entity.Property(e => e.Mnn)
                .HasMaxLength(300)
                .HasColumnName("MNN");
            entity.Property(e => e.PharmaForm).HasMaxLength(200);
            entity.Property(e => e.Producer).HasMaxLength(100);
        });

        modelBuilder.Entity<Instruction>(entity =>
        {
            entity.HasKey(e => e.InstructionId).HasName("PK__Instruct__CE06945100639D41");

            entity.Property(e => e.InstructionId)
                .ValueGeneratedNever()
                .HasColumnName("InstructionID");
            entity.Property(e => e.InstructionDescription).HasColumnType("ntext");
            entity.Property(e => e.InstructionName).HasMaxLength(300);
        });

        modelBuilder.Entity<InstructionsDrug>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Instruct__3214EC2752AD4697");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.DrugId).HasColumnName("DrugID");
            entity.Property(e => e.InstructionId).HasColumnName("InstructionID");

            entity.HasOne(d => d.Drug).WithMany(p => p.InstructionsDrugs)
                .HasForeignKey(d => d.DrugId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Instructi__DrugI__3C69FB99");

            entity.HasOne(d => d.Instruction).WithMany(p => p.InstructionsDrugs)
                .HasForeignKey(d => d.InstructionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Instructi__Instr__3B75D760");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
