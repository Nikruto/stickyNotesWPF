using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using stickyNotesWPF.Lib;


namespace stickyNotesWPF
{
    public partial class NoteWindow : Window
    {
        /// <summary>
        /// Not pencerelerinin masaüstüne yapışıp yapışmayacağını tutan değişken
        /// </summary>
        private bool sticky = true;

        /// <summary>
        /// Not penceresinin ait olduğu <see cref="Note"/> objesi
        /// </summary>
        private Note _note;

        /// <summary>
        /// Verilen class ve pencere ismi ile pencereyi bulan FindWindow methodunu user32.dll kaynağından importluyoruz
        /// </summary>
        /// <param name="lpClassName">Bulunacak pencerenin class adı</param>
        /// <param name="lpWindowName">Bulunacak pencerenin adı</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// Verilen parent pencere handle'ını yine verilen child pencere handle'ının ebeveyni yapan methodu user32.dll kaynağından
        /// importluyoruz
        /// </summary>
        /// <param name="child">Parent'ı değiştirelecek pencerenin handle'ı</param>
        /// <param name="parent">Child'ın ebeveyni yapılacak pencerenin handle'ı</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr child, IntPtr parent);

        /// <summary>
        /// <see cref="SubscribeToNoteManager"/> methodunu çağırarak ilgili methodları abone eden,
        /// Pencerenin içeriğini not'un içeriğine eşitleyen,
        /// Not penceresi yüklenmeyi tamamladığında <see cref="NoteWindow_Loaded(object, RoutedEventArgs)"/> methodunun çağırılmasını sağlayan
        /// constructor
        /// </summary>
        public NoteWindow(Note note)
        {
            _note = note;

            InitializeComponent();
            SubscribeToNoteManager();
           
            TextInput.Text = note.content;

            if(sticky)
                this.Loaded += NoteWindow_Loaded;
        }

        /// <summary>
        /// Not penceresi yüklendiğinde çağırılan, not penceresinin her zaman masaüstünde gözükmesi için Masaüstü penceresini
        /// <see cref="FindWindow(string, string)"/> methodu ile bulup, <see cref="SetParent(IntPtr, IntPtr)"/> methodu ile
        /// not penceresini masaüstü penceresinin child'ı yapan method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoteWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var hWnd = new WindowInteropHelper(this).Handle;
            IntPtr s = FindWindow("Progman", "Program Manager");
            SetParent(hWnd, s);
        }

        /// <summary>
        /// Title kısmına tıklanıldığında çağırılan, pencerenin sürüklenmesini sağlayan fonksiyon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitleGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        /// <summary>
        /// <see cref="NoteManager"/>'ın not ile ilgili eventlerine, ilgili fonksiyonları abone eden method
        /// </summary>
        private void SubscribeToNoteManager()
        {
            App.noteManager.OnNoteRemoved += OnNoteRemoved;
            App.noteManager.OnNoteUpdated += OnNoteUpdated;
        }

        /// <summary>
        /// Bir not silindiğinde çağırılan, removedNote parametresi'nin uuid'si pencereye atanan <see cref="_note"/>'nin uuid'sine
        /// eşitse bu pencereyi kapatan method.
        /// </summary>
        /// <param name="removedNote">Silinen <see cref="Note"/> objesi</param>
        private void OnNoteRemoved(Note removedNote)
        {
            if (_note.uuid == removedNote.uuid) this.Close();
        }

        /// <summary>
        /// Bir not güncellendiğinde çağırılan, updatedNote parametresi'nin uuid'si pencereye atanan <see cref="_note"/>'nin uuid'sine
        /// eşitse pencerenin içeriğini güncelleyen method
        /// </summary>
        /// <param name="updatedNote">İçeriği güncellenen <see cref="Note"/> objesi</param>
        private void OnNoteUpdated(Note updatedNote)
        {
            if(updatedNote.uuid == _note.uuid)
                TextInput.Text = updatedNote.content;
        }

        /// <summary>
        /// Not penceresinin içeriği değiştirildiğinde çağırılan, <see cref="NoteManager.UpdateNote(Note, string)"/> methodunu
        /// <see cref="_note"/> yani bu pencereye atanan <see cref="Note"/> objesi ve pencerenin yeni içeriği ile çağırarak
        /// not'un güncellenmesini sağlayan method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            App.noteManager.UpdateNote(_note, TextInput.Text);
        }

        /// <summary>
        /// Not penceresinin sağ üstünde bulunan buton'a tıklanıldığında çağırılan, bu pencereyi kapatan method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        /// <summary>
        /// Not penceresinin sol üstünde bulunan buton'a tıklanıldığında çağırılan, eğer not listesi penceresi
        /// kapatılmışsa yeni bir not listesi penceresi oluşturup onu gösteren method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.mainWindowInstance != null) return;

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            mainWindow.Focus();
        }

    }
}
