using System.Collections.Generic;

namespace Siterm.Domain.Models
{
    public class User : DomainObject
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual IEnumerable<Instruction> Instructions { get; set; }
        public virtual IEnumerable<Instruction> PerformedInstructions { get; set; }
        public virtual IEnumerable<ServiceReport> ServiceReports { get; set; }
    }
}