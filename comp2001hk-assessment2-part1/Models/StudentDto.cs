using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace comp2001hk_assessment2_part1.Models
{
    [Table("Students", Schema = "cw2")]
    public class StudentDto
    {
        [Key]
        public int Student_id { get; set; }
        public string Name { get; set; }
    }
}
