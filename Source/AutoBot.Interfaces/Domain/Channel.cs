﻿using System;

namespace AutoBot.Domain
{
    /// <summary>
    /// Represents a Channel on the IRC server.
    /// </summary>
    public class Channel
    {
        /// <summary>
        /// Gets or sets the channel id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the topic.
        /// </summary>
        /// <value>
        /// The topic.
        /// </value>
        public string Topic { get; set; }

        /// <summary>
        /// Gets or sets the created.
        /// </summary>
        /// <value>
        /// The created.
        /// </value>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the last active.
        /// </summary>
        /// <value>
        /// The last active.
        /// </value>
        public DateTime LastActive { get; set; }

        /// <summary>
        /// Gets or sets the owner id.
        /// </summary>
        /// <value>
        /// The owner id.
        /// </value>
        public int OwnerUserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is archived.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is archived; otherwise, <c>false</c>.
        /// </value>
        public bool IsArchived { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this channel is a one-on-one chat.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is chat; otherwise, <c>false</c>.
        /// </value>
        public bool IsChat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is private.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is private; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrivate { get; set; }
    }
}
