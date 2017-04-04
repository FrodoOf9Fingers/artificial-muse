using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Artificial_Muse
{
    class Staff
    {
        List<Measure> measures = new List<Measure>();

        public void addMeasure(Measure measure)
        {
            Measure temp = new Measure(measure);
            measures.Add(temp);
        }

        public double getHighNote()
        {
            double pitch = -5000;
            foreach (Measure measure in measures)
            {
                if (pitch < measure.getHighNote())
                    pitch = measure.getHighNote();
            }
            return pitch;
        }

        public double getLowNote()
        {
            double pitch = 500; // Only about 88 keys on a piano
            foreach (Measure measure in measures)
            {
                if (pitch > measure.getLowNote())
                    pitch = measure.getLowNote();
            }
            return pitch;
        }

        public double getTempoAverage()
        {
            double avg = 0;
            foreach(Measure measure in measures)
            {
                avg += measure.getTempo();
            }
            return avg / measures.Count;
        }

        public double getPercentChord()
        {
            double avg = 0;
            foreach (Measure measure in measures)
            {
                avg += measure.getPercentChord();
            }
            return avg / measures.Count;
        }

        public double getPercentTriplets()
        {
            double avg = 0;
            foreach (Measure measure in measures)
            {
                avg += measure.getPercentTriplets();
            }
            return avg / measures.Count;
        }

        public double getAvgNoteLength()
        {
            double avg = 0;
            foreach (Measure measure in measures)
            {
                avg += measure.getAvgNoteLength();
            }
            return avg / measures.Count;
        }

        public double getPercentStacato()
        {
            double avg = 0;
            foreach (Measure measure in measures)
            {
                avg += measure.getPercentStacato();
            }
            return avg / measures.Count;
        }

        public double getPercentSlur()
        {
            double avg = 0;
            foreach (Measure measure in measures)
            {
                avg += measure.getPercentSlur();
            }
            return avg / measures.Count;
        }
    }
}
