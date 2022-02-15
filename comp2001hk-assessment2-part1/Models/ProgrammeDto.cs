using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace comp2001hk_assessment2_part1.Models
{
    [Table("Programmes", Schema = "cw2")]
    [Index(nameof(Programme_code), IsUnique = true)]
    public class ProgrammeDto
    {
        [Key]
        public int Id { get; set; }
        public string Programme_code { get; set; }
        public string Programme_title { get; set; }
    }
}
