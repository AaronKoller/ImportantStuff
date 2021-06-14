using FluentAssertions;
using Models.Models;
using Newtonsoft.Json;
using TestInfrastructure;
using Xunit;

namespace Models.Unit.Tests
{
    public class ImmutableObjectsWithMatchingPropertyAndConstructorParametersTests
    {
        private ImmutableObjectsWithMatchingPropertyAndConstructorParameters _originalObject;
        private ImmutableObjectsWithMatchingPropertyAndConstructorParameters _clonedObject;

        [Fact]
        public void ShouldSerializeAndDeserializeImmutableObject()
        {
            GivenAnImmutableObject();

            WhenTheObjectIsSerializedAndDeserialized();

            ThenTheClonedObjectMatchesTheOriginalObject();
        }

        private void GivenAnImmutableObject()
        {
            _originalObject = BogusData.MakeImmutableObject();
        }

        private void WhenTheObjectIsSerializedAndDeserialized()
        {
            var objectSerialized = JsonConvert.SerializeObject(_originalObject);
            _clonedObject = JsonConvert.DeserializeObject<ImmutableObjectsWithMatchingPropertyAndConstructorParameters>(objectSerialized);
        }

        private void ThenTheClonedObjectMatchesTheOriginalObject()
        {
            _originalObject.Should().BeEquivalentTo(_clonedObject);
        }
    }
}
