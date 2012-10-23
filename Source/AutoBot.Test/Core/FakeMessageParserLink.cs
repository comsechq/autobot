using AutoBot.Domain;

namespace AutoBot.Core
{
    internal class FakeMessageParserLink : IMessageParserLink
    {
        public bool CanParse(string input)
        {
            return input == "CANPARSE";
        }

        public Message Parse(string input)
        {
            return new Message { Body = "Hello " + input, Type = MessageType.PrivateMessage };
        }
    }
}
