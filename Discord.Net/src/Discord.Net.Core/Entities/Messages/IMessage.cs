using System;
using System.Collections.Generic;

namespace Discord
{
    /// <summary>
    ///     Represents a message object.
    /// </summary>
    public interface IMessage : ISnowflakeEntity, IDeletable
    {
        /// <summary>
        ///     Gets the type of this system message.
        /// </summary>
        MessageType Type { get; }
        /// <summary>
        ///     Gets the source type of this message.
        /// </summary>
        MessageSource Source { get; }
        /// <summary>
        ///     Gets the value that indicates whether this message was meant to be read-aloud by Discord.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this message was sent as a text-to-speech message; otherwise <c>false</c>.
        /// </returns>
        bool IsTTS { get; }
        /// <summary>
        ///     Gets the value that indicates whether this message is pinned.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this message was added to its channel's pinned messages; otherwise <c>false</c>.
        /// </returns>
        bool IsPinned { get; }
        /// <summary>
        ///     Gets the content for this message.
        /// </summary>
        /// <returns>
        ///     A string that contains the body of the message; note that this field may be empty if there is an embed.
        /// </returns>
        string Content { get; }
        /// <summary>
        ///     Gets the time this message was sent.
        /// </summary>
        /// <returns>
        ///     Time of when the message was sent.
        /// </returns>
        DateTimeOffset Timestamp { get; }
        /// <summary>
        ///     Gets the time of this message's last edit.
        /// </summary>
        /// <returns>
        ///     Time of when the message was last edited; <c>null</c> if the message is never edited.
        /// </returns>
        DateTimeOffset? EditedTimestamp { get; }
        
        /// <summary>
        ///     Gets the source channel of the message.
        /// </summary>
        IMessageChannel Channel { get; }
        /// <summary>
        ///     Gets the author of this message.
        /// </summary>
        IUser Author { get; }

        /// <summary>
        ///     Gets all attachments included in this message.
        /// </summary>
        /// <remarks>
        ///     This property gets a read-only collection of attachments associated with this message. Depending on the
        ///     user's end-client, a sent message may contain one or more attachments. For example, mobile users may
        ///     attach more than one file in their message, while the desktop client only allows for one.
        /// </remarks>
        /// <returns>
        ///     A read-only collection of attachments.
        /// </returns>
        IReadOnlyCollection<IAttachment> Attachments { get; }
        /// <summary>
        ///     Gets all embeds included in this message.
        /// </summary>
        /// <remarks>
        /// </remarks>
        ///     This property gets a read-only collection of embeds associated with this message. Depending on the
        ///     message, a sent message may contain one or more embeds. This is usually true when multiple link previews
        ///     are generated; however, only one <see cref="EmbedType.Rich"/> <see cref="Embed"/> can be featured.
        /// <returns>
        ///     A read-only collection of embed objects.
        /// </returns>
        IReadOnlyCollection<IEmbed> Embeds { get; }
        /// <summary>
        ///     Gets all tags included in this message's content.
        /// </summary>
        IReadOnlyCollection<ITag> Tags { get; }
        /// <summary>
        ///     Gets the IDs of channels mentioned in this message.
        /// </summary>
        /// <returns>
        ///     A read-only collection of channel IDs.
        /// </returns>
        IReadOnlyCollection<ulong> MentionedChannelIds { get; }
        /// <summary>
        ///     Gets the IDs of roles mentioned in this message.
        /// </summary>
        /// <returns>
        ///     A read-only collection of role IDs.
        /// </returns>
        IReadOnlyCollection<ulong> MentionedRoleIds { get; }
        /// <summary>
        ///     Gets the IDs of users mentioned in this message.
        /// </summary>
        /// <returns>
        ///     A read-only collection of user IDs.
        /// </returns>
        IReadOnlyCollection<ulong> MentionedUserIds { get; }
        /// <summary>
        ///     Gets the activity associated with a message.
        /// </summary>
        /// <remarks>
        ///     Sent with Rich Presence-related chat embeds. This often refers to activity that requires end-user's
        ///     interaction, such as a Spotify Invite activity.
        /// </remarks>
        /// <returns>
        ///     A message's activity, if any is associated.
        /// </returns>
        MessageActivity Activity { get; }
        /// <summary>
        ///     Gets the application associated with a message.
        /// </summary>
        /// <remarks>
        ///     Sent with Rich-Presence-related chat embeds.
        /// </remarks>
        /// <returns>
        ///     A message's application, if any is associated.
        /// </returns>
        MessageApplication Application { get; }
    }
}
