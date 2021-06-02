using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using stickyNotesWPF.Lib;

namespace stickyNotesWPF
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Not snippetlarına en son tıklanılan zamanı tutan değişken
        /// </summary>
        private DateTime _lastClickedTime = DateTime.MinValue;

        /// <summary>
        /// Spn tıklanılan not snippet'ının uuid'sini tutan değişken
        /// </summary>
        private string _lastClickedItemId = null;

        /// <summary>
        /// <see cref="SubscribeToNoteManager"/> methodunu çağırarak ilgili methodları abone eden,
        /// <see cref="UpdateList"/> methodunu çağırarak listeyi ilk kez renderlayan,
        /// <see cref="App.mainWindowInstance"/>'a bu pencere nesnesini atayan ve
        /// son olarak pencerenin kapanma eventine <see cref="App.mainWindowInstance"/>'ı null yapan
        /// bir lamda atayan constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            SubscribeToNoteManager();
            UpdateList();
           
            App.mainWindowInstance = this;
            Closing += (object sender, CancelEventArgs e) => App.mainWindowInstance = null;
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
        /// Pencerenin sol üstünde bulunan '+' butonuna tıklanıldığında çağırılan, yeni <see cref="Note"/> objesi oluşturup
        /// onu listeye ekledikten sonra bu not ile <see cref="NoteWindow"/> oluşturan fonksiyon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Note newNote = new Note();
            DateTime time = DateTime.Now;
            newNote.content = "";
            newNote.createdAt = time;
            newNote.editedAt = time;

            App.noteManager.AddNote(newNote);

            NoteWindow noteWindow = new NoteWindow(newNote);
            noteWindow.Show();
            noteWindow.Focus();
        }

        /// <summary>
        /// Ekranın sağ üstündeki 'x' butonuna tıklanıldığında pencereyi kapatan fonksiyon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Arama kutusundaki yazı değiştirildiğinde çağırılan, <see cref="UpdateList"/> fonksiyonunu tetikleyerek
        /// listenin yeniden renderlanmasını sağlayan fonksiyon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTermTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateList();
        }

        /// <summary>
        /// <see cref="NoteManager"/>'ın not ile ilgili eventlerine, ilgili fonksiyonları abone eden fonksiyon
        /// </summary>
        private void SubscribeToNoteManager()
        {
            App.noteManager.OnNoteAdded += OnNoteAdded;
            App.noteManager.OnNoteRemoved += OnNoteRemoved;
            App.noteManager.OnNoteUpdated += OnNoteUpdated;
        }

        /// <summary>
        /// Yeni not eklendiğinde çağırılan, <see cref="UpdateList"/> fonksiyonunu tetikleyerek
        /// listenin yeniden renderlanmasını sağlayan fonksiyon
        /// </summary>
        /// <param name="addedNote"></param>
        private void OnNoteAdded(Note addedNote)
        {
            UpdateList();
        }

        /// <summary>
        /// Bir not silindiğinde çağırılan, <see cref="UpdateList"/> fonksiyonunu tetikleyerek
        /// listenin yeniden renderlanmasını sağlayan fonksiyon
        /// </summary>
        /// <param name="removedNote"></param>
        private void OnNoteRemoved(Note removedNote)
        {
            UpdateList();
        }

        /// <summary>
        /// Bir not güncellendiğinde çağırılan, <see cref="UpdateList"/> fonksiyonunu tetikleyerek
        /// listenin yeniden renderlanmasını sağlayan fonksiyon
        /// </summary>
        /// <param name="updatedNote"></param>
        private void OnNoteUpdated(Note updatedNote)
        {
            UpdateList();
        }

        /// <summary>
        /// <see cref="NoteManager"/>'daki <see cref="NoteManager.notes"/>'ları arama kriterine göre filtreleyen,
        /// güncellenme tarihine göre sıralayan ve son olarak bunları <see cref="NoteSnippet.NoteSnippetFromNote(Note)"/>
        /// ile <see cref="NoteSnippet"/> türünden nesnelere dönüştürdükten sonra itemlar'a atayan fonksiyon
        /// </summary>
        private void UpdateList()
        {
            List<Note> viewList = App.noteManager.notes;

            itemsList.ItemsSource = viewList.FindAll(note => note.content.Contains(SearchTermTextBox.Text)).OrderByDescending(note => note.editedAt).Select(note => NoteSnippet.NoteSnippetFromNote(note));
        }

        /// <summary>
        /// NoteSnippetlarından herhangi birisi tıklanınca çağırılan, double sol click eventini 0.5 bekleme süresi
        /// ile kontrol ettikten sonra tıklanan snippet'ın <see cref="NoteSnippet.note"/> nesnesi ile yeni bir <see cref="NoteWindow"/> oluşturan fonksiyon.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NoteSnippet snippet = ((Grid)sender).Tag as NoteSnippet;

            if (e.ChangedButton == MouseButton.Left)
            {
                if (_lastClickedItemId == snippet.note.uuid && (DateTime.Now - _lastClickedTime).TotalSeconds < 0.5)
                {
                    NoteWindow noteWindow = new NoteWindow(snippet.note);
                    noteWindow.Show();
                    noteWindow.Focus();

                    _lastClickedItemId = null;
                }
                else
                {
                    _lastClickedItemId = snippet.note.uuid;
                    _lastClickedTime = DateTime.Now;
                }
            }

        }

        /// <summary>
        /// Not snippetlarından herhangi birisine sağ tıklanıldığında açılan ContextMenu'de "Not'u sil" butonuna baslınıca tetiklenen,
        /// ve event'i tetikleyen snippet'ın <see cref="NoteSnippet.note"/> objesini not listesinden silen method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            NoteSnippet snippet = ((MenuItem)sender).Tag as NoteSnippet;
            App.noteManager.RemoveNote(snippet.note);
        }
    }
}
