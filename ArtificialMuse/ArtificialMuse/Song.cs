using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artificial_Muse
{
    class Song
    {
        public List<Staff> staffs = new List<Staff>();

        public string asString()
        {
            string line = "";
            line += getHighNote().ToString() + ",";
            line += getLowNote().ToString() + ",";
            line += getTempoAverage().ToString() + ",";
            line += getPercentTriplets().ToString() + ",";
            line += getPercentStacato().ToString() + ",";
            line += getPercentSlur().ToString() + ",";
            line += getPercentChord().ToString() + ",";
            line += getAvgNoteLength().ToString() + ",";
            line += (getHighNote() - getLowNote()).ToString();

            return line;
        }

        public double getHighNote()
        {
            double pitch = 0;
            foreach (Staff staff in staffs)
            {
                if (pitch < staff.getHighNote())
                    pitch = staff.getLowNote();
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
            return avg / staffs.Count;
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
            double avg = 0;
            foreach (Staff staff in staffs)
            {
                avg += staff.getPercentTriplets();
            }
            return avg / staffs.Count;
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

        public double getPercentStacato()
        {
            double avg = 0;
            foreach (Staff staff in staffs)
            {
                avg += staff.getPercentStacato();
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
