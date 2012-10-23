using AutoBot.Domain;
using Sugar.Command;

namespace AutoBot.Services
{
    /// <summary>
    /// Service interface for storing and retrieving IRC credentials.
    /// </summary>
    public interface ICredentialService
    {
        /// <summary>
        /// Gets the default credentials.
        /// </summary>
        /// <returns></returns>
        Credentials GetCredentials();

        /// <summary>
        /// Sets the credentials.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        void SetCredentials(Credentials credentials);

        /// <summary>
        /// Determines if the credentials for the IRC service have been set.
        /// </summary>
        /// <returns></returns>
        bool CredentialsSet();

        /// <summary>
        /// Parses the parameters and returns the credientials.
        /// </summary>
        /// <returns></returns>
        Credentials ParseCredentials(Parameters parameters);

        /// <summary>
        /// Validates the specified credentials.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <returns></returns>
        bool Validate(Credentials credentials);
    }
}
