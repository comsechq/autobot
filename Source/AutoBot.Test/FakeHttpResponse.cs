using Sugar.Net;

namespace AutoBot
{
    internal class FakeHttpResponse : HttpResponse
    {
        private readonly string contents;

        public FakeHttpResponse(string contents)
        {
            this.contents = contents;
        }

        public override string ToString()
        {
            return contents;
        }

    }
}
