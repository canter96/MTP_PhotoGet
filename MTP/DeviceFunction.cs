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
            Console.WriteLine("Подключены следующие устройства");
            foreach (var device2 in devices)
            {
                //int i = 0;
                nameDevices[nomerDevices - 1] = device2.FriendlyName;
                Console.WriteLine($"Номер устройства - {nomerDevices}, Название - {device2.FriendlyName}");
                nomerDevices++;
                //i++;
            }
            nomerDevices = 1;
            nameDevice = nameDevices[nomerDevices - 1];
            Console.WriteLine($"Будет использовано устройство - {nameDevice}") ;
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
            nomerMemory = 1;
            memoryPhone = memoryPhones[nomerMemory - 1];
            device.Disconnect();
            Console.WriteLine($"Будет использована память - {memoryPhone}");
            return memoryPhone;
        }

    }
}
