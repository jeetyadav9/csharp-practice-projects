using System.Text.Json;

namespace CSharpPracticeProjects.Core;

public sealed class JsonRepository<T>
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public JsonRepository(string fileName)
    {
        _filePath = AppPaths.DataFile(fileName);
    }

    public async Task<List<T>> LoadAsync()
    {
        if (!File.Exists(_filePath))
        {
            return new List<T>();
        }

        await using FileStream stream = File.OpenRead(_filePath);
        var items = await JsonSerializer.DeserializeAsync<List<T>>(stream, _options);
        return items ?? new List<T>();
    }

    public async Task SaveAsync(IEnumerable<T> items)
    {
        string? directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using FileStream stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, items, _options);
    }
}
