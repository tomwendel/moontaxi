#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
#endregion

namespace MoonTaxi
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool isServer = false;
            string username=null;
            using (MainWindow form = new MainWindow())
            {
                Application.Run(form);
                if (form.DialogResult != DialogResult.OK)
                    return;
                isServer = form.IsServer;
                username = form.Username;
            }

            using (var game = new MoonTaxi(isServer,username))
                game.Run();
        }
    }
#endif
}
