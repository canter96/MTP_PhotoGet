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
        int count = 1;
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
                //WriteSreamToDisk($@"D:\BOOK\{folder.Name}\{file.Name}", memoryStream);
                WriteSreamToDisk($@"D:\BOOK\{file.Name}", memoryStream);
                device.DeleteFile(file.FullName);
                device.DeleteDirectory(folder.FullName);
                //string fileName = Path.GetFileNameWithoutExtension(file.Name);
                string nomerFile = string.Format("{0:0000}", count);
                File.Move($@"D:\BOOK\{file.Name}", $@"D:\BOOK\{nomerFile}.mp3");
                Console.WriteLine($"Fayl {nomerFile}.mp3 peremeshchen");
                count++;
            }

        }
        device.Disconnect();
        Console.WriteLine("Vse naydenyye fayly peremeshcheny");
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