using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using stickyNotesWPF.Lib;

namespace stickyNotesWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static NoteManager noteManager;
        public static MainWindow mainWindowInstance;
        public App()
        {
            NoteFileManager.CreateAppDirIfNotExists();
            noteManager = new NoteManager();
        }
    }
}
