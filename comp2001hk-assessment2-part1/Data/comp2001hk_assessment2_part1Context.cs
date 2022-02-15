#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using comp2001hk_assessment2_part1.Models;

namespace comp2001hk_assessment2_part1.Data
{
    public class comp2001hk_assessment2_part1Context : DbContext
    {
        public comp2001hk_assessment2_part1Context (DbContextOptions<comp2001hk_assessment2_part1Context> options)
            : base(options)
        {
        }

        public DbSet<comp2001hk_assessment2_part1.Models.Programme> Programmes { get; set; }
        public DbSet<comp2001hk_assessment2_part1.Models.Student> Students { get; set; }
        public DbSet<comp2001hk_assessment2_part1.Models.ProgrammeAudit> ProgrammeAudit { get; set; }
        public DbSet<comp2001hk_assessment2_part1.Models.ProgrammeStudentAudit> ProgrammeStudentAudit { get; set;}
    }
}
