# Conventions

MongoDB .Net conventions to help cover common concerns and grunt code.

## Id As String With ObjectId as BsonType Convention

This convention helps set a string `Id` member of a POCO to be internally (in MongoDB) represented by an ObjectId.

This can help with serialization and REST API since the JSON representation then becomes a plain string, without the need of other side parsing ObjectId().

This helps POCO avoid the inclusion of MongoDB libraries in case you want to share the POCO but don't want others to worry about DLL versioning beyond the POCO. Using this convention you simply name the class's field "Id" and set it's type to String. No attributes are needed.

