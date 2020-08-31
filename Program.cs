using System;
using System.IO;
using System.Threading;

namespace DW
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length != 1)
            {
                Console.WriteLine("Используйте параметры: dw.exe [путь к папке]");
                return;
            }

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = args[0];

            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;


            watcher.Filter = "*.*";
            watcher.IncludeSubdirectories = true;
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);


            watcher.EnableRaisingEvents = true;
            Console.WriteLine("Бортжурнал веб-разработчика awebmaster.ru");
            Console.WriteLine("Мониторинг работает...");
            Console.WriteLine("Для остановки нажмите Ctrl+Break");
            while (true) Thread.Sleep(1000);
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            string fileOrDirectory;
            string fileID = FileID.getFileUniqueSystemID(e.FullPath);

            if (e.ChangeType == WatcherChangeTypes.Deleted) fileOrDirectory = "unknown";
            else if (Directory.Exists(e.FullPath)) fileOrDirectory = "directory";
            else fileOrDirectory = "file";

            Console.WriteLine("{0}|{1}|{2}|{3}", e.ChangeType, fileOrDirectory, fileID, e.FullPath);
            return;
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            string fileOrDirectory;
            string fileID = FileID.getFileUniqueSystemID(e.FullPath);

            if (Directory.Exists(e.FullPath)) fileOrDirectory = "directory";
            else fileOrDirectory = "file";

            Console.WriteLine("{0}|{1}|{2}|{3}|{4}", e.ChangeType, fileOrDirectory, fileID, e.OldFullPath, e.FullPath);

            return;
        }
    }
}

