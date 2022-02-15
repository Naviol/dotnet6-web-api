using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace comp2001hk_assessment2_part1.Models
{
    [Table("ProgrammeAudit", Schema = "cw2")]
    public class ProgrammeAudit
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Operation { get; set; }
        public int Programme_id { get; set; }
        public string? Programme_code_before { get; set; }
        public string? Programme_code_after { get; set; }
        public string? Programme_title_before { get; set; }
        public string? Programme_title_after { get; set; }
    }
}
