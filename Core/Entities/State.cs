using System;

namespace Core.Entities
{
    public class State
    {
        public State()
            : this(0, string.Empty, string.Empty,0, false,0, null, 0, null)
        { }
        public State(int id, string name, string description, int countryId, bool isActive, int? createdBy, DateTime? createdOn, int? updatedBy, DateTime? updatedOn)
        {
            Id = id;
            Name = name;
            Description = description;
            CountryId = countryId;
            IsActive = isActive;
            CreatedBy = createdBy;
            CreatedOn = createdOn;
            UpdatedBy = updatedBy;
            UpdatedOn = updatedOn;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CountryId { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}