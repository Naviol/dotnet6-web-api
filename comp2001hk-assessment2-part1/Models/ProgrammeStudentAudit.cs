using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace comp2001hk_assessment2_part1.Models
{
    [Table("ProgrammeStudentAudit", Schema = "cw2")]
    public class ProgrammeStudentAudit
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Operation { get; set; }
        public int? ProgrammeId_before { get; set; }
        public int? ProgrammeId_after { get; set; }
        public int? StudentId_before { get; set; }
        public int? StudentId_after { get; set; }
    }
}
