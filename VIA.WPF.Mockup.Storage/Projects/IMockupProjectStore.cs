// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMockupProjectStore.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.Mockup.Core.Model;

namespace VIA.WPF.Mockup.Storage.Projects;

/// <summary>
/// Defines persistent loading and saving of complete mockup projects.
/// </summary>
public interface IMockupProjectStore
{
    Task<MockupProject> LoadAsync(string filePath, CancellationToken cancellationToken = default);

    Task SaveAsync(MockupProject project, string filePath, CancellationToken cancellationToken = default);
}
