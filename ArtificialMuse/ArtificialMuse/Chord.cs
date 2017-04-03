using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artificial_Muse
{
    class Chord
    {
        public List<Note> notes = new List<Note>();
        public Fraction length { get; set; }
        public bool isStacato = false;
        public bool isSlurred = false;

        public Chord(Chord chord)
        {
            isStacato = chord.isStacato;
            isSlurred = chord.isSlurred;
            length = new Fraction(chord.length);
            foreach(Note note in chord.notes)
            {
                notes.Add(new Note(note));
            }
        }

        public Chord()
        {
        }

        public void addNote(Note note)
        {
            notes.Add(note);
        }

        public bool isChord()
        {
            return notes.Count > 1;
        }
        public double getHighNote()
        {
            double pitch = 0; 
            foreach (Note note in notes)
            {
                if (pitch < note.pitch)
                    pitch = note.pitch;
            }
            return pitch;
        }
        public double getLowNote()
        {
            double pitch = 500;
            foreach (Note note in notes)
            {
                if (pitch > note.pitch && note.pitch != -1)
                    pitch = note.pitch;
            }
            return pitch;
        }
    }
}
