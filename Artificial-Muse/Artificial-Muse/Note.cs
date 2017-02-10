using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artificial_Muse
{
    class Note: Measure
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="length">Length of the note</param>
        /// <param name="pitch">The pitch of the note</param>
        Note(double length, string pitch)
        {
            this.length = length;
            this.pitch = pitch;
        }

        public double length
        {
            get;
            set;
        }

        public string pitch
        {
            get;
            set;
        }
    }
}
