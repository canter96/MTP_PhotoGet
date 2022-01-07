public class Program
{
    static void Main(string[] args)
    {
        var devices = MediaDevice.GetDevices();
        using (var device = devices.First(d => d.FriendlyName == "Galaxy Note8"))
        {
            device.Connect();
            var photoDir = device.GetDirectoryInfo(@"\Phone\DCIM\Camera");

            var files = photoDir.EnumerateFiles("*.*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                MemoryStream memoryStream = new System.IO.MemoryStream();
                device.DownloadFile(file.FullName, memoryStream);
                memoryStream.Position = 0;
                WriteSreamToDisk($@"D:\PHOTOS\{file.Name}", memoryStream);
            }
            device.Disconnect();
        }

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