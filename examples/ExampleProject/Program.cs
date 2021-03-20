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
            var idSecond = Id1.New();

            if (id == idSecond)
            {

            }

            var converter = TypeDescriptor.GetConverter(id);

            var stringValue = converter.ConvertToString(id);

            var backToId = (Id1) converter.ConvertFromString(stringValue);

            var classExample = new ExampleOwner
            {
                Id = Id2.FromInt(256),
            };

            var bsonData = classExample.ToJson();

            var doc = BsonDocument.Parse(bsonData);

            var deserialised = BsonSerializer.Deserialize<ExampleOwner>(doc);

            Id2 defaultState = default;

            if (defaultState != Id2.Zero)
            {
                throw new InvalidOperationException();
            }

            deserialised.Id.Equals(Id2.FromInt(256));
        }
    }

    [TypedId]
    public partial struct Id1
    {
    }

    [TypedId(IdBackingType.Int)]
    public partial struct Id2
    {
    }

    [TypedId]
    public partial struct Id22
    {
    }

    public class ExampleOwner
    {
        public Id2 Id { get; set; }
    }

}
