using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace stickyNotesWPF.Lib
{
    static class NoteFileManager
    {
        /// <summary>
        /// Bu değişken uygulama verilerinin saklandığı dizinin yolunu tutuyor
        /// </summary>
        public static readonly string APP_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"stickyNotes");

        /// <summary>
        /// Bu değişken notların saklandığı dosyanın yolunu tutuyor
        /// </summary>
        public static readonly string SAVE_PATH = Path.Combine(APP_PATH,"notes.json");

        /// <summary>
        /// Uygulamanın verilerini kaydettiği klasör eğer oluşturulmamışsa oluşturan fonksiyon
        /// </summary>
        public static void CreateAppDirIfNotExists()
        {
            if (Directory.Exists(APP_PATH)) return;

            Directory.CreateDirectory(APP_PATH);
        }

        /// <summary>
        /// Uygulamanın verilerini kaydettiği dosya eğer oluşturulmamışsa oluşturan fonksiyon
        /// </summary>
        public static void CreateNotesFileIfNotExists()
        {
            if (File.Exists(SAVE_PATH)) return;

            CreateAppDirIfNotExists();

            Save save = new Save();
            save.Notes = new List<Note>();
            File.WriteAllText(SAVE_PATH,JsonConvert.SerializeObject(save,Formatting.Indented));
        }

        /// <summary>
        /// Diskteki notların kaydedildiği dosyayı okuyup onları Note türünden nesnelerin tutulduğu bir liste
        /// olarak döndüren fonksiyon
        /// </summary>
        /// <returns><see cref="Note"/> listesi</returns>
        public static List<Note> GetNotes()
        {
            CreateNotesFileIfNotExists();

            List<Note> notes = JsonConvert.DeserializeObject<Save>(File.ReadAllText(SAVE_PATH)).Notes;

            return notes;
        }

        /// <summary>
        /// Verilen <see cref="Note"/> listesini diske kaydeden fonksiyon
        /// </summary>
        /// <param name="notesToSave">Kaydedilecek notlar</param>
        public static void SaveNotes(List<Note> notesToSave)
        {
            CreateAppDirIfNotExists();

            Save save = new Save();
            save.Notes = notesToSave;

            File.WriteAllText(SAVE_PATH, JsonConvert.SerializeObject(save,Formatting.Indented));
        }
    }
}
