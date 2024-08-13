using MediatR;
using MongoDB.Bson.Serialization.Attributes;

namespace ChildCareApplication.Domain
{
    public class ChildInformation : BaseEntities, IRequest<bool>
    {
        [BsonElement("firstname")]
        public string FirstName { get; set; }

        [BsonElement("lastname")]
        public string LastName { get; set; }

        [BsonElement("address")]
         public List<AddressDetail> Address { get; set; }

        [BsonElement("dateofbirth")]
        public DateTime DateOfBirth { get; set; }

        [BsonElement("parents")]
        public List<ParentDetail> Parents { get; set; }

        [BsonElement("allergies")]
        public List<AllergyDetail> Allergies { get; set; }

    }


}
