using MongoDB.Bson.Serialization.Attributes;

namespace ChildCareApplication.Domain
{
    public class AllergyDetail
    {
        [BsonElement("allergyname")]
        public string AllergyName { get; set; }

        [BsonElement("serverity")]
        public string Severity { get; set; }

        [BsonElement("reaction")]
        public string Reaction { get; set; }

        [BsonElement("treatment")]
         public string Treatment { get; set; }
    }
}
