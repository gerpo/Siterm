using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Siterm.Domain.Models
{
    public class Device: DomainObject
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int DeviceNumber { get; set; }
        public virtual Facility Facility { get; set; }
        public virtual User Chief { get; set; }
        public virtual IEnumerable<Instruction> Instructions { get; set; }
        public virtual IEnumerable<ServiceReport> ServiceReports { get; set; }
        public bool HasInstructions => Instructions.Any();
        public bool HasServiceReports => ServiceReports.Any();
    }
}
