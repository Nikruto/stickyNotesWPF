using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stickyNotesWPF.Lib
{
    /// <summary>
    /// Json dosyasına kaydedilecek, not listesini tutan class
    /// </summary>
    class Save
    {
        /// <summary>
        /// Kaydedilecek notlar listesi
        /// </summary>
        public List<Note> Notes { get; set; }
    }
}
