using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Artificial_Muse
{
   static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String [] args)
        {
            NWCTXTParser parser = new NWCTXTParser();
            List<Song> songs = parser.parseSongs(args[0]);

            CSVSpitter spitter = new CSVSpitter();
            spitter.genCSV(songs, args[1]);
        }
    }
}
