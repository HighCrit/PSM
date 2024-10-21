namespace PSM.Gui
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var args = Environment.GetCommandLineArgs();

            if (args.Length > 1)
            {
                var path = args[1];
                if (Path.Exists(path) && path.EndsWith(".xml"))
                {
                    Application.Run(new PSMConstructorGui(path));
                }
                else
                {
                    Application.Run(new PSMConstructorGui());
                }
            }
            else
            {
                Application.Run(new PSMConstructorGui());
            }

        }
    }
}