namespace AutoBot.Services
{
    internal class StubConfigService : ConfigService
    {
        public override string GetConfigurationDirectory()
        {
            return ".";
        }
    }
}
