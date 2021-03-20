# TypedIds - A Typed ID Generator for .NET

It's often a good idea to avoid using primitives directly as your entity IDs (string, Guid, etc), because it's
easy to get ID values confused, and for bugs to creep in accidentally.

To get round this you'd often have to create custom wrapper types every time you want a new Typed ID, which can be a pain.

TypedIds helps you avoid that by automatically generating typed IDs for you!

TypedIds is built as a [.NET Roslyn Source Generator](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/); 
you'll need to be running the .NET 5 SDK and the latest Visual Studio version.

## Using It

Fairly straightforward; add a reference to the `TypedIds` NuGet package, then define a struct and add the `TypedId` attribute:

```csharp
// It's important that it's partial so we can add to it!
[TypedId]
public partial struct AccountId
{
}

public class Program
{
    static void Main(string[] args)
    {
        // Create a new unique ID.
        var accId = AccountId.New();

        // Outputs: 57f59c84b5924c88af544ba1091104b5 (or your unique value)
        Console.WriteLine(accId.ToString());
    }
}
```

When you add that `TypedId` attribute, we generate the implementation of your typed ID (which defaults to being backed by a GUID),
with standard equality operators, plus methods to parse identifiers from strings and access the underlying ID if required.

The code it generates should show up in Solution Explorer, under `Project -> Analyzers -> TypedIds -> TypedIds.Generator`.

## Customise the Backing Type

By default `TypedIds` uses a Guid as its backing value type. However, you can customise the type we store quite easily by passing parameters
to the `TypedId` attribute:

```csharp

// Backed by a 32-bit int
[TypedId(IdBackingType.Int)]
public partial struct IntId
{
}

// Backed by a 64-bit long
[TypedId(IdBackingType.Long)]
public partial struct LongId
{
}

// Backed by a string
[TypedId(IdBackingType.String)]
public partial struct StringId
{
}

```

## Support for Serialisers

TypedIds automatically adds a TypeConverter implementation, so your IDs should already "just work" in a variety of typical conversion situations, including
ASP.NET Core model binding.

Additionally, we also currently support the following serialisers/converters. If the relevant library is referenced in your project, we'll
generate the serialisers automatically for you:

| Format         | Referenced Library |
| -------------- | ------------------ |
| BSON (MongoDB) | MongoDB.Bson       |
| JSON           | Newtonsoft.Json    |

## Planned Features

- Built in serialiser support for:
    - System.Text.Json
    - Entity Framework
    - More?

Feel free to raise issues for any additional converters you want.