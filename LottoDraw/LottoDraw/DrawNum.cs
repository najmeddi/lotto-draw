using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDraw
{

    /// <summary>
    /// Class representation of drawed numbers
    /// </summary>
    class DrawNum
    {
        public int num;
        public bool isEven = false;
        public bool isLow = false;
        public int numGroup;
        public DrawNum()
        {
            
        }
        
        public void ChooseNum()
        {
            if(this.numGroup == 0)
            {
                this.num = (new Random()).Next(this.numGroup + 10);
            }
            else
            {
                this.num = (new Random()).Next(this.numGroup - 10, this.numGroup);
            }
            
        }
        public override string ToString()
        {
            return this.num.ToString();
        }

    }
}
