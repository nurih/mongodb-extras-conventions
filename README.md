# Conventions

MongoDB .Net conventions to help cover common concerns and grunt code.

## Id As String With ObjectId as BsonType Convention

This convention helps set a string `Id` member of a POCO to be internally (in MongoDB) represented by an ObjectId.

This can help with serialization and REST API since the JSON representation then becomes a plain string, without the need of other side parsing ObjectId().

This helps POCO avoid the inclusion of MongoDB libraries in case you want to share the POCO but don't want others to worry about DLL versioning beyond the POCO. Using this convention you simply name the class's field "Id" and set it's type to String. No attributes are needed.

### Usage

Register an instance of the the convention into the `ConventionRegistry` through a new convention pack:

```csharp
// Somewhere in your program, before using any MongoDB for the first time:
var pack = new ConventionPack();
pack.Add(new StringObjectIdIdGeneratorConvention());

ConventionRegistry.Register(
    "Id as string on poco with ObjectId in the document",
    pack,
    t => true );

```

Once registered, the convention will do two things:

1. Set up the id field to have a string serializer.
2. Set up an ObjectId id generator for when you create a new object but don't assign the id explicitly.

Alternatively, you can supply the `StringObjectIdIdGeneratorConvention` a predicate to determine whether this convention should apply to it.
The default predicate checks for a class member named "id" (case-insensitive spelling) of type string.

The predicate takes a `BsonMemberMap` parameter as context. For example:

```csharp
var convention = new StringObjectIdIdGeneratorConvention((BsonMemberMap m) => {
        return m.MemberName.Equals("Id", StringComparison.InvariantCultureIgnoreCase)
        && m.MemberType == typeof(string);
    });
```

