using System;
using System.Collections.Generic;
using System.Text;

namespace Siterm.Domain.Models
{
    public class ServiceReport: DomainObject
    {
        public enum ValidityType:int
        {
            Weekly = 7,
            Monthly = 30,
            HalfYearly = 182,
            Yearly = 365,
            Biyearly = 730
        }

        public User PerformingUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public ValidityType Validity { get; set; }
        public IEnumerable<ServiceTask> ServiceTasks { get; set; }
    }
}
