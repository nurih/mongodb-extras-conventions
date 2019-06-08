using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace PlusN.MongoDB.Extras.Conventions
{
    /** Makes it easy to have a <see cref="String"/> Id field on a POCO but an ObjectId representation in the database */
    public class IdAsStringWithObjectIdAsBsonTypeConvention : ConventionBase, IMemberMapConvention
    {
        public static readonly Predicate<BsonMemberMap> StringIdIdMemberFilter = m =>
            {
                return m.MemberName.Equals("Id", StringComparison.InvariantCultureIgnoreCase) && m.MemberType == typeof(string);
            };
        /** <summary>Sets an <see cref="StringSerializer"/> 
        and a <see cref="StringSerializer"/> on the designated id field of the document object.
        </summary>
         */
        public IdAsStringWithObjectIdAsBsonTypeConvention()
        {
            this.IdMemberMapFilter = StringIdIdMemberFilter;
        }

        /** <summary>Sets an <see cref="StringSerializer"/> 
        and a <see cref="StringSerializer"/> on the designated id field of the document object.
        </summary>

        <paramref>idMemberMapFilter</paramref> supplies a filter to apply to the ClassMap's IdMemberMap. When this predicate returns true, this convention will apply a StringObjectIdGenerator and a StringSerializer to the Id member. When false, this convention leaves the BsonMemberMap as is.
         */
        public IdAsStringWithObjectIdAsBsonTypeConvention(Predicate<BsonMemberMap> idMemberMapFilter)
        {
            if (idMemberMapFilter == null) { throw new ArgumentNullException(nameof(idMemberMapFilter)); }
            this.IdMemberMapFilter = idMemberMapFilter;
        }
        public Predicate<BsonMemberMap> IdMemberMapFilter { get; private set; }
        public void Apply(BsonMemberMap memberMap)
        {
            var idMemberMap = memberMap.ClassMap.IdMemberMap;
            if (idMemberMap == null) { return; }
            if (this.IdMemberMapFilter(idMemberMap))
            {
                idMemberMap.SetIdGenerator(StringObjectIdGenerator.Instance).SetSerializer(new StringSerializer(BsonType.ObjectId));
            }
        }
    }
}
