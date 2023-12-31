using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NHotkey.Wpf;
using NHotkey;
using System.Windows.Forms;





namespace ClipToBase64
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public int Handle { get; private set; }
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte vk, byte scan, int flags, ref int extrainfo);


        public MainWindow()
        {
            InitializeComponent();
            HotkeyManager.Current.AddOrReplace("Increment", Key.B , ModifierKeys.Control | ModifierKeys.Shift, ClipboardToBase64);

        }

        [STAThread]
        private void ClipboardToBase64(object sender, HotkeyEventArgs e)
        {
            string fileName = "temp";
            string _001 = "d:\\00_Working_Temp\\" + fileName + ".png";
            string filePathAndName = _001;


            if ((System.Windows.Clipboard.GetDataObject() != null) && (System.Windows.Clipboard.ContainsImage()))
            {
                var image = System.Windows.Clipboard.GetImage();
                using (var fileStream = new FileStream(filePathAndName, FileMode.Create))
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(image));
                    encoder.Save(fileStream);
                }

                StringBuilder base64StringBuilder = new StringBuilder("<img src=\"data: image / png; base64,");

                byte[] imageBytes = File.ReadAllBytes(filePathAndName);
                string base64String = Convert.ToBase64String(imageBytes);
                base64StringBuilder.AppendFormat("{1}", fileName, base64String);
                base64StringBuilder.Remove(base64StringBuilder.Length - 2, 2);
                base64StringBuilder.Append("\" alt = \"\" > ");

                string _002 = base64StringBuilder.ToString();
                if (_002 != null)
                {
                    System.Windows.Clipboard.SetText(_002);
                }

                int extraInfoTemp = 0;  
                const byte KEYUP = 2;
                const byte KEYDOWN = 0;
                 
                keybd_event((byte)Keys.ControlKey, 0, KEYDOWN, ref extraInfoTemp);
                keybd_event((byte)Keys.Tab, 0, KEYDOWN, ref extraInfoTemp);
                keybd_event((byte)Keys.Tab, 0, KEYUP, ref extraInfoTemp);
                keybd_event((byte)Keys.ControlKey, 0, KEYUP, ref extraInfoTemp);


                keybd_event((byte)Keys.ControlKey, 0, KEYDOWN, ref extraInfoTemp);
                keybd_event((byte)Keys.V, 0, KEYDOWN, ref extraInfoTemp);
                keybd_event((byte)Keys.V, 0, KEYUP, ref extraInfoTemp);
                keybd_event((byte)Keys.ControlKey, 0, KEYUP, ref extraInfoTemp);

                keybd_event((byte)Keys.Down, 0, KEYDOWN, ref extraInfoTemp);
                keybd_event((byte)Keys.Down, 0, KEYUP, ref extraInfoTemp);

                keybd_event((byte)Keys.Enter, 0, KEYDOWN, ref extraInfoTemp);
                keybd_event((byte)Keys.Enter, 0, KEYUP, ref extraInfoTemp);

            }

            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
                ClipboardToBase64(this, null);
        }
    }
}