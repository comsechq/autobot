namespace AutoBot.Core.Samples
{
    public class Concrete
    {
        public IFoo Foo { get; set; }

        public string SayFoo()
        {
            return Foo.SayFoo();
        }
    }
}
