﻿using System;
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
            //System.IO.StreamWriter outStream = new System.IO.StreamWriter(fileName);
            List<String> lines = new List<String>();

            //Output header            
            lines.Add("#, hn, ln, tempo, pt, ps, psl, pc, nl, range");
            //Output Fields
            for (int i = 0; i < songs.Count; i++)
            {
                lines.Add(Convert.ToString(i) + "," + songs[i].asString()); 
            }

            System.IO.File.WriteAllLines(fileName, lines);
        }
    }
}
