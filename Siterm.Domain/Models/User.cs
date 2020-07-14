using System;
using System.Collections.Generic;
using System.Text;

namespace Siterm.Domain.Models
{
    public class User: DomainObject
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IEnumerable<Instruction> Instructions { get; set; }
        public IEnumerable<Instruction> PerformedInstructions { get; set; }
        public IEnumerable<ServiceReport> ServiceReports { get; set; }
    }
}
