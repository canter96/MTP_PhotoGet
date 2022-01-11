using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Console.WriteLine("Podklyucheny sleduyushchiye ustroystva");
            foreach (var device2 in devices)
            {
                //int i = 0;
                nameDevices[nomerDevices - 1] = device2.FriendlyName;
                Console.WriteLine($"Nomer ustroystva - {nomerDevices}, Nazvaniye - {device2.FriendlyName}");
                nomerDevices++;
                //i++;
            }
            LABLE1:
            Console.WriteLine("Vvedite nomer ustroystva");
            nomerDevices = Convert.ToInt32(Console.ReadLine());
            if (nomerDevices == 0) { nomerDevices = 42; }
            if (nomerDevices > devices.Count() || nomerDevices == 42)
            {
                Console.WriteLine("Nomer ustroystva ukazan ne verno");
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
            Console.WriteLine("Ustroystvo imeyet takuyu pamyat");
            foreach (var memoryP in memory)
            {
                memoryPhones[count] = memoryP.Name;
                Console.WriteLine($"Pamyat nomer - {count + 1}, Nazvaniye - {memoryP.Name}");
                count++;
            }
            
            Console.WriteLine("Vvedite nomer pamyati");
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
