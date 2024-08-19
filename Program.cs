using System;
using System.Windows.Forms;
namespace TronGame;

static class Program
{
    /// <summary>
    ///  The main entry point for the application. Here the program will execute it self and call all methods
    /// </summary>
    [STAThread]
    static void Main()
    {
        AllocConsole(); // Esto abre la consola

        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Form1());
    }    

    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
}