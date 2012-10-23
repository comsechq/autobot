using AutoBot.Domain;
using Sugar;
using Sugar.Command;

namespace AutoBot.Services
{
    /// <summary>
    /// Service to allow the application to store and retrieve security credentials.
    /// </summary>
    public class CredentialService : ICredentialService
    {
        #region Dependencies
        
        /// <summary>
        /// Gets or sets the config service.
        /// </summary>
        /// <value>
        /// The config service.
        /// </value>
        public IConfigService ConfigService { get; set; }

        #endregion

        /// <summary>
        /// Gets the default credentials.
        /// </summary>
        /// <returns></returns>
        public Credentials GetCredentials()
        {
            // Get Configuration
            var config = ConfigService.GetConfig();

            var credentials = new Credentials
            {
                Server = config.GetValue("Credentials", "Server", string.Empty),
                Port = config.GetValue("Credentials", "Port", string.Empty).AsInt32(),
                Password = config.GetValue("Credentials", "Password", string.Empty),
                Nick = config.GetValue("Credentials", "Nick", "bot"),
                Name = config.GetValue("Credentials", "Name", "bot")
            };

            return credentials;
        }

        /// <summary>
        /// Sets the credentials.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        public void SetCredentials(Credentials credentials)
        {
            // Get Configuration
            var config = ConfigService.GetConfig();

            // Set values
            config.SetValue("Credentials", "Server", credentials.Server);
            config.SetValue("Credentials", "Port", credentials.Port.ToString());
            config.SetValue("Credentials", "Password", credentials.Password);
            config.SetValue("Credentials", "Nick", credentials.Nick);
            config.SetValue("Credentials", "Name", credentials.Name);

            // Save to disk
            ConfigService.SetConfig(config);
        }

        /// <summary>
        /// Determines if the credentials for the IRC service have been set.
        /// </summary>
        /// <returns></returns>
        public bool CredentialsSet()
        {
            // Get Configuration
            var config = ConfigService.GetConfig();

            return !string.IsNullOrWhiteSpace(config.GetValue("Credentials", "Server", string.Empty));
        }

        /// <summary>
        /// Sets the credentials from the command line.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Credentials ParseCredentials(Parameters parameters)
        {
            var credentials = new Credentials
            {
                Server = parameters.AsString("s"),
                Port = parameters.AsInteger("p"),
                Password = parameters.AsString("pwd"),
                Nick = parameters.AsString("n"),
                Name = parameters.AsString("r")
            };

            return credentials;
        }

        /// <summary>
        /// Validates the specified credentials.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool Validate(Credentials credentials)
        {
            if (string.IsNullOrEmpty(credentials.Server))
            {
                return false;
            }

            if (credentials.Port == 0)
            {
                return false;
            }

            if (string.IsNullOrEmpty(credentials.Nick))
            {
                return false;
            }

            return true;
        }
    }
}
