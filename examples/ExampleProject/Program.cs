namespace ExampleProject.NestedNameSpace;

using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using TypedIds;

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
            IdGuid = Id1.New(),
            Id = Id2Another.FromLong(256),
            TxtId = TextId.FromString("hello"),
        };

        var bsonData = classExample.ToJson();

        var doc = BsonDocument.Parse(bsonData);

        var deserialised = BsonSerializer.Deserialize<ExampleOwner>(doc);

        Id2Another defaultState = default;

        if (defaultState != Id2Another.Zero)
        {
            throw new InvalidOperationException();
        }

        deserialised.Id.Equals(Id2Another.FromLong(256));

        var txtId = TextId.FromString("id1");

        var txtId2 = TextId.FromString("id2");

        txtId2.Equals(txtId);

        //var jsonConverted = Newtonsoft.Json.JsonConvert.SerializeObject(classExample);

        //var reconstructed = Newtonsoft.Json.JsonConvert.DeserializeObject<ExampleOwner>(jsonConverted);

        //var 

        var jsonConverted = JsonSerializer.Serialize(classExample);

        var reconstructed = JsonSerializer.Deserialize<ExampleOwner>(jsonConverted);
    }
}

[TypedId]
public partial struct Id1
{
}

[TypedId(IdBackingType.Long)]
public partial struct Id2Another
{
}

[TypedId]
public partial struct Id22
{
}

[TypedId(IdBackingType.String)]
public partial struct TextId
{
}

public class ExampleOwner
{
    public Id1 IdGuid { get; set; }

    public Id2Another Id { get; set; }

    public TextId TxtId { get; set; }
}

