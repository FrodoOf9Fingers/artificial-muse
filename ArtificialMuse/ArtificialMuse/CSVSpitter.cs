using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artificial_Muse
{
    class CSVSpitter
    {
        public void genCSV(List<Song> songs, String fileName)
        {
            List<String> lines = new List<String>();

            //Output header            
            lines.Add("#, highNote, lowNote, tempo, percentTriplets, percentStaccato, percentSlurs, percentChord, avgNoteLength, range, percentAccidental, numParts, fluidity");
            //Output Fields
            foreach (Song song in songs)
            {
                lines.Add(song.asString()); 
            }

            System.IO.File.WriteAllLines(fileName, lines);
        }
    }
}
