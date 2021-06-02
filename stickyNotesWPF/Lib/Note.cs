using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stickyNotesWPF.Lib
{
    public class Note
    {
        /// <summary>
        /// Not'un içeriği
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// Not'un oluşturulduğu tarih
        /// </summary>
        public DateTime createdAt { get; set; }

        /// <summary>
        /// Not'un en son düzenlendiği tarih
        /// </summary>
        public DateTime editedAt { get; set; }

        /// <summary>
        /// Not'un eşsiz kimliği
        /// </summary>
        public string uuid { get; set; }

        /// <summary>
        /// Yeni oluşturukmuş objeye eşsiz bir id atayan constructor
        /// </summary>
        public Note()
        {
            uuid = Guid.NewGuid().ToString();
        }
    }
}
