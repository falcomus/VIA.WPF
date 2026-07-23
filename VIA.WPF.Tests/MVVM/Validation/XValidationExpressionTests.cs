// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationExpressionTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.MVVM;

namespace VIA.WPF.Tests.MVVM.Validation;

#region ### Class XValidationExpressionTests ###
/// <summary>
/// Tests expression-based validation property helpers.
/// </summary>
public sealed class XValidationExpressionTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies simple property name extraction.
    /// </summary>
    [Fact]
    public void GetPropertyName_ShouldReturnSimplePropertyName()
    {
        string propertyName = XValidationExpression.GetPropertyName<TestModel, string?>(model => model.Name);

        Assert.Equal(nameof(TestModel.Name), propertyName);
    }

    /// <summary>
    /// Verifies nested property path extraction.
    /// </summary>
    [Fact]
    public void GetPropertyName_ShouldReturnNestedPropertyPath()
    {
        string propertyName = XValidationExpression.GetPropertyName<TestModel, string?>(model => model.Address.City);

        Assert.Equal("Address.City", propertyName);
    }

    /// <summary>
    /// Verifies conversion expression handling.
    /// </summary>
    [Fact]
    public void GetPropertyName_ShouldIgnoreConvertExpressions()
    {
        string propertyName = XValidationExpression.GetPropertyName<TestModel, object?>(model => model.Age);

        Assert.Equal(nameof(TestModel.Age), propertyName);
    }

    /// <summary>
    /// Verifies context expression validation helpers.
    /// </summary>
    [Fact]
    public void ExpressionContextHelpers_ShouldUseExpressionPropertyName()
    {
        TestModel model = new()
        {
            Name = "",
            Email = "invalid"
        };

        XValidationContext context = new(model);

        context.Required(model, item => item.Name, XValidationText.Text("Name required."));
        context.Email(model, item => item.Email, XValidationText.Text("Email invalid."));

        Assert.Equal(2, context.Messages.Count);
        Assert.Contains(context.Messages, message => message.PropertyNames.Single() == nameof(TestModel.Name));
        Assert.Contains(context.Messages, message => message.PropertyNames.Single() == nameof(TestModel.Email));
    }
    #endregion

    #region ### Private Classes ###
    private sealed class TestModel
    {
        #region ### Public Properties ###
        public AddressModel Address { get; } = new();

        public int Age { get; set; }

        public string? Email { get; set; }

        public string? Name { get; set; }
        #endregion
    }

    private sealed class AddressModel
    {
        #region ### Public Properties ###
        public string? City { get; set; }
        #endregion
    }
    #endregion
}
#endregion
