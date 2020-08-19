using System;

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
        public DateTime ValidTill => CreatedAt.AddYears(1);
        public bool IsArchived { get; set; }

        public string OldInstructedFirstName => OldInstructedString?.Split(' ').Length > 1
            ? OldInstructedString?.Split(' ')[0]
            : string.Empty;

        public string OldInstructedLastName => OldInstructedString?.Split(' ').Length > 1
            ? OldInstructedString?.Split(' ')[1]
            : OldInstructedString?.Split(' ')[0];

        public int DeviceId { get; set; }
        public int? InstructorId { get; set; }
        public int? InstructedId { get; set; }
    }
}