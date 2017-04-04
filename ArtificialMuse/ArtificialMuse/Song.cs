using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artificial_Muse
{
    class Song
    {
        public List<Staff> staffs = new List<Staff>();
        public string fileName = "";

        public string asString()
        {
            string line = "";
            FileInfo info = new FileInfo(fileName);

            line += info.Name + ",";
            line += getHighNote().ToString() + ",";
            line += getLowNote().ToString() + ",";
            line += getTempoAverage().ToString() + ",";
            line += getPercentTriplets().ToString() + ",";
            line += getPercentStaccato().ToString() + ",";
            line += getPercentSlur().ToString() + ",";
            line += getPercentChord().ToString() + ",";
            line += getAvgNoteLength().ToString() + ",";
            line += (getHighNote() - getLowNote()).ToString() + ",";
            line += getPercentAccidentals().ToString() + ",";
            line += staffs.Count.ToString() + ",";
            line += getFluidityAverage();

            return line;
        }
        public double getFluidityAverage()
        {
            Chord prevChord;
            double numChords = 0;
            double amountMovement = 0;
            foreach (Staff staff in staffs)
            {
                prevChord = null;
                foreach (Measure measure in staff.measures)
                    foreach (Chord chord in measure.chords)
                    {
                        if (!chord.isRest() && prevChord != null)
                        {
                            amountMovement += Math.Abs(chord.avgPitch() - prevChord.avgPitch());
                            prevChord = chord;
                            numChords++;
                        }
                        else if (prevChord == null && !chord.isRest())
                        {
                            numChords++;
                            prevChord = chord;
                        }
                    }
            }
            return amountMovement / numChords;
        }

        public double getPercentAccidentals()
        {
            double numNotes = 0;
            double numAccidentals = 0;
            foreach (Staff staff in staffs)
                foreach (Measure measure in staff.measures)
                    foreach (Chord chord in measure.chords)
                        foreach (Note note in chord.notes)
                        {
                            if (note.isAccident)
                                numAccidentals++;
                            numNotes++;
                        }
            return numAccidentals / numNotes;
        }



        public double getHighNote()
        {
            double pitch = -5000;
            foreach (Staff staff in staffs)
            {
                if (pitch < staff.getHighNote())
                    pitch = staff.getHighNote();
            }
            return pitch;
        }

        public double getLowNote()
        {
            double pitch = 500; // Only about 88 keys on a piano
            foreach (Staff staff in staffs)
            {
                if (pitch > staff.getLowNote())
                    pitch = staff.getLowNote();
            }
            return pitch;
        }

        public double getTempoAverage()
        {
            double avg = 0;
            foreach (Staff staff in staffs)
            {
                avg += staff.getTempoAverage();
            }
            return avg / (double)staffs.Count;
        }

        public double getPercentChord()
        {
            double avg = 0;
            foreach (Staff staff in staffs)
            {
                avg += staff.getPercentChord();
            }
            return avg / staffs.Count;
        }


        public double getPercentTriplets()
        {
            double numNotes = 0;
            double numTriplets = 0;
            foreach (Staff staff in staffs)
                foreach (Measure measure in staff.measures)
                    foreach (Chord chord in measure.chords)
                    {
                        if (chord.length.Denominator % 3 == 0)
                            numTriplets++;
                        numNotes++;
                    }
            return numTriplets / numNotes;
        }



        public double getAvgNoteLength()
        {
            double avg = 0;
            foreach (Staff staff in staffs)
            {
                avg += staff.getAvgNoteLength();
            }
            return avg / staffs.Count;
        }

        public double getPercentStaccato()
        {
            double avg = 0;
            foreach (Staff staff in staffs)
            {
                avg += staff.getPercentStaccato();
            }
            return avg / staffs.Count;
        }

        public double getPercentSlur()
        {
            double avg = 0;
            foreach (Staff staff in staffs)
            {
                avg += staff.getPercentSlur();
            }
            return avg / staffs.Count;
        }
    }
}
