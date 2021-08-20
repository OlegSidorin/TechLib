using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.IO;
using ZetaLongPaths; // Использование ZetaLongPaths вызвано длинными путями

namespace TechLib
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    class AllFilesCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            WalkDirectoryTree(new ZlpDirectoryInfo(@"K:\Проекты"));

            TaskDialog.Show("Warning", "Мы закончили ");

            return Result.Succeeded;
        }

        static void WalkDirectoryTree(ZlpDirectoryInfo root)
        {
            ZlpFileInfo[] files = null;
            ZlpDirectoryInfo[] subDirs = null;
            try
            {
                files = root.GetFiles("*.rvt");
            }
            catch (UnauthorizedAccessException e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }

            if (files != null)
            {
                foreach (ZlpFileInfo fi in files)
                {
                    string name = fi.FullName.ToString();
                    if (name.Contains(@"ТИМ\В_работе") && (!name.Contains("BIM_координатор")))
                    {
                        WriteToFile(Main.filename, String.Format("{0}\t{1}\t{2}", fi.Name, fi.LastWriteTime.ToString(), fi.Directory));
                    }
                }

                subDirs = root.GetDirectories();

                foreach (ZlpDirectoryInfo dirInfo in subDirs)
                {
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
