// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationBuilderTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using VIA.WPF.MVVM;

namespace VIA.WPF.Tests.MVVM.Validation;

#region ### Class XValidationBuilderTests ###
/// <summary>
/// Tests fluent validation builders.
/// </summary>
public sealed class XValidationBuilderTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies fluent rule execution.
    /// </summary>
    [Fact]
    public async Task ValidateAsync_ShouldExecuteConfiguredRules()
    {
        XValidationRuleSet<TestModel> ruleSet = XValidationBuilder
            .For<TestModel>()
            .Required(model => model.Name, XValidationText.Text("Name required."))
            .MaxLength(model => model.Description, 5, XValidationText.Text("Description too long."))
            .Range(model => model.Age, 0, 120, XValidationText.Text("Age out of range."))
            .Build();

        TestModel model = new()
        {
            Name = "",
            Description = "Too long",
            Age = 200
        };

        XValidationResult result = await ruleSet.ValidateAsync(model);

        Assert.False(result.IsValid);
        Assert.Equal(3, result.Errors.Count);
        Assert.Contains(result.Errors, error => error.PropertyNames.Single() == nameof(TestModel.Name));
        Assert.Contains(result.Errors, error => error.PropertyNames.Single() == nameof(TestModel.Description));
        Assert.Contains(result.Errors, error => error.PropertyNames.Single() == nameof(TestModel.Age));
    }

    /// <summary>
    /// Verifies conditional and custom rules.
    /// </summary>
    [Fact]
    public async Task ValidateAsync_ShouldExecuteConditionalAndCustomRules()
    {
        XValidationRuleSet<TestModel> ruleSet = XValidationBuilder
            .For<TestModel>()
            .RequiredIf(model => model.RequiresEmail, model => model.Email, XValidationText.Text("Email required."))
            .Rule((model, context) => context.WarningIf(model.Age < 18, XValidationText.Text("Minor."), nameof(TestModel.Age)))
            .Build();

        TestModel model = new()
        {
            RequiresEmail = true,
            Email = "",
            Age = 17
        };

        XValidationResult result = await ruleSet.ValidateAsync(model);

        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Single(result.Warnings);
    }

    /// <summary>
    /// Verifies DataAnnotations rule integration.
    /// </summary>
    [Fact]
    public async Task ValidateAsync_ShouldExecuteDataAnnotationsRule()
    {
        XValidationRuleSet<AnnotatedModel> ruleSet = XValidationBuilder
            .For<AnnotatedModel>()
            .DataAnnotations()
            .Build();

        AnnotatedModel model = new()
        {
            Name = null
        };

        XValidationResult result = await ruleSet.ValidateAsync(model);

        Assert.False(result.IsValid);
        XValidationError error = Assert.Single(result.Errors);
        Assert.Equal(nameof(AnnotatedModel.Name), error.PropertyNames.Single());
    }

    /// <summary>
    /// Verifies comparison rules for non-nullable value properties.
    /// </summary>
    [Fact]
    public async Task ValidateAsync_ShouldSupportNonNullableComparisonRules()
    {
        XValidationRuleSet<TestModel> ruleSet = XValidationBuilder
            .For<TestModel>()
            .GreaterThan(model => model.Age, 18, XValidationText.Text("Age must be greater than 18."))
            .GreaterThanOrEqual(model => model.Age, 20, XValidationText.Text("Age must be at least 20."))
            .LessThan(model => model.Age, 18, XValidationText.Text("Age must be less than 18."))
            .LessThanOrEqual(model => model.Age, 17, XValidationText.Text("Age must be at most 17."))
            .Build();

        XValidationResult result = await ruleSet.ValidateAsync(new TestModel { Age = 18 });

        Assert.False(result.IsValid);
        Assert.Equal(4, result.Errors.Count);
        Assert.All(result.Errors, error => Assert.Equal(nameof(TestModel.Age), error.PropertyNames.Single()));
    }

    /// <summary>
    /// Verifies asynchronous rule integration.
    /// </summary>
    [Fact]
    public async Task ValidateAsync_ShouldExecuteAsyncRule()
    {
        XValidationRuleSet<TestModel> ruleSet = XValidationBuilder
            .For<TestModel>()
            .MustBeTrueAsync(
                (model, _) => Task.FromResult(model.Name == "Allowed"),
                XValidationText.Text("Name is not allowed."),
                model => model.Name)
            .Build();

        XValidationResult result = await ruleSet.ValidateAsync(new TestModel { Name = "Denied" });

        Assert.False(result.IsValid);
        Assert.Equal(nameof(TestModel.Name), Assert.Single(result.Errors).PropertyNames.Single());
    }
    #endregion

    #region ### Private Classes ###
    private sealed class AnnotatedModel
    {
        #region ### Public Properties ###
        [Required]
        public string? Name { get; set; }
        #endregion
    }

    private sealed class TestModel
    {
        #region ### Public Properties ###
        public int Age { get; set; }

        public string? Description { get; set; }

        public string? Email { get; set; }

        public string? Name { get; set; }

        public bool RequiresEmail { get; set; }
        #endregion
    }
    #endregion
}
#endregion
