using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artificial_Muse
{
    class Measure
    {
        public int cleftOffset;
        public Dictionary<int, double> keyOffsets = new Dictionary<int,double>();
        //Time signature parts
        public Fraction timeSignature;
        public int tempo = 0;
        bool timeChange;
        List<Chord> chords = new List<Chord>();
        
        public Measure(Measure measure)
        {
            cleftOffset = measure.cleftOffset;
            keyOffsets = new Dictionary<int, double>(measure.keyOffsets);
            timeSignature = new Fraction(measure.timeSignature);
            tempo = measure.tempo;
            timeChange = measure.timeChange;
            foreach(Chord chord in measure.chords)
            {
                chords.Add(new Chord(chord));
            }
            chords = new List<Chord>(measure.chords);
        }

        public Measure()
        {
        }

        public void addChord(Chord chord)
        {
            chords.Add(chord);
        }

        public bool isCorrect()
        {
            //Check validity of the measure
            Fraction length = new Fraction(0, 1);
            foreach (Chord chord in chords)
            {
                length += chord.length;
            }
            return length.ToDouble() == timeSignature.ToDouble();
        }
        
        public double getHighNote()
        {
            double pitch = -5000;
            foreach(Chord chord in chords)
            {
                if (pitch < chord.getHighNote())
                    pitch = chord.getHighNote();
            }
            return pitch;
        }

        public double getLowNote()
        {
            double pitch = 500; // Only about 88 keys on a piano
            foreach (Chord chord in chords)
            {
                if (pitch > chord.getLowNote())
                    pitch = chord.getLowNote();
            }
            return pitch;
        }

        public int getTempo()
        {
            return tempo;
        }

        public double getPercentChord()
        {
            Fraction lengthOfChords = new Fraction(0, 1);
            foreach (Chord chord in chords)
            {
                if (chord.isChord())
                    lengthOfChords += chord.length;
            }
            return (lengthOfChords / timeSignature).ToDouble();
        }

        public double getPercentTriplets()
        {
            Fraction lengthTriplets = new Fraction(0,1);
            foreach (Chord chord in chords)
            {
                if (chord.length.Denominator == 3 ||
                    chord.length.Denominator == 6 ||
                    chord.length.Denominator == 9)
                    lengthTriplets += chord.length;
            }
            return (lengthTriplets / timeSignature).ToDouble();
        }

        public double getAvgNoteLength()
        {
            return timeSignature.Numerator / (timeSignature.Denominator * chords.Count);
        }

        public double getPercentStacato()
        {
            Fraction lengthStacato = new Fraction(0, 1);
            foreach (Chord chord in chords)
            {
                if (chord.isStacato)
                    lengthStacato += chord.length;
            }
            return (lengthStacato / timeSignature).ToDouble();
        }

        public double getPercentSlur()
        {
            Fraction lengthSlurred = new Fraction(0, 1);
            foreach (Chord chord in chords)
            {
                if (chord.isSlurred)
                    lengthSlurred += chord.length;
            }
            return (lengthSlurred / timeSignature).ToDouble();
        }
    }
}
