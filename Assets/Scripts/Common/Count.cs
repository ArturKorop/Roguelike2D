using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Common
{
    [Serializable]
    public class Count
    {
        public int Minimum;
        public int Maximum;

        public Count(int min, int max)
        {
            this.Minimum = min;
            this.Maximum = max;
        }
    }
}
