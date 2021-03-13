# TypedIds - A Typed ID Generator for .NET

It's often a good idea to avoid using primitives directly as your entity IDs (string, Guid, etc), because it's
easy to get ID values confused, and for bugs to creep in accidentally.

To get round this you'd often have to create custom wrapper types every time you want a new Typed ID, which can be a pain.

TypedIds helps you avoid that by automatically generating typed IDs for you!

> TypedIds is a very early work in progress!! It works for a simple case, but bear in mind it needs more work.

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

When you add that `TypedId` attribute, we generate the implementation of your typed ID (currently just backed by a GUID),
with standard equality operators, plus methods to parse identifiers from strings and access the underlying ID if required.

The code it generates should show up in Solution Explorer, under `Project -> Analyzers -> TypedIds -> TypedIds.Generator`.

## Support for Serialisers

TypedIds automatically adds a TypeConverter implementation, so your IDs will "just work" in normal ASP.NET Core model binding.

Additionally, we also support the following serialisers automatically. If the relevant library is referenced in your project, we'll
generate the serialisers for you:

| Format | Referenced Library |
| BSON (MongoDB) | MongoDB.Bson |

## Planned Features

TypedId is a very basic implementation right now, but I plan to add support for:

- More ID backing types (numerics, strings, etc)
- Built in serialiser support for:
    - System.Text.Json
    - Newtonsoft.Json
    - Entity Framework
    - More?

Feel free to raise issues for any additional converters you want.