using System;

namespace Core.Entities
{
    public class Address     
    {
        public Address()
            : this(-1, string.Empty, string.Empty, 0, 0, 0, string.Empty, string.Empty, false, -1, null, -1, null)
        { }


        public Address(int id, string addressLine1, string addressLine2, int cityId, int stateId, int countryId, string zip, string remarks, bool isDeleted, int createdBy, DateTime? createdDate, int updatedBy, DateTime? updatedDate)
        {
            Id = id;
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            CityId = cityId;
            StateId = stateId;
            CountryId = countryId;
            Zip = zip;
            Remarks = remarks;
            IsDeleted = isDeleted;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
        }

        public int Id { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string Zip { get; set; }
        public string Remarks { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}














