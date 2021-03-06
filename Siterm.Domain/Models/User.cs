﻿using System.Collections.Generic;
using Siterm.Support.Misc;

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
        public string FullName => $"{FirstName} {LastName}".ToTitleCase();
    }
}