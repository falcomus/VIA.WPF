// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CoreExtensionsTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using VIA.WPF.Extensions;

namespace VIA.WPF.Tests.Controls.Extensions;

#region ### Class CoreExtensionsTests ###
/// <summary>
/// Provides tests for non-visual extension helpers.
/// </summary>
public sealed class CoreExtensionsTests
{
    #region ### Private Fields ###
    private static readonly int[] ExpectedNumbers123 = [1, 2, 3];

    private static readonly int[] ExpectedNumbers45 = [4, 5];

    private static readonly int[] ExpectedNumbers57 = [5, 7];

    private static readonly int[] ExpectedNumbers75 = [7, 5];

    private static readonly int[] Numbers23 = [2, 3];

    private static readonly int[] Numbers102030 = [10, 20, 30];

    private static readonly int[] Numbers4567 = [4, 5, 6, 7];

    private static readonly int[] SingleNumber = [1];

    private static readonly string[] ExpectedAncestorNames = ["GrandChild", "Child", "Root"];

    private static readonly string[] ExpectedBreadthFirstNames = ["Root", "A", "B", "A1", "A2", "B1"];

    private static readonly string[] ExpectedDepthFirstNames = ["Root", "A", "A1", "A2", "B", "B1"];

    private static readonly string[] ExpectedNonNullValues = ["A", "B"];
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Ensures that string helper methods handle null, whitespace and case-insensitive comparisons.
    /// </summary>
    [Fact]
    public void StringExtensions_ShouldHandleCommonStringCases()
    {
        string? nullText = null;

        Assert.True(nullText.IsNullOrWhiteSpace());
        Assert.True("   ".IsNullOrWhiteSpace());
        Assert.False("Text".IsNullOrWhiteSpace());
        Assert.Null("   ".NullIfWhiteSpace());
        Assert.Equal("Value", "  Value  ".NullIfWhiteSpace());
        Assert.True("Alpha".EqualsIgnoreCase("alpha"));
        Assert.True("Alpha Beta".ContainsIgnoreCase("BETA"));
        Assert.True("Alpha Beta".StartsWithIgnoreCase("alpha"));
        Assert.False("Alpha".ContainsIgnoreCase(null));
        Assert.False(nullText.StartsWithIgnoreCase("alpha"));
    }

    /// <summary>
    /// Ensures that search text normalization trims, lower-cases, removes diacritics and collapses whitespace.
    /// </summary>
    [Fact]
    public void StringExtensions_NormalizeSearchText_ShouldNormalizeSearchInput()
    {
        Assert.Equal(string.Empty, ((string?)null).NormalizeSearchText());
        Assert.Equal(string.Empty, "   ".NormalizeSearchText());
        Assert.Equal("hello world", "  Héllo\t  Wörld  ".NormalizeSearchText());
    }

    /// <summary>
    /// Ensures that string truncation applies the requested ellipsis rules.
    /// </summary>
    [Fact]
    public void StringExtensions_LimitLength_ShouldRespectMaximumLengthAndEllipsis()
    {
        Assert.Equal(string.Empty, ((string?)null).LimitLength(5));
        Assert.Equal(string.Empty, "Text".LimitLength(0));
        Assert.Equal("Short", "Short".LimitLength(10));
        Assert.Equal("He…", "Hello".LimitLength(3));
        Assert.Equal("Hel", "Hello".LimitLength(3, string.Empty));
        Assert.Equal("Hel", "Hello".LimitLength(3, "----"));
    }

    /// <summary>
    /// Ensures that enumerable helpers handle null values, predicates and reference equality.
    /// </summary>
    [Fact]
    public void EnumerableExtensions_ShouldHandleCommonEnumerableOperations()
    {
        IEnumerable<int>? nullNumbers = null;
        List<string?> nullableValues = ["A", null, "B"];
        List<int> forEachValues = [];
        object first = new();
        object second = new();
        object equalButDifferent = new();
        List<object> references = [first, second];

        Assert.Empty(nullNumbers.EmptyIfNull());
        ExpectedNumbers123.ForEach(forEachValues.Add);
        Assert.Equal(ExpectedNumbers123, forEachValues);
        Assert.Equal(ExpectedNonNullValues, nullableValues.WhereNotNull());
        Assert.Equal(1, Numbers102030.IndexOf(value => value == 20));
        Assert.Equal(-1, Numbers102030.IndexOf(value => value == 99));
        Assert.True(references.ContainsReference(second));
        Assert.False(references.ContainsReference(equalButDifferent));

        ObservableCollection<int> collection = ExpectedNumbers45.ToObservableCollection();
        Assert.Equal(ExpectedNumbers45, collection);
    }

    /// <summary>
    /// Ensures that enumerable helpers reject null arguments where required.
    /// </summary>
    [Fact]
    public void EnumerableExtensions_ShouldRejectNullArguments()
    {
        IEnumerable<int>? numbers = null;
        IEnumerable<string?>? textValues = null;
        IEnumerable<object>? objects = null;

        Assert.Throws<ArgumentNullException>(() => numbers!.ForEach(_ => { }));
        Assert.Throws<ArgumentNullException>(() => SingleNumber.ForEach<int>(null!));
        Assert.Throws<ArgumentNullException>(() => textValues!.WhereNotNull().ToList());
        Assert.Throws<ArgumentNullException>(() => numbers!.IndexOf(_ => true));
        Assert.Throws<ArgumentNullException>(() => SingleNumber.IndexOf(null!));
        Assert.Throws<ArgumentNullException>(() => objects!.ContainsReference(new object()));
        Assert.Throws<ArgumentNullException>(() => numbers!.ToObservableCollection());
    }

    /// <summary>
    /// Ensures that observable collection helpers add, replace, remove and move items correctly.
    /// </summary>
    [Fact]
    public void ObservableCollectionExtensions_ShouldMutateCollectionCorrectly()
    {
        ObservableCollection<int> collection = [1];

        collection.AddRange(Numbers23);
        Assert.Equal(ExpectedNumbers123, collection);

        collection.ReplaceWith(Numbers4567);
        Assert.Equal(Numbers4567, collection);

        int removedCount = collection.RemoveWhere(value => value % 2 == 0);
        Assert.Equal(2, removedCount);
        Assert.Equal(ExpectedNumbers57, collection);

        Assert.True(collection.MoveItem(7, 0));
        Assert.Equal(ExpectedNumbers75, collection);
        Assert.True(collection.MoveItem(7, -10));
        Assert.Equal(ExpectedNumbers75, collection);
        Assert.True(collection.MoveItem(7, 99));
        Assert.Equal(ExpectedNumbers57, collection);
        Assert.False(collection.MoveItem(42, 0));
    }

    /// <summary>
    /// Ensures that observable collection helpers reject null arguments where required.
    /// </summary>
    [Fact]
    public void ObservableCollectionExtensions_ShouldRejectNullArguments()
    {
        ObservableCollection<int>? collection = null;
        ObservableCollection<int> validCollection = [];

        Assert.Throws<ArgumentNullException>(() => collection!.AddRange(SingleNumber));
        Assert.Throws<ArgumentNullException>(() => validCollection.AddRange(null!));
        Assert.Throws<ArgumentNullException>(() => collection!.ReplaceWith(SingleNumber));
        Assert.Throws<ArgumentNullException>(() => validCollection.ReplaceWith(null!));
        Assert.Throws<ArgumentNullException>(() => collection!.RemoveWhere(_ => true));
        Assert.Throws<ArgumentNullException>(() => validCollection.RemoveWhere(null!));
        Assert.Throws<ArgumentNullException>(() => collection!.MoveItem(1, 0));
    }

    /// <summary>
    /// Ensures that tree traversal helpers use the expected traversal order and reference semantics.
    /// </summary>
    [Fact]
    public void TreeExtensions_ShouldTraverseAndFindNodes()
    {
        TestNode root = TestNode.Create(
            "Root",
            TestNode.Create("A", TestNode.Create("A1"), TestNode.Create("A2")),
            TestNode.Create("B", TestNode.Create("B1")));
        TestNode[] roots = [root];
        TestNode target = root.Children[0].Children[1];
        TestNode equalButDifferent = new("A2");

        Assert.Equal(ExpectedDepthFirstNames, roots.TraverseDepthFirst(node => node.Children).Select(node => node.Name));
        Assert.Equal(ExpectedBreadthFirstNames, roots.TraverseBreadthFirst(node => node.Children).Select(node => node.Name));
        Assert.Same(target, roots.FindInTree(node => node.Children, node => node.Name == "A2"));
        Assert.True(roots.ContainsReferenceInTree(node => node.Children, target));
        Assert.False(roots.ContainsReferenceInTree(node => node.Children, equalButDifferent));
    }

    /// <summary>
    /// Ensures that tree ancestor enumeration starts with the current node and walks upwards.
    /// </summary>
    [Fact]
    public void TreeExtensions_SelfAndAncestors_ShouldReturnNodeAndParents()
    {
        TestNode root = new("Root");
        TestNode child = new("Child", root);
        TestNode grandChild = new("GrandChild", child);

        Assert.Equal(ExpectedAncestorNames, grandChild.SelfAndAncestors(node => node.Parent).Select(node => node.Name));
        Assert.Empty(((TestNode?)null).SelfAndAncestors(node => node.Parent));
    }

    /// <summary>
    /// Ensures that tree helpers reject null delegates where required.
    /// </summary>
    [Fact]
    public void TreeExtensions_ShouldRejectNullDelegates()
    {
        TestNode[] roots = [new("Root")];

        Assert.Throws<ArgumentNullException>(() => roots.TraverseDepthFirst(null!).ToList());
        Assert.Throws<ArgumentNullException>(() => roots.TraverseBreadthFirst(null!).ToList());
        Assert.Throws<ArgumentNullException>(() => roots.FindInTree(null!, _ => true));
        Assert.Throws<ArgumentNullException>(() => roots.FindInTree(node => node.Children, null!));
        Assert.Throws<ArgumentNullException>(() => roots.ContainsReferenceInTree(null!, roots[0]));
        Assert.Throws<ArgumentNullException>(() => roots[0].SelfAndAncestors(null!).ToList());
    }

    /// <summary>
    /// Ensures that enum display helpers respect display and description attributes.
    /// </summary>
    [Fact]
    public void EnumExtensions_ShouldResolveDisplayNamesAndDescriptions()
    {
        Assert.Equal("Display Name", TestEnum.Displayed.GetDisplayName());
        Assert.Equal("Display Description", TestEnum.Displayed.GetDescription());
        Assert.Equal("Description Text", TestEnum.Described.GetDisplayName());
        Assert.Equal("Description Text", TestEnum.Described.GetDescription());
        Assert.Equal("Plain", TestEnum.Plain.GetDisplayName());
        Assert.Equal("Plain", TestEnum.Plain.GetDescription());
        Assert.Equal(TestEnum.Displayed, "displayed".ToEnumOrDefault(TestEnum.Plain));
        Assert.Equal(TestEnum.Plain, "displayed".ToEnumOrDefault(TestEnum.Plain, ignoreCase: false));
        Assert.Equal(TestEnum.Described, "missing".ToEnumOrDefault(TestEnum.Described));
        Assert.Contains(TestEnum.Displayed, EnumExtensions.GetValues<TestEnum>());
    }

    /// <summary>
    /// Ensures that command helpers execute only executable commands.
    /// </summary>
    [Fact]
    public void CommandExtensions_ShouldExecuteOnlyWhenCommandCanExecute()
    {
        TestCommand executableCommand = new(canExecute: true);
        TestCommand blockedCommand = new(canExecute: false);

        Assert.True(executableCommand.CanExecuteSafe("Parameter"));
        Assert.True(executableCommand.ExecuteIfCan("Parameter"));
        Assert.Equal(1, executableCommand.ExecuteCount);
        Assert.Equal("Parameter", executableCommand.LastParameter);

        Assert.False(blockedCommand.CanExecuteSafe("Parameter"));
        Assert.False(blockedCommand.ExecuteIfCan("Parameter"));
        Assert.Equal(0, blockedCommand.ExecuteCount);

        ICommand? nullCommand = null;
        Assert.False(nullCommand.CanExecuteSafe());
        Assert.False(nullCommand.ExecuteIfCan());
    }

    /// <summary>
    /// Ensures that task helper observes exceptions and validates null tasks.
    /// </summary>
    [Fact]
    public async Task TaskExtensions_Forget_ShouldForwardExceptionsToHandler()
    {
        Task? nullTask = null;
        InvalidOperationException expectedException = new("Boom");
        TaskCompletionSource<Exception> observedExceptionSource = new();

        Assert.Throws<ArgumentNullException>(() => nullTask!.Forget());

        Task.FromException(expectedException).Forget(exception => observedExceptionSource.TrySetResult(exception));

        Task completedTask = await Task.WhenAny(observedExceptionSource.Task, Task.Delay(TimeSpan.FromSeconds(2)));
        Assert.Same(observedExceptionSource.Task, completedTask);
        Assert.Same(expectedException, await observedExceptionSource.Task);

        Task.CompletedTask.Forget();
    }
    #endregion

    #region ### Test Types ###
    private enum TestEnum
    {
        [Display(Name = "Display Name", Description = "Display Description")]
        Displayed,

        [Description("Description Text")]
        Described,

        Plain
    }

    private sealed class TestNode(string name, TestNode? parent = null)
    {
        public string Name { get; } = name;

        public TestNode? Parent => this.ParentBacking ?? parent;

        public List<TestNode> Children { get; } = [];

        public static TestNode Create(string name, params TestNode[] children)
        {
            TestNode node = new(name);

            foreach (TestNode child in children)
            {
                child.ParentBacking = node;
                node.Children.Add(child);
            }

            return node;
        }

        private TestNode? ParentBacking { get; set; }
    }

    private sealed class TestCommand(bool canExecute) : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add
            {
            }

            remove
            {
            }
        }

        public int ExecuteCount { get; private set; }

        public object? LastParameter { get; private set; }

        public bool CanExecute(object? parameter)
        {
            return canExecute;
        }

        public void Execute(object? parameter)
        {
            this.ExecuteCount++;
            this.LastParameter = parameter;
        }
    }
    #endregion
}
#endregion