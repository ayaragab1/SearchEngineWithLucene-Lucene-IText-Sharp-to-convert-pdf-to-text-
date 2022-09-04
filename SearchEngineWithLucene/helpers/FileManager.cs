namespace SearchEngineWithLucene.helpers;

public class FileManager
{
    public static async Task<string> Upload(IFormFile file, string folder, bool compress)
    {

        try
        {
            var name = Guid.NewGuid() + "." + file.FileName.Split('.').Last();
            var path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{folder}", name);
            await using var bits = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(bits);
            bits.Close();
            if (compress)
            {
                var snakewareLogo = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{folder}", name));
                snakewareLogo.Refresh();
            }
            return name;
        }
        catch
        {
            return null;
        }
    }

    public static bool Exist(string file, string folder)
    {
        if (!string.IsNullOrWhiteSpace(file))
        {
            var pathOrg = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{folder}", file);
            var result = File.Exists(pathOrg);
            return result;
        }

        return false;
    }


    public static void Delete(string file, string folder)
    {
        File.Delete(Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{folder}", file));
    }
}
