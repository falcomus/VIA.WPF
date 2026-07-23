// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationRuleSet.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.MVVM;

#region ### Class XValidationRuleSet ###
/// <summary>
/// Represents an immutable set of validation rules for a model type.
/// </summary>
/// <typeparam name="TModel">The model type.</typeparam>
public sealed class XValidationRuleSet<TModel>
    where TModel : notnull
{
    #region ### Fields ###
    private readonly IReadOnlyList<Func<TModel, XValidationContext, CancellationToken, Task>> rules;
    #endregion

    #region ### Constructors ###
    internal XValidationRuleSet(IEnumerable<Func<TModel, XValidationContext, CancellationToken, Task>> rules)
    {
        this.rules = rules.ToArray();
    }
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Executes this rule set and returns a validation result.
    /// </summary>
    /// <param name="model">The model to validate.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The validation result.</returns>
    public async Task<XValidationResult> ValidateAsync(TModel model, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);

        XValidationContext context = new(model);
        await this.ValidateAsync(model, context, cancellationToken).ConfigureAwait(false);
        return XValidationResult.FromMessages(context.Messages);
    }

    /// <summary>
    /// Executes this rule set against an existing validation context.
    /// </summary>
    /// <param name="model">The model to validate.</param>
    /// <param name="context">The validation context.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ValidateAsync(TModel model, XValidationContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(context);

        foreach (Func<TModel, XValidationContext, CancellationToken, Task> rule in this.rules)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await rule(model, context, cancellationToken).ConfigureAwait(false);
        }
    }
    #endregion
}
#endregion
