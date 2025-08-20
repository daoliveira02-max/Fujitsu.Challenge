using Fujitsu.Challenge.API.Interfaces;
using System.Text.Json;

namespace Fujitsu.Challenge.API.Repositories
{
    public class FileRepository<T> : IFileRepository<T>
    {
        private readonly string _filePath;

        public FileRepository(string fileName)
        {
            var dataDir = Path.Combine(AppContext.BaseDirectory, "Data");
            if (!Directory.Exists(dataDir))
                Directory.CreateDirectory(dataDir);

            _filePath = Path.Combine(dataDir, fileName);

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }

        public List<T> GetAll()
        {
            var json = File.ReadAllText(_filePath);
            
            if (json == string.Empty) 
                return new List<T>();

            return JsonSerializer.Deserialize<List<T>>(json);
        }

        public void SaveAll(List<T> items)
        {
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}
