﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FreeCourse.Services.Catolog.Models {
    public class Category {
        [BsonId] //MongoDb tarafından bu prop'un id olarak algılanmasını sağlar
        //Mongoda tutulan id değerini String olarak bize verir 0bjectId-> String / String-> ObjetId dönüşünü sağlar
        [BsonRepresentation(BsonType.ObjectId)] 
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
