using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDraw
{
    /// <summary>
    /// Class representation for picking draw numbers
    /// </summary>
    class DrawNumbers
    {
        private List<DrawNum> drawNumList = new List<DrawNum>();
        private DrawsFile df= new DrawsFile();
        private List<int> numGroups;
        private const int NUM_PICKS = 7;
        private int[] possArr = { 2, 3, 4 };

        public DrawNumbers()
        {
            // Initialize the draw numbers list
            int count = NUM_PICKS;
            while(count != 0)
            {
                this.drawNumList.Add(new DrawNum());
                count -= 1;
            }
            this.numGroups = this.ChooseRandNumGroup();
        }

        public List<List<int>> SelectDraws(List<int> lst, int size, int numDraws)
        {
            List<List<int>> result = new List<List<int>>();
            int i = 0;
            int ord = 1;
            DrawsFile df = new DrawsFile();
            while(i != numDraws)
            {
                List<int> randDraw = this.RandomSelect(lst, size);
                int numGroupCount = df.CheckNumGroup(randDraw);
                if(numGroupCount >= 5 && (!df.HasNumGroup(result, randDraw)))
                {
                    result.Add(randDraw);
                    Console.WriteLine("Number group occurances: " +  numGroupCount + " | Draw sum: " + this.DrawSum(randDraw));
                    ord += 1;
                    i += 1;
                }
            }
            return result;
        }

        private int DrawSum(List<int> draw)
        {
            int sum = 0;
            foreach(int n in draw)
            {
                sum += n;
            }
            return sum;
        }
        
        private List<int> RandomSelect(List<int> lst, int size)
        {
            Random r = new Random();
            List<int> result = new List<int>();
            int i = 0;
            while(i != size)
            {
                int randNum = lst[r.Next(lst.Count)];
                if (!result.Contains(randNum))
                {
                    result.Add(randNum);
                    i += 1;
                }

            }
            result.Sort((p1, p2) =>
            {
                return p1 - p2;
            });
            return result;
        }

        /// <summary>
        /// Randomly picks an arrangment of odd even placements in the
        /// draw list to assign.
        /// </summary>
        private void PickEvenOdd()
        {
            // Randomly pick arrangment
            Random rand = new Random();
            int randPick = rand.Next(0, 3);
            int randArr = this.possArr[randPick];
            // Ranomly pick even numbers
            List<int> blackList = new List<int>();
            while(randArr != 0)
            {
                // Pick a draw number index randomly
                int randIndex = rand.Next(0, NUM_PICKS - 1);
                if (blackList.Contains(randIndex))
                    continue;
                else
                    blackList.Add(randIndex);
                // Assign that index to be even
                this.drawNumList[randIndex].isEven = true;
                randArr -= 1;
            }

        }
        /// <summary>
        /// Randomly picks an arrangment of high low placements in the
        /// draw list to assign.
        /// </summary>
        private void PickHighLow()
        {
            // Randomly pick arrangment
            Random rand1 = new Random();
            int randPick = rand1.Next(0, 3);
            int randArr = this.possArr[randPick];
            // Ranomly pick low numbers
            List<int> blackList1 = new List<int>();
            while (randArr != 0)
            {
                // Pick a draw number index randomly
                int randIndex = rand1.Next(0, NUM_PICKS - 1);
                if (blackList1.Contains(randIndex))
                    continue;
                else
                    blackList1.Add(randIndex);
                // Assign that index to be even
                this.drawNumList[randIndex].isLow = true;
                randArr -= 1;
            }

        }


        private List<List<int>> MakeWheel()
        {
            List<List<int>> wheel = new List<List<int>>();
            Dictionary<List<int>, int> dict = df.GetNumGroupCount();
            Random rand = new Random();
            foreach(var map in dict)
            {
                double p = (double)map.Value / dict.Count;
                int count = Convert.ToInt32(p * 100);
                while (count != 0)
                {
                    wheel.Add(map.Key);
                    count -= 1;
                }
                    
            }
            wheel = wheel.OrderBy(x => rand.Next()).ToList();
            return wheel;
        }

        private List<int> ChooseRandNumGroup()
        {
            List<int> choice = new List<int>();
            Random r = new Random();
            var wheel = this.MakeWheel();
            int rand = r.Next(0, wheel.Count);
            choice = wheel[rand];
            return choice;
        }
        public override string ToString()
        {
            string result = "";
            foreach(var a in this.drawNumList)
            {
                result += a.num.ToString() + " " + a.isEven + "-";
            }
            return result;
        }
    }
}
