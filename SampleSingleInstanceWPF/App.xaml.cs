using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;


namespace SampleSingleInstanceWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [DllImport( "user32" )]
        static extern bool IsIconic( IntPtr hWnd );

        [DllImport( "user32" )]
        static extern bool ShowWindow( IntPtr hWnd, int cmdShow );
        const int SW_RESTORE = 9;

        [DllImport( "user32" )]
        static extern bool SetForegroundWindow( IntPtr hWnd );

        private void Application_Startup( object sender, StartupEventArgs e )
        {
            Process current_process = Process.GetCurrentProcess( );
            Debug.Assert( current_process.MainWindowHandle == IntPtr.Zero );

            Process? other_process = Process.GetProcessesByName( current_process.ProcessName ).FirstOrDefault( p => p.Id != current_process.Id && p.MainWindowHandle != IntPtr.Zero );

            if( other_process != null )
            {
                if( IsIconic( other_process.MainWindowHandle ) )
                {
                    ShowWindow( other_process.MainWindowHandle, SW_RESTORE );
                }

                SetForegroundWindow( other_process.MainWindowHandle );

                Shutdown( );
            }
        }
    }

}
