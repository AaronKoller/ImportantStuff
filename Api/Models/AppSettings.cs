namespace Api.Models
{
    public class AppSettings
    {
        public ChildAppSetting ChildAppSetting { get; set; }
        public AnotherChildAppSetting AnotherChildAppSetting { get; set; }
    }

    public class ChildAppSetting
    {
        public string ThisIsAProperty { get; set; }
    }

    public class AnotherChildAppSetting
    {
        public string AnotherChildProperty { get; set; }
    }

    public class AggregateAppSetting
    {
        public string TogetherWeAreStronger { get; set; }
    }
}
