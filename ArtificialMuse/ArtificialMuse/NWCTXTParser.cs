using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Artificial_Muse
{
    class NWCTXTParser
    {
        Song song = new Song();
        String prevLine = "";
        int numFailures = 0;

        public List<Song> parseSongs(string dirPath)
        {
            List<Song> songs = new List<Song>();
            foreach (String filePath in Directory.GetFiles(dirPath))
            {
                try
                {
                    songs.Add(parseSong(filePath));
                }
                catch (Exception)
                {
                    numFailures++;
                }
            }
            return songs;
        }

        public Song parseSong(string filePath)
        {
            String line;
            System.IO.StreamReader file = new System.IO.StreamReader(filePath);
            bool done = false;

            Song song = new Song();
            song.fileName = filePath;
            while (!done)
            {
                if (file.EndOfStream)
                    return song;

                if (prevLine.Length == 0)
                    line = file.ReadLine();

                else
                {
                    line = prevLine;
                    prevLine = "";
                }

                if (Regex.IsMatch(line, "\\|AddStaff\\|") &&
                    Regex.IsMatch(line, "\\|Group:\"Standard\""))
                {
                    song.staffs.Add(parseMeasures(file));
                }
                else if (Regex.IsMatch(line, "!NoteWorthyComposer-End"))
                {
                    done = true;
                }
            }
            return song;
        }

        private Staff parseMeasures(StreamReader reader)
        {
            Staff staff = new Staff();
            string line = "";
            bool done = false;
            int cleftOffset = 0;
            String keySig = "";
            String timeSig = "";
            bool inBar = false;
            int tempo = 0;

            int i = 0;
            Measure measure = new Measure();
            while (!done)
            {
                i++;
                line = reader.ReadLine();
                if (reader.EndOfStream)
                    break;

                //Clef
                if (Regex.IsMatch(line, "Clef"))
                {
                    if (Regex.IsMatch(line, "Treble"))
                    {
                        cleftOffset = 6;
                    }
                    else if (Regex.IsMatch(line, "Bass"))
                    {
                        cleftOffset = -6;
                    }
                }
                //Time signature
                else if (Regex.IsMatch(line, "\\|TimeSig\\|"))
                {
                    Regex rx = new Regex(":");
                    foreach (Match m in rx.Matches(line))
                    {
                        timeSig = line.Substring(m.Index);
                    }
                }
                //Key
                else if (Regex.IsMatch(line, "\\|Key\\|"))
                {
                    Regex rx = new Regex(":");
                    foreach (Match m in rx.Matches(line))
                    {
                        keySig = line.Substring(m.Index);
                    }
                }
                // New Measure
                else if (Regex.IsMatch(line, "\\|Bar"))
                {
                    measure.timeSignature = parseTimeSig(timeSig);
                    measure.cleftOffset = cleftOffset;
                    measure.keyOffsets = parseKey(keySig);
                    measure.tempo = tempo;
                    if (measure.isCorrect())
                        staff.addMeasure(measure);
                    measure = new Measure();
                }
                //Parse Rests
                else if (Regex.IsMatch(line, "\\|Rest\\|"))
                {
                    Note note = new Note();
                    note = parseLength(line, note);
                    note.pitch = 0;
                    note.isRest = true;
                    Chord chord = new Chord();
                    chord.length = note.length;
                    chord.addNote(note);
                    measure.addChord(chord);
                }
                //Parse Notes
                else if (Regex.IsMatch(line, "\\|Note\\|"))
                {
                    Note note = parseNotes(line)[0];
                    Chord chord = new Chord();
                    note = parseLength(line, note);
                    chord.length = note.length;
                    chord.addNote(note);
                    measure.addChord(chord);
                }
                //Parse Chords
                else if (Regex.IsMatch(line, "\\|Chord\\|"))
                {
                    Chord chord = new Chord();
                    chord.notes = parseNotes(line);
                    chord.length = chord.notes[0].length;
                    measure.addChord(chord);
                }
                //Parse Rest-Chords TODO: Improve this, it currently ignores the chord!
                else if (Regex.IsMatch(line, "\\|RestChord\\|"))
                {
                    Note note = new Note();
                    note = parseLength(line, note);
                    note.pitch = -1;
                    Chord chord = new Chord();
                    chord.length = note.length;
                    chord.addNote(note);
                    measure.addChord(chord);
                }
                // Tempo
                else if (Regex.IsMatch(line, "\\|Tempo\\|"))
                {
                    tempo = parseTempo(line);
                }
                // Found the end!
                else if (Regex.IsMatch(line, "\\|AddStaff"))
                {
                    done = true;
                    prevLine = line;
                }
                // Found the end of the song!
                else if (Regex.IsMatch(line, "!NoteWorthyComposer-End"))
                {
                    done = true;
                    prevLine = line;
                }
            }
            return staff;
        }

        private int parseTempo(string line)
        {
            Regex rx = new Regex("\\|Tempo:");
            int start = rx.Matches(line)[0].Index;
            rx = new Regex("\\|");
            int end = 0;
            int retval = 0;
            if (rx.IsMatch(line.Substring(start + 1)))
            {
                end = rx.Matches(line.Substring(start + 1))[0].Index;
                retval = Convert.ToInt32(line.Substring(start + 7, end - 6));
            }
            else
            {
                retval = Convert.ToInt32(line.Substring(start + 7));
            }

            return retval;
        }

        private Note parseNotePitch(String pitch)
        {
            Note note = new Note();

            double doublePitch = 0;
            double offset = 0;
            bool done = false;

            for (int i = 0; i < pitch.Length && !done; i++)
            {
                switch (pitch[i])
                {
                    case 'b':
                        offset = .5;
                        note.isAccident = true;
                        break;
                    case '#':
                        offset = -.5;
                        note.isAccident = true;
                        break;
                    default:
                        if (pitch[pitch.Length - 1] == '^')
                        {
                            note.isSlurred = true;
                            pitch = pitch.Substring(0, pitch.Length - 1);
                        }
                        doublePitch = Convert.ToInt32(pitch.Substring(i));
                        note.pitch = doublePitch;
                        done = true;
                        break;
                }
            }

            return note;
        }

        private List<Note> parseNotes(String line)
        {
            List<Note> notes = new List<Note>();
            Regex rx = new Regex("\\|Pos:");
            int start = rx.Matches(line)[0].Index;
            rx = new Regex("\\|");
            int end = 0;

            if (rx.IsMatch(line.Substring(start + 1)))
            {
                end = rx.Matches(line.Substring(start + 1))[0].Index;

                rx = new Regex(",");
                foreach (String pitch in rx.Split(line.Substring(start + 5, end - 4)))
                {
                    Note note = parseNotePitch(pitch);
                    note = parseLength(line, note);
                    notes.Add(note);
                }
            }
            else
            {
                rx = new Regex(",");
                foreach (String pitch in rx.Split(line.Substring(start + 5)))
                {
                    Note note = parseNotePitch(pitch);
                    note = parseLength(line, note);
                    notes.Add(note);
                }
            }

            return notes;
        }

        private Note parseLength(String line, Note note)
        {
            string lengthStr;
            Fraction length = new Fraction();
            length.Numerator = 1;
            Regex rx = new Regex("\\|Dur:");
            switch (line[rx.Matches(line)[0].Index + 5])
            {
                case 'w':
                case 'W':
                    length.Denominator = 1;
                    break;
                case 'h':
                case 'H':
                    length.Denominator = 2;
                    break;
                case 't':
                case 'T':
                case '3':
                    if (line[rx.Matches(line)[0].Index + 6] == '2')
                        length.Denominator = 32;
                    else
                        length.Denominator = 3;
                    break;
                case '1':
                    length.Denominator = 16;
                    break;
                case '8':
                    length.Denominator = 8;
                    break;
                case '4':
                    length.Denominator = 4;
                    break;
                default:
                    length.Denominator = 1;
                    break;
            }
            rx = new Regex("Staccato");
            if (rx.IsMatch(line))
                note.isStaccato = true;
            rx = new Regex("Triplet");
            if (rx.IsMatch(line))
                length *= new Fraction(2, 3);

            note.length = length;

            return note;

        }

        private Dictionary<int, double> parseKey(String keySig)
        {
            if (String.IsNullOrEmpty(keySig))
                return new Dictionary<int, double>();

            Dictionary<int, double> keyOffsets = new Dictionary<int, double>();
            Regex rx = new Regex(",");
            foreach (String pitch in rx.Split(keySig))
            {
                double offset = 0;
                if (Regex.IsMatch(pitch, "b"))
                    offset = -.5;
                else if (Regex.IsMatch(pitch, "#"))
                    offset = .5;

                keyOffsets.Add((pitch[0] - 67 % 7), offset);
            }
            return keyOffsets;
        }

        private Fraction parseTimeSig(String timeSig)
        {
            int num = 0;
            int dem = 0;

            int numPos = 0;
            int slashPos = 0;
            Regex rx = new Regex("(\\d)+\\/");
            foreach (Match m in rx.Matches(timeSig))
            {
                numPos = m.Index;
            }
            rx = new Regex("\\/");
            foreach (Match m in rx.Matches(timeSig))
            {
                slashPos = m.Index;
            }
            if (timeSig == ":Common")
            {
                num = 4;
                dem = 4;
            }
            else if (timeSig == ":AllaBreve")
            {
                num = 2;
                dem = 2;
            }
            else if (timeSig == "")
            {
                throw new Exception();
            }
            else
            {
                num = Convert.ToInt32(timeSig.Substring(numPos, slashPos - numPos));
                dem = Convert.ToInt32(timeSig.Substring(slashPos + 1));
            }

            return new Fraction(num, dem);
        }
    }
}
