namespace CSharpPracticeProjects.Core;

public static class AppPaths
{
    public static string DataDirectory
    {
        get
        {
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (string.IsNullOrWhiteSpace(basePath))
            {
                basePath = AppContext.BaseDirectory;
            }

            string path = Path.Combine(basePath, "CSharpPracticeProjects");
            Directory.CreateDirectory(path);
            return path;
        }
    }

    public static string DataFile(string fileName)
    {
        string safeName = fileName.Replace("..", string.Empty).Replace(Path.DirectorySeparatorChar, '_');
        return Path.Combine(DataDirectory, safeName);
    }
}
