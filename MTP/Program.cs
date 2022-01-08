using MediaDevices;

public class Program
{
    static void Main(string[] args)
    {
        var devices = MediaDevice.GetDevices();
        //using (var device = devices.First(d => d.FriendlyName == "ZTE Blade A522"))
        //var device = devices.First();


        int nomerDevices = 1;
        string nameDevice;
        string[] nameDevices = new string[devices.Count()];
        //var nameDevices = Array.Empty<string>();
        Console.WriteLine("Подключены следующие устройства");
        foreach (var device2 in devices)
        {
            //int i = 0;
            nameDevices[nomerDevices - 1] = device2.FriendlyName;
            Console.WriteLine($"Номер устройства - {nomerDevices}, Название - {device2.FriendlyName}");
            nomerDevices++;
            //i++;
        }
        Console.WriteLine("Введите номер устроства");
        nomerDevices = Convert.ToInt32(Console.ReadLine());
        if(nomerDevices == 0) { nomerDevices = 42; }
        if (nomerDevices > devices.Count() || nomerDevices == 42)
        {
            Console.WriteLine("Номер устройства указан не верно");
            Console.Read();

        } 
        else 
        {
            nameDevice = nameDevices[nomerDevices - 1];
            var device = devices.First(d => d.FriendlyName == $"{nameDevice}");

            int nomerMemory;
            string memoryPhone;
            string[] memoryPhones = new string[2];
            int count = 0;
            device.Connect();
            var memorys = device.GetDirectoryInfo(@"\");
            var memory = memorys.EnumerateDirectories("*", SearchOption.TopDirectoryOnly);
            Console.WriteLine("Устройство имеет такую память");
            foreach (var memoryP in memory)
            {
                memoryPhones[count] = memoryP.Name;                
                Console.WriteLine($"Память номер - {count+1}, Название - {memoryP.Name}");
                count++;
            }
            Console.WriteLine("Введите номер памяти");
            nomerMemory = Convert.ToInt32(Console.ReadLine());
            if(nomerMemory == 0 || nomerMemory>memory.Count())
            {
                nomerMemory = 1;
            }
            memoryPhone = memoryPhones[nomerMemory - 1];
            device.Disconnect();
            {
                device.Connect();
                var photoDir = device.GetDirectoryInfo($@"\{memoryPhone}\Android\data\org.audioknigi.app\files\downloads\");
                var folders = photoDir.EnumerateDirectories("*", SearchOption.TopDirectoryOnly);

                foreach (var folder in folders)
                {
                    Directory.CreateDirectory(@"D:\BOOK\" + folder.Name);
                    var photoSubDir = device.GetDirectoryInfo($@"\{memoryPhone}\Android\data\org.audioknigi.app\files\downloads\" + folder.Name);
                    var files = photoSubDir.EnumerateFiles("*.*", SearchOption.TopDirectoryOnly);
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
                Console.WriteLine("Все найденые файлы перемещены");
                Console.Read();
                device.Disconnect();
            }
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

    static void ConsoleCMD(string Stroka)
    {
        System.Diagnostics.Process.Start("CMD.exe", "/C" + Stroka);
    }

}