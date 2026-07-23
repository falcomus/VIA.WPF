// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationResult.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.MVVM;

#region ### Class XValidationResult ###
/// <summary>
/// Represents the complete result of a validation run.
/// </summary>
public sealed class XValidationResult
{
    #region ### Fields ###
    private static readonly XValidationResult SuccessfulResult = new([]);
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XValidationResult"/> class.
    /// </summary>
    /// <param name="messages">The validation messages.</param>
    public XValidationResult(IEnumerable<XValidationError> messages)
    {
        this.Messages = messages?.ToArray() ?? [];
        this.Errors = this.Messages.Where(message => message.Severity == XValidationSeverity.Error).ToArray();
        this.Warnings = this.Messages.Where(message => message.Severity == XValidationSeverity.Warning).ToArray();
        this.InformationMessages = this.Messages.Where(message => message.Severity == XValidationSeverity.Information).ToArray();
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets all validation messages.
    /// </summary>
    public IReadOnlyList<XValidationError> Messages { get; }

    /// <summary>
    /// Gets all validation errors.
    /// </summary>
    public IReadOnlyList<XValidationError> Errors { get; }

    /// <summary>
    /// Gets all validation warnings.
    /// </summary>
    public IReadOnlyList<XValidationError> Warnings { get; }

    /// <summary>
    /// Gets all informational validation messages.
    /// </summary>
    public IReadOnlyList<XValidationError> InformationMessages { get; }

    /// <summary>
    /// Gets a value indicating whether the validation result contains no errors.
    /// </summary>
    public bool IsValid => this.Errors.Count == 0;

    /// <summary>
    /// Gets a value indicating whether the validation result contains validation messages.
    /// </summary>
    public bool HasMessages => this.Messages.Count > 0;

    /// <summary>
    /// Gets a value indicating whether the validation result contains warnings.
    /// </summary>
    public bool HasWarnings => this.Warnings.Count > 0;

    /// <summary>
    /// Gets a value indicating whether the validation result contains informational messages.
    /// </summary>
    public bool HasInformation => this.InformationMessages.Count > 0;
    #endregion

    #region ### Public Methods ###

    /// <summary>
    /// Compares this result with another result by validation message content.
    /// </summary>
    /// <param name="other">The other validation result.</param>
    /// <returns><c>true</c> when both results contain equivalent messages in the same order; otherwise <c>false</c>.</returns>
    public bool HasSameMessagesAs(XValidationResult? other)
    {
        return other is not null && MessagesEqual(this.Messages, other.Messages);
    }

    /// <summary>
    /// Compares this result with a validation message sequence by validation message content.
    /// </summary>
    /// <param name="messages">The messages to compare with.</param>
    /// <returns><c>true</c> when both sequences contain equivalent messages in the same order; otherwise <c>false</c>.</returns>
    public bool HasSameMessagesAs(IEnumerable<XValidationError>? messages)
    {
        return MessagesEqual(this.Messages, messages);
    }

    /// <summary>
    /// Creates a successful validation result without messages.
    /// </summary>
    /// <returns>The validation result.</returns>
    public static XValidationResult Success()
    {
        return SuccessfulResult;
    }

    /// <summary>
    /// Compares two validation results by validation message content.
    /// </summary>
    /// <param name="first">The first validation result.</param>
    /// <param name="second">The second validation result.</param>
    /// <returns><c>true</c> when both results contain equivalent messages in the same order; otherwise <c>false</c>.</returns>
    public static bool MessagesEqual(XValidationResult? first, XValidationResult? second)
    {
        if (ReferenceEquals(first, second))
        {
            return true;
        }

        return first is not null
            && second is not null
            && MessagesEqual(first.Messages, second.Messages);
    }

    /// <summary>
    /// Compares two validation message sequences by validation message content.
    /// </summary>
    /// <param name="first">The first validation message sequence.</param>
    /// <param name="second">The second validation message sequence.</param>
    /// <returns><c>true</c> when both sequences contain equivalent messages in the same order; otherwise <c>false</c>.</returns>
    public static bool MessagesEqual(IEnumerable<XValidationError>? first, IEnumerable<XValidationError>? second)
    {
        if (ReferenceEquals(first, second))
        {
            return true;
        }

        if (first is null || second is null)
        {
            return false;
        }

        IReadOnlyList<XValidationError> firstMessages = first as IReadOnlyList<XValidationError> ?? first.ToArray();
        IReadOnlyList<XValidationError> secondMessages = second as IReadOnlyList<XValidationError> ?? second.ToArray();

        return XValidationHelpers.ValidationMessagesEqual(firstMessages, secondMessages);
    }

    /// <summary>
    /// Creates a validation result from existing messages.
    /// </summary>
    /// <param name="messages">The validation messages.</param>
    /// <returns>The validation result.</returns>
    public static XValidationResult FromMessages(IEnumerable<XValidationError> messages)
    {
        XValidationError[] messageArray = messages?.ToArray() ?? [];
        return messageArray.Length == 0
            ? Success()
            : new XValidationResult(messageArray);
    }

    /// <summary>
    /// Creates a validation result containing one error.
    /// </summary>
    /// <param name="text">The validation text.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <returns>The validation result.</returns>
    public static XValidationResult FromError(XValidationText text, params string[] propertyNames)
    {
        return FromMessage(text, XValidationSeverity.Error, propertyNames);
    }

    /// <summary>
    /// Creates a validation result containing one error using a resource key.
    /// </summary>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <returns>The validation result.</returns>
    public static XValidationResult FromError(string resourceKey, params string[] propertyNames)
    {
        return FromError(XValidationText.Key(resourceKey), propertyNames);
    }

    /// <summary>
    /// Creates a validation result containing one warning.
    /// </summary>
    /// <param name="text">The validation text.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <returns>The validation result.</returns>
    public static XValidationResult FromWarning(XValidationText text, params string[] propertyNames)
    {
        return FromMessage(text, XValidationSeverity.Warning, propertyNames);
    }

    /// <summary>
    /// Creates a validation result containing one warning using a resource key.
    /// </summary>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <returns>The validation result.</returns>
    public static XValidationResult FromWarning(string resourceKey, params string[] propertyNames)
    {
        return FromWarning(XValidationText.Key(resourceKey), propertyNames);
    }

    /// <summary>
    /// Creates a validation result containing one informational message.
    /// </summary>
    /// <param name="text">The validation text.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <returns>The validation result.</returns>
    public static XValidationResult FromInformation(XValidationText text, params string[] propertyNames)
    {
        return FromMessage(text, XValidationSeverity.Information, propertyNames);
    }

    /// <summary>
    /// Creates a validation result containing one informational message using a resource key.
    /// </summary>
    /// <param name="resourceKey">The localization resource key.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <returns>The validation result.</returns>
    public static XValidationResult FromInformation(string resourceKey, params string[] propertyNames)
    {
        return FromInformation(XValidationText.Key(resourceKey), propertyNames);
    }

    /// <summary>
    /// Creates a validation result containing one message.
    /// </summary>
    /// <param name="text">The validation text.</param>
    /// <param name="severity">The validation severity.</param>
    /// <param name="propertyNames">The affected property names.</param>
    /// <returns>The validation result.</returns>
    public static XValidationResult FromMessage(XValidationText text, XValidationSeverity severity, params string[] propertyNames)
    {
        return new XValidationResult([new XValidationError(text, severity, propertyNames)]);
    }
    #endregion
}
#endregion
