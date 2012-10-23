using AutoBot.Domain;

namespace AutoBot.Handlers
{
    internal class FakeHandler : IHandler
    {
        private readonly string respondsTo;

        public FakeHandler(string respondsTo)
        {
            this.respondsTo = respondsTo;
        }

        public bool Fired { get; private set; }

        public bool CanHandle(Message message)
        {
            return message.Body == respondsTo;
        }

        public void Receive(Message message)
        {
            Fired = true;
        }
    }
}
