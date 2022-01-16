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
        var files = bookDir.EnumerateFiles("*.exo", SearchOption.AllDirectories);
        var folders = bookDir.EnumerateDirectories("*", SearchOption.AllDirectories);
        int count = 1;
        foreach (var file in files)
        {
            Directory.CreateDirectory(@"D:\BOOK\");         
            MemoryStream memoryStream = new System.IO.MemoryStream();
            device.DownloadFile(file.FullName, memoryStream);
            memoryStream.Position = 0;
            WriteSreamToDisk($@"D:\BOOK\{file.Name}", memoryStream);
            device.DeleteFile(file.FullName);
            //string fileName = Path.GetFileNameWithoutExtension(file.Name);
            string nomerFile = string.Format("{0:00000}", count);
            File.Move($@"D:\BOOK\{file.Name}", $@"D:\BOOK\{nomerFile}.mp3");
            Console.WriteLine($"Файл {nomerFile}.mp3 перемещен");
            count++;
        }
        foreach (var folder in folders)
        {
            device.DeleteDirectory(folder.FullName);
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