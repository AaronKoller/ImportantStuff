using Bogus;
using Models.Models;

namespace TestInfrastructure
{
    public static class BogusData
    {
        public static ImmutableObjectsWithMatchingPropertyAndConstructorParameters MakeImmutableObject()
        {
            return new Faker<ImmutableObjectsWithMatchingPropertyAndConstructorParameters>()
                //.StrictMode(true)
                .CustomInstantiator(f => new ImmutableObjectsWithMatchingPropertyAndConstructorParameters(new Faker().Name.FirstName(), new Faker().Random.Number(0, 100)))
                .RuleFor(p => p.MutableProperty, f => new Faker().Name.FirstName())
                .Generate();
        }

        public static ImmutableObjectPropertyNamesHasJsonProperty MakeImmutableObject1()
        {
            return new Faker<ImmutableObjectPropertyNamesHasJsonProperty>()
                //.StrictMode(true)
                .CustomInstantiator(f => new ImmutableObjectPropertyNamesHasJsonProperty(new Faker().Name.FirstName(), new Faker().Random.Number(0, 100)))
                .RuleFor(p => p.MutableProperty, f => new Faker().Name.FirstName())
                .Generate();
        }
    }
}
