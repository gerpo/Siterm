using System;
using System.Collections.Generic;
using System.Text;

namespace Siterm.Domain.Models
{
    public class Device: DomainObject
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public int DeviceNumber { get; set; }
        public Facility Facility { get; set; }
        public User Chief { get; set; }
        public IEnumerable<Instruction> Instructions { get; set; }
        public IEnumerable<ServiceReport> ServiceReports { get; set; }
    }
}
