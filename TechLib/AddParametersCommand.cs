using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.IO;
using ZetaLongPaths;

namespace TechLib
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    class AddParametersCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            /*
            
            // You can also use System.Environment.GetLogicalDrives to
            // obtain names of all logical drives on the computer.
            System.IO.DriveInfo di = new System.IO.DriveInfo(@"C:\");
            WriteToFile(Main.filename, di.TotalFreeSpace.ToString());
            WriteToFile(Main.filename, di.VolumeLabel.ToString());

            // Get the root directory and print out some information about it.
            System.IO.DirectoryInfo dirInfo = di.RootDirectory;
            WriteToFile(Main.filename, dirInfo.Attributes.ToString());

            // Get the files in the directory and print out some information about them.
            System.IO.FileInfo[] fileNames = dirInfo.GetFiles("*.*");

            foreach (System.IO.FileInfo fi in fileNames)
            {
                WriteToFile(Main.filename, String.Format("{0}: {1}: {2}", fi.Name, fi.LastAccessTime, fi.Length));
            }

            // Get the subdirectories directly that is under the root.
            // See "How to: Iterate Through a Directory Tree" for an example of how to
            // iterate through an entire tree.
            System.IO.DirectoryInfo[] dirInfos = dirInfo.GetDirectories("*.*");

            foreach (System.IO.DirectoryInfo d in dirInfos)
            {
                WriteToFile(Main.filename, d.Name);
            }
            */
            // The Directory and File classes provide several static methods
            // for accessing files and directories.

            //System.IO.Directory.SetCurrentDirectory(@"K:\Проекты\Выхино Жулебино 128Б В\ТИМ\В_работе\Архитектура\АПМ2\BIM-model");

            //WriteListFilesInFile();

            WalkDirectoryTree(new ZlpDirectoryInfo(@"K:\Проекты"));


            // Change the directory. In this case, first check to see
            // whether it already exists, and create it if it does not.
            // If this is not appropriate for your application, you can
            // handle the System.IO.IOException that will be raised if the
            // directory cannot be found.

            /*
            if (!System.IO.Directory.Exists(@"K:\Проекты\МОЦ_Киргизия\"))
            {
                //System.IO.Directory.CreateDirectory(@"K:\Проекты\МОЦ_Киргизия\");
            }


            currentDirName = System.IO.Directory.GetCurrentDirectory();
            WriteToFile(Main.filename, currentDirName);

            */

            // Keep the console window open in debug mode.
            //Console.WriteLine("Press any key to exit.");
            //Console.ReadKey();
            TaskDialog.Show("Warning", "Мы закончили ");
            //WriteToFile(Main.filename, "Привет, все ок?");
            return Result.Succeeded;
        }

        static void WalkDirectoryTree(ZlpDirectoryInfo root)
        {
            ZlpFileInfo[] files = null;
            ZlpDirectoryInfo[] subDirs = null;

            // First, process all the files directly under this folder
            try
            {
                files = root.GetFiles("*.rvt");
            }
            // This is thrown if even one of the files requires permissions greater
            // than the application provides.
            catch (UnauthorizedAccessException e)
            {
                // This code just writes out the message and continues to recurse.
                // You may decide to do something different here. For example, you
                // can try to elevate your privileges and access the file again.
                System.Windows.MessageBox.Show(e.Message);
                //log.Add(e.Message);
            }

            catch (System.IO.DirectoryNotFoundException e)
            {
                System.Windows.MessageBox.Show(e.Message);
                //Console.WriteLine(e.Message);
            }

            if (files != null)
            {
                foreach (ZlpFileInfo fi in files)
                {
                    // In this example, we only access the existing FileInfo object. If we
                    // want to open, delete or modify the file, then
                    // a try-catch block is required here to handle the case
                    // where the file has been deleted since the call to TraverseTree().
                    if(fi.FullName.ToString().Contains(@"ТИМ\В_работе"))
                    {
                        WriteToFile(Main.filename, String.Format("{0}\t{1}\t{2}", fi.Name, fi.LastWriteTime.ToString(), fi.Directory));
                    }
                    
                }

                // Now find all the subdirectories under this directory.
                subDirs = root.GetDirectories();

                foreach (ZlpDirectoryInfo dirInfo in subDirs)
                {
                    // Resursive call for each subdirectory.
                    WalkDirectoryTree(dirInfo);
                }
            }
        }
        public void WriteListFilesInFile()
        {
            // Get the current application directory.
            string currentDirName = System.IO.Directory.GetCurrentDirectory();
            WriteToFile(Main.filename, currentDirName);

            // Get an array of file names as strings rather than FileInfo objects.
            // Use this method when storage space is an issue, and when you might
            // hold on to the file name reference for a while before you try to access
            // the file.
            string[] files = System.IO.Directory.GetFiles(currentDirName, "*.rvt");

            foreach (string s in files)
            {
                // Create the FileInfo object only when needed to ensure
                // the information is as current as possible.
                System.IO.FileInfo fi = null;
                try
                {
                    fi = new System.IO.FileInfo(s);
                }
                catch (System.IO.FileNotFoundException e)
                {
                    // To inform the user and continue is
                    // sufficient for this demonstration.
                    // Your application may require different behavior.
                    WriteToFile(Main.filename, e.Message);
                    continue;
                }
                WriteToFile(Main.filename, String.Format("{0} : {1}", fi.Name, fi.Directory));
            }
        }

        public static void WriteToFile(string fileName, string txt)
        {

            if (!File.Exists(fileName))
            {
                try
                {
                    using (FileStream fs = File.Create(fileName))
                    {
                        byte[] info = new UTF8Encoding(true).GetBytes("");
                        fs.Write(info, 0, info.Length);
                    }
                }
                catch (Exception e)
                {
                    System.Windows.MessageBox.Show(e.ToString());
                }
            }
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine(txt);
            }

        }

        public static string ReadIniFile(string fileName)
        {
            string output = "";
            using (StreamReader reader = new StreamReader(fileName, true))
            {
                output = reader.ReadLine();
            }
            return output;
        }
    }
}
