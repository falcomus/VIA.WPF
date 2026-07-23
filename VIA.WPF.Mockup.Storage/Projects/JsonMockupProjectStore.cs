// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonMockupProjectStore.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;
using VIA.WPF.Mockup.Core.Model;

namespace VIA.WPF.Mockup.Storage.Projects;

/// <summary>
/// Stores mockup projects as indented JSON files.
/// </summary>
public sealed class JsonMockupProjectStore : IMockupProjectStore
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    public async Task<MockupProject> LoadAsync(string filePath, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        await using FileStream stream = File.OpenRead(filePath);
        MockupProject? project = await JsonSerializer.DeserializeAsync<MockupProject>(stream, SerializerOptions, cancellationToken);
        return project ?? throw new InvalidDataException($"The mockup project '{filePath}' is empty or invalid.");
    }

    public async Task SaveAsync(MockupProject project, string filePath, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(project);
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        string? directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using FileStream stream = File.Create(filePath);
        await JsonSerializer.SerializeAsync(stream, project, SerializerOptions, cancellationToken);
    }
}
