using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FreeCourse.Services.Catolog.Models {
    public class Course {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
        public string UserId { get; set; }
        public string Picture { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedTime { get; set; }

        //Feature ile bağlantı kurulması
        public Feature Feature { get; set; }

        //Kategori ile bağlantı kurulması
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }

        //Bu prop kodlama esnasında kullanılacak -> product dönerken kategoride dönmek amacıyla
        [BsonIgnore] //Mongo tarafında bir karşılığı yok, mongo db collection'lara satır olarak yansıtmaz, ignore eder
        public Category Category { get; set; }
    }
}
