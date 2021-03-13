using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using System;
using System.ComponentModel;
using System.IO;
using TypedIds;

namespace ExampleProject
{
    public static class Program
    {
        public static void Main()
        {
            var id = Id1.New();

            var converter = TypeDescriptor.GetConverter(id);

            var stringValue = converter.ConvertToString(id);

            var backToId = (Id1) converter.ConvertFromString(stringValue);

            var classExample = new ExampleOwner
            {
                Id = Id1.New(),
            };

            var bsonData = classExample.ToJson();

            var doc = BsonDocument.Parse(bsonData);

            var deserialised = BsonSerializer.Deserialize<ExampleOwner>(doc);
        }
    }

    [TypedId]
    public partial struct Id1
    {
    }

    [TypedId]
    internal partial struct Id2
    {

    }

    public class ExampleOwner
    {
        public Id1 Id { get; set; }
    }

}
