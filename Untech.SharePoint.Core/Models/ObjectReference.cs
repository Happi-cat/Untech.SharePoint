using System;

namespace Untech.SharePoint.Core.Models
{
    /// <summary>
    /// Class ObjectReference
    /// </summary>
    [Serializable]
    public class ObjectReference
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }
        /// <summary>
        /// Gets or sets the list id.
        /// </summary>
        /// <value>The list id.</value>
        public Guid ListId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectReference" /> class.
        /// </summary>
        public ObjectReference()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectReference" /> class.
        /// </summary>
        /// <param name="listId">The list id.</param>
        /// <param name="id">The id.</param>
        /// <param name="value">The value.</param>
        public ObjectReference(Guid listId, int id, string value)
        {
            ListId = listId;
            Id = id;
            Value = value;
        }
    }
}
