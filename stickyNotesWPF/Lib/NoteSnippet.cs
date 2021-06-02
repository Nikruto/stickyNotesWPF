using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stickyNotesWPF.Lib
{
    public class NoteSnippet
    {
        /// <summary>
        /// Snippet'ın ait olduğu <see cref="Note"/> objesi
        /// </summary>
        public Note note { get; set; }

        /// <summary>
        /// Snippet'ın içeriği
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// Snippet'ın güncellendiği saat:dakika
        /// </summary>
        public string time { get; set; }

        /// <summary>
        /// Alınan parametreleri ilgili üye değişkenlere atayan constructor.
        /// </summary>
        /// <param name="_note">Snippet'ın ait olduğu not objesi</param>
        /// <param name="_text">Snippet'ın içeriği</param>
        /// <param name="_time">Snippet'ın sağ ksımında gösterilen tarih yazısı</param>
        public NoteSnippet(Note _note, string _text, string _time)
        {
            this.note = _note;
            this.text = _text;
            this.time = _time;
        }

        /// <summary>
        /// Verilen not objesinden NoteSnippet nesnesi oluşturan fonksiyon
        /// </summary>
        /// <param name="note">Snippet'ı oluşturulacak not objesi</param>
        /// <returns></returns>
        public static NoteSnippet NoteSnippetFromNote(Note note)
        {
            string hour = note.editedAt.Hour < 10 ? $"0{note.editedAt.Hour}" : $"{ note.editedAt.Hour}";
            string minute = note.editedAt.Minute < 10 ? $"0{note.editedAt.Minute}" : $"{ note.editedAt.Minute}";

            return new NoteSnippet(note, note.content, $"{hour}:{minute}");
        }
    }
}
