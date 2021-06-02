using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stickyNotesWPF.Lib
{
    public class NoteManager
    {
        /// <summary>
        /// Notlar listesi
        /// </summary>
        public List<Note> notes;

        /// <summary>
        /// Not eklenme eventinin kullandığı delegate
        /// </summary>
        /// <param name="addedNote">Eklenen not</param>
        public delegate void NoteAddedDelegate(Note addedNote);

        /// <summary>
        /// Not silme eventinin kullandığı delegate
        /// </summary>
        /// <param name="removedNote"></param>
        public delegate void NoteRemovedDelegate(Note removedNote);

        /// <summary>
        /// Not güncellenme eventinin kullandığı delegate
        /// </summary>
        /// <param name="updatedNote"></param>
        public delegate void NoteUpdatedDelegate(Note updatedNote);

        /// <summary>
        /// Bir not eklendiğinde tetiklenen event
        /// </summary>
        public event NoteAddedDelegate OnNoteAdded;

        /// <summary>
        /// Bir not silindiğinde tetiklenen event
        /// </summary>
        public event NoteRemovedDelegate OnNoteRemoved;

        /// <summary>
        /// Bir not güncellendiğinde tetiklenen event
        /// </summary>
        public event NoteUpdatedDelegate OnNoteUpdated;

        /// <summary>
        /// Yeni oluşturulan objenin notes listesini NoteFileManager class'ı aracılığı ile
        /// diskten okuyan constructor
        /// </summary>
        public NoteManager()
        {
            notes = NoteFileManager.GetNotes();
        }

        /// <summary>
        /// notes dizisinin elemanlarını NoteFileManager class'ı aracılığı ile disk'e kaydeden fonksiyon
        /// </summary>
        private void SaveToDisk()
        {
            NoteFileManager.SaveNotes(notes);
        }

        /// <summary>
        /// Alınan note parametresini listeye kaydedip, ilgili event'i tetikleyip son olarak disk'e kaydeden
        /// fonksyion
        /// </summary>
        /// <param name="note">Eklenecek not objesi</param>
        public void AddNote(Note note)
        {
            notes.Add(note);
            if(OnNoteAdded != null)
            {
                OnNoteAdded(note);
            }
            SaveToDisk();
        }

        /// <summary>
        /// Alınan note parametresini listeden silip, ilgili event'i tetikleyip son olarak disk'ten silen
        /// fonksiyon
        /// </summary>
        /// <param name="note">Silinecek not objesi</param>
        public void RemoveNote(Note note)
        {
            notes.Remove(notes.Single(x => x.uuid == note.uuid));
            if (OnNoteRemoved != null)
            {
                OnNoteRemoved(note);
            }
            SaveToDisk();
        }

        /// <summary>
        /// Alınan not objesinin içeriğini, yine alınan string'e atayıp ilgili eventleri çağırdıktan sonra 
        /// disk'e kaydeden fonksiyon
        /// </summary>
        /// <param name="noteToUpdate">Güncellenecek not objesi</param>
        /// <param name="newContent">Not objesinin yeni içeriği</param>
        public void UpdateNote(Note noteToUpdate, string newContent)
        {
            noteToUpdate.content = newContent;
            noteToUpdate.editedAt = DateTime.Now;
            if(OnNoteUpdated != null)
            {
                OnNoteUpdated(noteToUpdate);
            }
            SaveToDisk();
        }
    }
}
