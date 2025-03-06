using Cosmos.HAL.Drivers.PCI.Audio;
using Cosmos.System.Audio.IO;
using Cosmos.System.Audio;
using Cosmos.System.Graphics;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Sys = Cosmos.System;

namespace CosmosKernel1
{
    public class Kernel : Sys.Kernel
    //Инициализация файловой системы
    {
        Sys.FileSystem.CosmosVFS fs = new Cosmos.System.FileSystem.CosmosVFS();
        string currentdirectory = @"0:\";
        //Все что используется перед запуском
        protected override void BeforeRun()
        {
            //Очищение консоли после проверок железа пк
            Console.Clear();
            //Регистрация файловой системы
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            Console.WriteLine("Nex OS booted sucsesfuly. If you need commands list use Help");
        }
        //Все что используется после запуска
        //Ссылаться на методы здесь
        protected override void Run()
        {
            Console.Write("Input: ");
            var input = Console.ReadLine();
            Console.WriteLine(input);

            Commands(input);
            Filesystem(input);
            Software(input);


        }
        //Системные команды
        public void Commands(string input)

        {
            switch (input)
            {
                //Вывод списка команд
                case "Help":
                    Console.WriteLine("\n1.Syscom \n2.Software \n3.Power");
                    break;
                case "Syscom":
                    Console.WriteLine("\n1.Ver \n2.Sysinfo \n3.Free space \n4.Fylesys type");
                    break;
                case "Software":
                    Console.WriteLine("\n1.Calc \n2.Calendar \n3.Games");
                    break;
                case "Power":
                    Console.WriteLine("\n1.Shutdown \n2.Reboot");
                    break;
                //Отображения версии системы
                case "Ver":
                    Console.WriteLine("Nex Technology [OS version 1.0.0]");
                    break;
                //Отображения информации о железе пользователя
                case "Sysinfo":
                    string CPUbrand = Cosmos.Core.CPU.GetCPUBrandString();
                    string CPUvendor = Cosmos.Core.CPU.GetCPUVendorName();
                    uint amount_of_ram = Cosmos.Core.CPU.GetAmountOfRAM();
                    ulong availible_ram = Cosmos.Core.GCImplementation.GetAvailableRAM();
                    uint UsedRam = Cosmos.Core.GCImplementation.GetUsedRAM();
                    Console.WriteLine(@"CPU: {0}
CPU Vendor: {1}
Amount of RAM: {2} MB
Avialible of RAM: {3} MB
Used RAM: {4}B", CPUbrand, CPUvendor, amount_of_ram, availible_ram, UsedRam);
                    break;

                //Выключение
                case "Shutdown":
                    Cosmos.System.Power.Shutdown();
                    break;
                //Перезагрузка
                case "Reboot":
                    Cosmos.System.Power.Reboot();
                    break;
                case "clear":
                    Console.Clear();
                    break;

            }
        }

        //Команды файловой системы
        public void Filesystem(string input)
        {
            string filename = "";
            string dirname = "";
            switch (input)
            {
                //Отображение наличия свободного места
                case "Free space":
                    var available_space = fs.GetAvailableFreeSpace(@"0:\");
                    Console.WriteLine("Available Free Space: " + available_space);
                    break;
                default:
                    Console.WriteLine("Unknown command. Type HELP for a list of commands.");
                    break;
                //Определение файловой системы пользователя
                case "Fylesys type":
                    var fs_type = fs.GetFileSystemType(@"0:\");
                    Console.WriteLine("File System Type: " + fs_type);
                    break;
                //Выбор директории
                case "cd":
                    currentdirectory = Console.ReadLine();
                    break;
                //Создание файлов
                case "mkfile":
                    filename = Console.ReadLine();
                    fs.CreateFile(currentdirectory + filename);
                    break;
                //Создание директорий
                case "mkdir":
                    dirname = Console.ReadLine();
                    fs.CreateDirectory(currentdirectory + dirname);
                    break;
                    filename = Console.ReadLine();
                //Удаление файлов 
                case "delfile":
                    Sys.FileSystem.VFS.VFSManager.DeleteFile(currentdirectory + filename);
                    break;
                    dirname = Console.ReadLine();
                //Удаление директорий
                case "deldir":
                    Sys.FileSystem.VFS.VFSManager.DeleteFile(currentdirectory + dirname);
                    break;
                //чтение файлов
                case "dir":
                    try
                    {
                        var directory_list = Sys.FileSystem.VFS.VFSManager.GetDirectoryListing(currentdirectory);
                        foreach (var directoryEntry in directory_list)
                        {
                            try
                            {
                                var entry_type = directoryEntry.mEntryType;
                                if (entry_type == Sys.FileSystem.Listing.DirectoryEntryTypeEnum.File)
                                {
                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine("| <File>       " + directoryEntry.mName);
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                                if (entry_type == Sys.FileSystem.Listing.DirectoryEntryTypeEnum.Directory)
                                {
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.WriteLine("| <Directory>      " + directoryEntry.mName);
                                    Console.ForegroundColor = ConsoleColor.White;
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error: Directory not found");
                                Console.WriteLine(e.ToString());
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        break;
                    }
                    break;
            }
        }
        //Програмное обеспечение
        public void Software(string input)
        {

            switch (input)
            {
                case "Games":

                case "Calendar":
                case "Calc":
                    Calc();
                    break;

            }
        }
        public void Calc()
        {
            Console.WriteLine("Choise operation (+, -, *, /, ^, v(for sqrt), b(for break)):");
            char input = Console.ReadKey().KeyChar;
            Console.WriteLine();

            double a = 0, b = 0, c = 0;

            switch (input)
            {
                case '+':
                    Console.WriteLine("Enter numb a:");
                    a = double.Parse(Console.ReadLine());
                    Console.WriteLine("Enter numb b:");
                    b = double.Parse(Console.ReadLine());
                    c = a + b;
                    Console.WriteLine($"Result: {c}");
                    return;

                case '-':
                    Console.WriteLine("Enter numb a:");
                    a = double.Parse(Console.ReadLine());
                    Console.WriteLine("Enter numb b:");
                    b = double.Parse(Console.ReadLine());
                    c = a - b;
                    Console.WriteLine($"Result: {c}");
                    return;

                case '*':
                    Console.WriteLine("Enter numb a:");
                    a = double.Parse(Console.ReadLine());
                    Console.WriteLine("Enter numb b:");
                    b = double.Parse(Console.ReadLine());
                    c = a * b;
                    Console.WriteLine($"Result: {c}");
                    return;

                case '/':
                    Console.WriteLine("Enter numb a:");
                    a = double.Parse(Console.ReadLine());
                    Console.WriteLine("Enter numb b:");
                    b = double.Parse(Console.ReadLine());
                    if (b != 0)
                    {
                        c = a / b;
                        Console.WriteLine($"Result: {c}");
                    }
                    else
                    {
                        Console.WriteLine("Error:  you cannot divide by zero.");
                    }
                    return;

                case '^':
                    Console.WriteLine("Enter numb a:");
                    a = double.Parse(Console.ReadLine());
                    Console.WriteLine("Enter power b:");
                    b = double.Parse(Console.ReadLine());
                    c = Math.Pow(a, b);
                    Console.WriteLine($"Result: {c}");
                    return;

                case 'v':
                    Console.WriteLine("Enter numb a:");
                    a = double.Parse(Console.ReadLine());
                    c = Math.Sqrt(a);
                    Console.WriteLine($"Result: {c}");
                    return;
                case 'b':
                    break;
                default:
                    Console.WriteLine("Unknow operation in this task");
                    break;
            }
        }

    }
}
