namespace TronGame;

static class Program
{
    /// <summary>
    ///  The main entry point for the application. Here the program will execute it self and call all methods
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }    
}