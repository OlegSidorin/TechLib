using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using adWin = Autodesk.Windows;
using System.Windows;

namespace TechLib
{
    public class Main : IExternalApplication
    {
        public static string filename { get; set; } = @"C:\Users\Sidorin_O\Documents\TEST\dirinfo.txt";
        public static string TabName { get; set; } = "Надстройки";
        public static string PanelTechName { get; set; } = "Диск К";

        public Result OnStartup(UIControlledApplication application)
        {
            var techPanel = application.CreateRibbonPanel(PanelTechName);
            string path = Assembly.GetExecutingAssembly().Location;

            var TechBtnData = new PushButtonData("TechBtnData", "Модели на К\nв работе", path, "TechLib.AllFilesCommand")
            {
                ToolTipImage = new BitmapImage(new Uri(Path.GetDirectoryName(path) + "\\res\\tech-32.png", UriKind.Absolute)),
                ToolTip = "Создает отчет о файлах ревита на диске К"
            };
            var TechBtn = techPanel.AddItem(TechBtnData) as PushButton;
            TechBtn.LargeImage = new BitmapImage(new Uri(Path.GetDirectoryName(path) + "\\res\\tech-32.png", UriKind.Absolute));

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

    }
}
