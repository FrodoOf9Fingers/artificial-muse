using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artificial_Muse
{
    class Note
    {

        public Note(Note note)
        {
            length = note.length;
            pitch = note.pitch;
        }

        public Note()
        {
        }

        public double pitch { get; set; } // -1 for rests
        public Fraction length { get; set; }
        public bool isSlurred = false;
        public bool isRest = false;
        public bool isStaccato = false;
        public bool isAccident = false;
    }
}
