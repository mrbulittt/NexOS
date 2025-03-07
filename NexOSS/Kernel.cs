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
                    Games games = new Games();
                    games.Main(null);
                    break;
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
                    Console.WriteLine("Unknow operation");
                    break;
            }
        }

    }
    public class Games
    {
        internal void Main(object value)
        {
            throw new NotImplementedException();
        }
            public char[,] board = new char[3, 3];

        public char currentPlayer = 'X'; // используется для отслеживания игрока
        public void Main(string[] args)
        {
            InitializeBoard();
            bool gameOver = false;
            // происходит основной игровой процесс
            while (!gameOver)
            {
                DrawBoard();
                Console.WriteLine($"Игрок {currentPlayer}, ваш ход!");
                Console.Write("Введите строку (0, 1, 2): ");
                int row = int.Parse(Console.ReadLine());
                Console.Write("Введите столбец (0, 1, 2): ");
                int col = int.Parse(Console.ReadLine());
                if (IsValidMove(row, col))
                {
                    board[row, col] = currentPlayer;

                    if (CheckWin())
                    {
                        DrawBoard();
                        Console.WriteLine($"Игрок {currentPlayer} выиграл!");
                        gameOver = true;
                    }
                    else if (IsBoardFull())
                    {
                        DrawBoard();
                        Console.WriteLine("Ничья!");
                        gameOver = true;
                    }
                    else
                    {
                        currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
                    }
                }
                else
                {
                    Console.WriteLine("Недопустимый ход. Попробуйте снова.");
                }

            }
        }
        public void InitializeBoard() // пустое поле в начале игры
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = ' ';
                }
            }
        }
        public void DrawBoard() // рисует поле в начале игры
        {
            Console.WriteLine("  0 1 2");
            for (int i = 0; i < 3; i++)
            {
                Console.Write(i + " ");
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(board[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        public bool IsValidMove(int row, int col) // проверяет ход
        {
            if (row < 0 || row >= 3 || col < 0 || col >= 3)
            {
                return false;
            }
            return board[row, col] == ' ';
        }
        public bool CheckWin()
        {
            // Проверка строк и столбцов
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] != ' ' && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                {
                    return true;
                }
                if (board[0, i] != ' ' && board[0, i] == board[1, i] && board[1, i] == board[2, i])
                {
                    return true;
                }
            }

            // Проверка диагоналей
            if (board[0, 0] != ' ' && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
            {
                return true;
            }
            if (board[0, 2] != ' ' && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
            {
                return true;
            }

            return false;
        }
        public bool IsBoardFull() // проверяет заполненность доски
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == ' ')
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}




