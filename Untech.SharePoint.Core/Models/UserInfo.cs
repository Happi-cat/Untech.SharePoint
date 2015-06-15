using System;
using Microsoft.SharePoint;

namespace Untech.SharePoint.Core.Models
{
    /// <summary>
    /// Class UserInfo
    /// </summary>
    [Serializable]
    public class UserInfo
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the login.
        /// </summary>
        /// <value>The login.</value>
        public string Login { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfo" /> class.
        /// </summary>
        public UserInfo()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfo" /> class.
        /// </summary>
        /// <param name="user">The user.</param>
        internal UserInfo(SPUser user)
        {
            Email = user.Email;
            Login = user.LoginName;
            Name = user.Name;
            Id = user.ID;
        }
    }
}
