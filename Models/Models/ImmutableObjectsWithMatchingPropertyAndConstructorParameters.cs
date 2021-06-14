namespace Models.Models
{
    public class ImmutableObjectsWithMatchingPropertyAndConstructorParameters
    {

        public readonly string Property1;
        public readonly int Property2;
        public string MutableProperty { get; set; }


        //When constructor names match property names (case insensetive) then newtonsoft can work with it
        public ImmutableObjectsWithMatchingPropertyAndConstructorParameters(string property1, int property2)
        {
            Property1 = property1;
            Property2 = property2;
        }
    }
}
