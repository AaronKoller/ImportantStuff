using Newtonsoft.Json;

namespace Models.Models
{
    public class ImmutableObjectPropertyNamesHasJsonProperty
    {
        [JsonProperty(nameof(Property1))]
        public readonly string Property1;
        [JsonProperty(nameof(Property2))]
        public readonly int Property2;
        public string MutableProperty { get; set; }


        //When constructor names don't match Newtonsoft needs JsonProperty
        public ImmutableObjectPropertyNamesHasJsonProperty(string parameter1, int parameter2)
        {
            Property1 = parameter1;
            Property2 = parameter2;
        }
    }
}