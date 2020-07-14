using System;
using System.Collections.Generic;
using System.Text;

namespace Siterm.Domain.Models
{
    public class Instruction : DomainObject
    {
        public User Instructor { get; set; }
        public User Instructed { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AllowedActivities { get; set; }
        public string ForbiddenActivities { get; set; }
    }
}
