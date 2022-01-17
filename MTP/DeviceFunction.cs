using MediaDevices;

namespace MTP
{
    class DeviceFunction
    {
        public string getNameDevices()
        {
            var devices = MediaDevice.GetDevices();
            int nomerDevices = 1;
            string nameDevice;
            string[] nameDevices = new string[devices.Count()];
            //var nameDevices = Array.Empty<string>();
            Console.WriteLine("Подключены следующие устройства");
            foreach (var device2 in devices)
            {
                nameDevices[nomerDevices - 1] = device2.FriendlyName;
                Console.WriteLine($"Номер устройства - {nomerDevices}, Название - {device2.FriendlyName}");
                nomerDevices++;
            }
        LABLE1:
            Console.WriteLine("Введите номер устроства");
            nomerDevices = Convert.ToInt32(Console.ReadLine());
            if (nomerDevices == 0) { nomerDevices = 42; }
            if (nomerDevices > devices.Count() || nomerDevices == 42)
            {
                Console.WriteLine("Номер устройства указан не верно");
                goto LABLE1;
            }
            nameDevice = nameDevices[nomerDevices - 1];
            return nameDevice;
        }
        public string getNameMemory(string nameDevice)
        {
            var devices = MediaDevice.GetDevices();
            var device = devices.First(d => d.FriendlyName == $"{nameDevice}");
            device.Connect();
            int nomerMemory;
            string memoryPhone;
            string[] memoryPhones = new string[2];
            int count = 0;
            var memorys = device.GetDirectoryInfo(@"\");
            var memory = memorys.EnumerateDirectories("*", SearchOption.TopDirectoryOnly);
            Console.WriteLine("Устройство имеет такую память");
            foreach (var memoryP in memory)
            {
                memoryPhones[count] = memoryP.Name;
                Console.WriteLine($"Память номер - {count + 1}, Название - {memoryP.Name}");
                count++;
            }

            Console.WriteLine("Введите номер памяти");
            nomerMemory = Convert.ToInt32(Console.ReadLine());
            if (nomerMemory == 0 || nomerMemory > memory.Count())
            {
                nomerMemory = 1;
            }
            memoryPhone = memoryPhones[nomerMemory - 1];
            device.Disconnect();
            return memoryPhone;
        }

    }
}
