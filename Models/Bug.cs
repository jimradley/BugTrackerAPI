using System;

namespace BugTrackerApi.Models
{
    public class Bug
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateOpened { get; set; }
        public bool IsActive { get; set; }

        public int AssignedToUserId { get; set; }
        public User AssignedToUser { get; set; }
    }
}