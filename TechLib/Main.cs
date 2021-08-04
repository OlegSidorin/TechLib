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
        public static string TabName { get; set; } = "Надстройки";
        public static string PanelTechName { get; set; } = "Технология";

        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                application.ControlledApplication.DocumentSynchronizingWithCentral += AppEvent_DocumentSynchronizingWithCentral_Handler;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            var techPanel = application.CreateRibbonPanel(PanelTechName);

            string path = Assembly.GetExecutingAssembly().Location;

            var TechBtnData = new PushButtonData("TechBtnData", "Добавить\nпараметры", path, "TechLib.AddParametersCommand")
            {
                ToolTipImage = new BitmapImage(new Uri(Path.GetDirectoryName(path) + "\\res\\tech-32.png", UriKind.Absolute)),
                ToolTip = "Добавляет необходимые параметры для семейства ТХ"
            };
            var TechBtn = techPanel.AddItem(TechBtnData) as PushButton;
            TechBtn.LargeImage = new BitmapImage(new Uri(Path.GetDirectoryName(path) + "\\res\\tech-32.png", UriKind.Absolute));

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            application.ControlledApplication.DocumentSynchronizingWithCentral -= AppEvent_DocumentSynchronizingWithCentral_Handler;
            return Result.Succeeded;
        }

        public static void AppEvent_DocumentSynchronizingWithCentral_Handler(Object sender, EventArgs args)
        {
            MessageBox.Show("Синхронизируемся...");

        }
    }
}
