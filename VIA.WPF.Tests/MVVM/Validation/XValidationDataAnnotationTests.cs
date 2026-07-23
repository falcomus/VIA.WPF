// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationDataAnnotationTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using VIA.WPF.MVVM;

namespace VIA.WPF.Tests.MVVM.Validation;

#region ### Class XValidationDataAnnotationTests ###
/// <summary>
/// Tests DataAnnotations integration.
/// </summary>
public sealed class XValidationDataAnnotationTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies object-level DataAnnotations validation.
    /// </summary>
    [Fact]
    public void ValidateDataAnnotations_ShouldAddErrorsForInvalidModel()
    {
        AnnotatedModel model = new()
        {
            Name = "",
            Code = "123456"
        };

        XValidationContext context = new(model);

        context.ValidateDataAnnotations(model);

        Assert.Equal(2, context.Messages.Count);
        Assert.Contains(context.Messages, message => message.PropertyNames.Single() == nameof(AnnotatedModel.Name));
        Assert.Contains(context.Messages, message => message.PropertyNames.Single() == nameof(AnnotatedModel.Code));
    }

    /// <summary>
    /// Verifies context-source DataAnnotations validation.
    /// </summary>
    [Fact]
    public void ValidateDataAnnotations_ShouldUseContextSource()
    {
        AnnotatedModel model = new()
        {
            Name = "",
            Code = "ABC"
        };

        XValidationContext context = new(model);

        context.ValidateDataAnnotations();

        XValidationError error = Assert.Single(context.Messages);
        Assert.Equal(nameof(AnnotatedModel.Name), error.PropertyNames.Single());
    }

    /// <summary>
    /// Verifies property-level DataAnnotations validation.
    /// </summary>
    [Fact]
    public void ValidateDataAnnotationsProperty_ShouldAddSinglePropertyErrors()
    {
        AnnotatedModel model = new()
        {
            Name = "Valid",
            Code = "123456"
        };

        XValidationContext context = new(model);

        context.ValidateDataAnnotationsProperty(model, item => item.Code);

        XValidationError error = Assert.Single(context.Messages);
        Assert.Equal(nameof(AnnotatedModel.Code), error.PropertyNames.Single());
    }
    #endregion

    #region ### Private Classes ###
    private sealed class AnnotatedModel
    {
        #region ### Public Properties ###
        [StringLength(4)]
        public string? Code { get; set; }

        [Required]
        public string? Name { get; set; }
        #endregion
    }
    #endregion
}
#endregion
