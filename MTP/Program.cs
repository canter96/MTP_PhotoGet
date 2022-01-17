using MediaDevices;
using MTP;

public class Program
{
    static void Main(string[] args)
    {
        var devices = MediaDevice.GetDevices();

        DeviceFunction devicesFunction = new DeviceFunction();
        string nameDevice = devicesFunction.getNameDevices();
        var device = devices.First(d => d.FriendlyName == $"{nameDevice}");
        string memoryPhone = devicesFunction.getNameMemory(nameDevice);


        device.Connect();
        var bookDir = device.GetDirectoryInfo($@"\{memoryPhone}\Android\data\org.audioknigi.app\files\downloads\");
        var folders = bookDir.EnumerateDirectories("*", SearchOption.TopDirectoryOnly);

        foreach (var folder in folders)
        {
            Directory.CreateDirectory(@"D:\BOOK\" + folder.Name);
            var bookSubDir = device.GetDirectoryInfo($@"\{memoryPhone}\Android\data\org.audioknigi.app\files\downloads\" + folder.Name);
            var files = bookSubDir.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                MemoryStream memoryStream = new System.IO.MemoryStream();
                device.DownloadFile(file.FullName, memoryStream);
                memoryStream.Position = 0;
                WriteSreamToDisk($@"D:\BOOK\{folder.Name}\{file.Name}", memoryStream);
                device.DeleteFile(file.FullName);
                device.DeleteDirectory(folder.FullName);
                string fileName = Path.GetFileNameWithoutExtension(file.Name);
                File.Move($@"D:\BOOK\{folder.Name}\{file.Name}", $@"D:\BOOK\{folder.Name}\{fileName}.mp3");
                Console.WriteLine($"Файл {fileName}.mp3 перемещен");
            }

        }
        device.Disconnect();
        Console.WriteLine("Все найденые файлы перемещены");
        Console.Read();

    }

    static void WriteSreamToDisk(string filePath, MemoryStream memoryStream)
    {
        using (FileStream file = new FileStream(filePath, FileMode.Create, System.IO.FileAccess.Write))
        {
            byte[] bytes = new byte[memoryStream.Length];
            memoryStream.Read(bytes, 0, (int)memoryStream.Length);
            file.Write(bytes, 0, bytes.Length);
            memoryStream.Close();
        }
    }

}