using System;

namespace WebDAVTests
{
    /// <summary>
    /// The parameters for the configuration must be defined here
    /// </summary>
    internal class WebDavConfig
    {
        internal static readonly string WebDavLocalPath = "c:/";
        internal static readonly Uri WebDavUri = new Uri("http://localhost:8880/");
        internal static readonly Uri WebDavTestBaseUri = new Uri(WebDavUri, "your_folder/");
        internal static readonly string UserName = "your_username";
        internal static readonly string Password = "your_password";
        internal static readonly string Domain = "your_domain";
    }
}
