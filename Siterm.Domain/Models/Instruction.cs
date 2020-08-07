using System;
using System.Collections.Generic;
using System.Text;

namespace Siterm.Domain.Models
{
    public class Instruction : DomainObject
    {
        public string Path { get; set; }
        public virtual User Instructor { get; set; }
        public virtual User Instructed { get; set; }
        public virtual Device Device { get; set; }
        public string OldInstructedString { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AllowedActivities { get; set; }
        public string ForbiddenActivities { get; set; }
    }
}
