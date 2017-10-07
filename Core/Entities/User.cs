using System;
using System.Net.Sockets;

namespace Core.Entities
{
    public class User : BaseEntitiy
    {
        public User()
            : this(
                -1, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty, null, -1, -1, string.Empty, false, -1, null, -1, null)
        {
            Address = new Address();
        }

        public User(int id, string userName, string password, string firstName, string middleName, string lastName,
            string fatherHusbandName, string email, string contactNo, byte[] profilePhoto, int designationId,
            int addressId, string remarks, bool isActive, int createdBy, DateTime? createdDate, int updatedBy,
            DateTime? updatedDate)
        {
            Id = id;
            UserName = userName;
            Password = password;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            FatherHusbandName = fatherHusbandName;
            Email = email;
            ContactNo = contactNo;
            ProfilePhoto = profilePhoto;
            DesignationId = designationId;
            AddressId = addressId;
            Remarks = remarks;
            IsActive = isActive;
            CreatedBy = createdBy;
            CreatedDate = createdDate;
            UpdatedBy = updatedBy;
            UpdatedDate = updatedDate;
        }

        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string FatherHusbandName { get; set; }
        public virtual string Email { get; set; }
        public virtual string ContactNo { get; set; }
        public virtual byte[] ProfilePhoto { get; set; }
        public virtual int DesignationId { get; set; }
        public virtual int AddressId { get; set; }
        public virtual string Remarks { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual int CreatedBy { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual int UpdatedBy { get; set; }
        public virtual DateTime? UpdatedDate { get; set; }


        public Address Address { get; set; }

        //to get all total number records from database.
        public int Total { get; set; }
    }
}