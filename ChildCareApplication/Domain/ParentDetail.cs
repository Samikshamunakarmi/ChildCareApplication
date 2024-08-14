using MediatR;
using MongoDB.Bson.Serialization.Attributes;

namespace ChildCareApplication.Domain
{
    public class ParentDetail :BaseEntities, IRequest<bool>
    {

        [BsonElement("firstname")]
        public string FirstName { get; set; }

        [BsonElement("lastname")]

        public string LastName { get; set; }

        [BsonElement("relationship")]
        public string Relationship { get; set; }

        [BsonElement("mobnumber")]
        public string ContactNumber { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("dob")]
        public DateTime DateOfBirth { get; set; }


        [BsonElement("address")]
        public AddressDetail Address { get; set; }

        // New properties for child reference
        [BsonElement("childId")]
        public string ChildId { get; set; }

        [BsonElement("childFullName")]
        public string ChildFullName { get; set; }

    }
}
