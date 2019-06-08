using System;
using Xunit;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using PlusN.MongoDB.Extras.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;

namespace PlusN.MongoDB.Extras.Conventions.Tests
{
    public class IdAsStringWithObjectIdAsBsonTypeConventionTest
    {
        [Fact]
        public void Ctor_Predicate_SetsPredicate()
        {
            Predicate<BsonMemberMap> subject = m => true;

            var target = new IdAsStringWithObjectIdAsBsonTypeConvention(subject);

            Assert.Same(subject, target.IdMemberMapFilter);
        }

        [Fact]
        public void Ctor_NoPredicate_UsesDefaultPredicate()
        {
            var target = new IdAsStringWithObjectIdAsBsonTypeConvention();

            Assert.Same(IdAsStringWithObjectIdAsBsonTypeConvention.StringIdIdMemberFilter, target.IdMemberMapFilter);
        }

        [Fact]
        public void Apply_MapWithStringId_SetsSerializer()
        {
            var target = new IdAsStringWithObjectIdAsBsonTypeConvention();
            var subject = new BsonClassMap<SampleWithStringId>(cm => cm.AutoMap()).GetMemberMap("Id");

            target.Apply(subject);

            Assert.IsType<StringSerializer>(subject.GetSerializer());
        }

        [Fact]
        public void Apply_MapWithStringId_SetsIdGenerator()
        {
            var target = new IdAsStringWithObjectIdAsBsonTypeConvention();
            var subject = new BsonClassMap<SampleWithStringId>(cm => cm.AutoMap()).GetMemberMap("Id");

            target.Apply(subject);

            Assert.IsType<StringObjectIdGenerator>(subject.IdGenerator);
        }


        [Fact]
        public void Apply_MapWithStringId_LeavesSerializer()
        {
            var target = new IdAsStringWithObjectIdAsBsonTypeConvention();
            var subject = new BsonClassMap<SampleWithIntId>(cm => cm.AutoMap()).GetMemberMap("Id");

            target.Apply(subject);

            Assert.IsNotType<StringSerializer>(subject.GetSerializer());
        }
        [Fact]
        public void Apply_MapWithStringId_NoIdGenerator()
        {
            var target = new IdAsStringWithObjectIdAsBsonTypeConvention();
            var subject = new BsonClassMap<SampleWithIntId>(cm => cm.AutoMap()).GetMemberMap("Id");

            target.Apply(subject);

            Assert.Null(subject.IdGenerator);
        }


        public class SampleWithStringId { public string Id; }
        public class SampleWithIntId { public int Id; }
    }
}
