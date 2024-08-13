using MongoDB.Bson.Serialization.Attributes;

namespace ChildCareApplication.Domain
{
    public class AddressDetail
    {
        [BsonElement("state")]
        public string State { get; set; }

        [BsonElement("postalcode")]
        public int PostalCode { get; set; }

        [BsonElement("suburb")]
        public string Suburb { get; set; }

        [BsonElement("streetaddress")]
        public string StreetAddress { get; set; } 
    }
}
