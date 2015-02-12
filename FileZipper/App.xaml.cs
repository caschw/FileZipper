using System;
using System.Windows;

namespace FileZipper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public App()
        {
            Startup += App_Startup;
            this.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
        }

        /// <summary>
        /// Any unhandled exceptions come here.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Additional startup tasks.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_Startup(object sender, StartupEventArgs e)
        {

        }

        /// <summary>
        /// Any necessary Exit tasks.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit( e );
        }

    }
}
