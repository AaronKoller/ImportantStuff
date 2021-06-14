using FluentAssertions;
using Models.Models;
using Newtonsoft.Json;
using TestInfrastructure;
using Xunit;

namespace Models.Unit.Tests
{
    public class ImmutableObjectPropertyNamesHasJsonPropertyTests
    {
        private ImmutableObjectPropertyNamesHasJsonProperty _originalObject;
        private ImmutableObjectPropertyNamesHasJsonProperty _clonedObject;

        [Fact]
        public void ShouldSerializeAndDeserializeImmutableObject()
        {
            GivenAnImmutableObject();

            WhenTheObjectIsSerializedAndDeserialized();

            ThenTheClonedObjectMatchesTheOriginalObject();
        }

        private void GivenAnImmutableObject()
        {
            _originalObject = BogusData.MakeImmutableObject1();
        }

        private void WhenTheObjectIsSerializedAndDeserialized()
        {
            var objectSerialized = JsonConvert.SerializeObject(_originalObject);
            _clonedObject = JsonConvert.DeserializeObject<ImmutableObjectPropertyNamesHasJsonProperty>(objectSerialized);
        }

        private void ThenTheClonedObjectMatchesTheOriginalObject()
        {
            _originalObject.Should().BeEquivalentTo(_clonedObject);
        }
    }
}
