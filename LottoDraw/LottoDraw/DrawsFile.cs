using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LottoDraw
{
    /// <summary>
    /// Class representation of previous lotto data
    /// </summary>
    class DrawsFile
    {
        
        private readonly List<string> drawList;     // List representation of strings of previous drawed numbers
        private Dictionary<List<int>, int> numGroupCountDict = new Dictionary<List<int>, int>();
        private List<List<int>> ng = new List<List<int>>();
        
        /// <summary>
        /// Class constructor for a file object that contains data
        /// on pervious draws.
        /// </summary>
        public DrawsFile()
        {
            using (var reader = new StreamReader(Path.GetFullPath(Constants.FILE_LOCATION)))
            {
                // Initialize the draw list object
                drawList = new List<string>();
                // Add line as a new string to the draw list until the buffer stream ends
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    drawList.Add(line);
                }
            }
            ng = this.GetNumberGroupsList();
        }

        //------------------------------- PREVIOUS DRAWS -----------------------------------------------------------------//

        /// <summary>
        /// Returns list of drawed numbers.
        /// </summary>
        /// <returns> A list of strings being the drawed numbers with dates.</returns>
        public List<string> GetDrawsList()
        {
            return this.drawList;
        }

        /// <summary>
        /// Returns list of string arrays that contain the date and numbers
        /// of draws
        /// </summary>
        /// <returns>Returns list of string arrays that contain the date and numbers
        /// of draws</returns>
        public List<string[]> GetSplitDrawsList()
        {
            List<string[]> result = new List<string[]>();
            foreach (string line in this.drawList)
            {
                result.Add(line.Split(Constants.CSV_SPLIT_CHAR));
            }
            return result;
        }

        /// <summary>
        /// Returns list of string arrays that contain the numbers
        /// of draws
        /// </summary>
        /// <returns>Returns list of string arrays that contain the numbers
        /// of draws</returns>
        public List<int[]> GetNumbersSplitList()
        {
            List<string[]> splitList = this.GetSplitDrawsList();
            List<int[]> result = new List<int[]>();
            foreach(string[] line in splitList)
            {
                var lineList = new List<string>(line);
                lineList.RemoveAt(Constants.BONUS_NUM_INDEX);
                lineList.RemoveAt(Constants.DATE_INDEX);
                List<int> newLine = new List<int>();
                for(int i = 0; i < lineList.Count; i++)
                {
                     newLine.Add(Convert.ToInt32(lineList[i]));
                }
                result.Add(newLine.ToArray());    
            }
            return result;
        }

        /// <summary>
        /// Returns a previous draw that contains the number to check.
        /// </summary>
        /// <param name="numToCheck">The integer to check last occurance of</param>
        /// <returns></returns>
        public int[] GetPreviousOccurances(int numToCheck)
        {
            int[] result = new List<int>().ToArray<int>();
            List<int[]> prevDraws = this.GetNumbersSplitList();
            foreach(int[] draw in prevDraws)
            {
                if (draw.Contains<int>(numToCheck))
                {
                    result = draw;
                    break;
                }
            }
            return result;
        }
        //-------------------------------------------- END OF PREVIOUS DRAWS ----------------------------------------------------//

        //-------------------------------------------- NUMBER GROUPS ----------------------------------------------------//

        /// <summary>
        /// Returns list of number groups.
        /// </summary>
        /// <returns>Returns a list containing nested list of integers being the number group combinations</returns>
        public List<List<int>> GetNumberGroupsList()
        {
            List<List<int>> result = new List<List<int>>();
            foreach(int [] line in this.GetNumbersSplitList())
            {
                //var newNumGroup = this.CondenseNumGroup(this.GetNumberGroup(line));
                var newNumGroup = this.GetNumberGroup(line);
                result.Add(newNumGroup);
            
            }
            var resultCopy = result;
            result = this.RemoveDuplicates(result);
            // TODO: Remove. for testing purposes
            foreach(List<int> numGroup in result)
            {
                this.numGroupCountDict.Add(numGroup, this.CountDuplicates(resultCopy, numGroup));
                
            }
            return result;
        }

        private List<List<int>> RemoveDuplicates(List<List<int>> lst)
        {
            List<List<int>> result = new List<List<int>>();
            foreach(List<int> numGroup in lst)
            {
                if (this.HasNumGroup(result, numGroup))
                {
                    continue;
                }
                else
                {
                    result.Add(numGroup);
                }
            }
            return result;
        }

        public bool HasNumGroup(List<List<int>> lst, List<int> numGroup)
        {
            bool has = false;
            foreach(List<int> l in lst)
            {
                has = this.CompareNumGroups(l, numGroup);
            }
            return has;
        }

        private bool CompareNumGroups(List<int> lst1, List<int> lst2)
        {
            bool matches = true;
            bool result = false;
            if (lst1.Count == lst2.Count)
            {
                for (int i = 0; i < lst1.Count; i++)
                {
                    if (lst1[i] != lst2[i])
                        matches = false;
                }
                if (matches)
                {
                    result = true;
                }
            }
            return result;
        }
        /// <summary>
        /// Given a nested list and a list of int (the elements of the list), count the number of 
        /// times the nested list occurs.
        /// </summary>
        /// <param name="lst">The list to count from.</param>
        /// <param name="numLst">The element to count for.</param>
        /// <returns>The number of time the nested list appears in the list.</returns>
        public int CountDuplicates (List<List<int>> lst, List<int> numLst)
        {
            int count = 0;
            foreach(List<int> l in lst)
            {
                if(l.Count == numLst.Count)
                {
                    bool match = true;
                    for (int i = 0; i < l.Count; i++)
                    {
                        if (l[i] != numLst[i])
                            match = false;
                    }
                    if (match)
                        count += 1;

                }
            }
            return count;
        }
        
        /// <summary>
        /// Given a list of int, returns a list of int where
        /// repeated occurances of the same int given is omitted.
        /// </summary>
        /// <param name="uncondensedNumGroup">List that may have int repeating.</param>
        /// <returns>List of int where repeats are omitted.</returns>
        private List<int> CondenseNumGroup(List<int> uncondensedNumGroup)
        {
            List<int> result = new List<int>();
            foreach (int num in uncondensedNumGroup)
            {
                if (result.Contains(num))
                    continue;
                result.Add(num);
            }
            return result;
        }

        private List<int> GetNumberGroup(int [] draw)
        {
            List<int> result = new List<int>();

            foreach (int drawNum in draw)
            {
                result.Add(this.ConvertToNumGroup(drawNum));
            }
            return result;
        }

        private int ConvertToNumGroup(int num)
        {
            return Convert.ToInt32(Math.Floor((Convert.ToDouble(num) / 10))) * 10;
        }

        /// <summary>
        /// Returns a dictionary of list of integers that map to a count of
        /// number of occurances for those list of integers.
        /// </summary>
        /// <returns>Returns a dictionary of list of integers that map to a count of
        /// number of occurances for those list of integers.</returns>
        public Dictionary<List<int>, int> GetNumGroupCount()
        {
            return this.numGroupCountDict;
        }

        public int CheckNumGroup(List<int> draw)
        {
            int result = 0;
            Dictionary<List<int>, int> numGroupsDict = this.GetNumGroupCount();
            var numGroups = numGroupsDict.Keys;
            List<int> cDraw = this.GetNumberGroup(draw.ToArray());
            
            foreach(var key in numGroups)
            {
                if (CompareNumGroups(key, cDraw))
                {
                    result = numGroupsDict[key];

                }
            }
            return result;
        }
        //------------------------------------- END OF NUMBER GROUPS -----------------------------------------------------------//

        //-------------------------------------- GAMES OUT ----------------------------------------------------------//
        /// <summary>
        /// Returns the number of times a number has not occured in drawed numbers
        /// </summary>
        /// <param name="num">The number to count occurances of.</param>
        /// <param name="offset">The date offset to check from.</param>
        /// <returns>Returns the number of times a number has not occured in drawed numbers.</returns>
        private int GetGamesOut(int num, int offset)
        {
            int gamesOut = 0;
            List<int[]> prevDraws = this.GetNumbersSplitList();
            int i = offset;
            while (!prevDraws[i].Contains(num))
            {
                gamesOut += 1;
                i += 1;
            }
            return gamesOut;
        }
        
        private int[] GetDrawGamesOut(int [] draw, int offset)
        {
            List<int> gamesOutList = new List<int>();
            foreach(int num in draw)
            {
                gamesOutList.Add(this.GetGamesOut(num, offset));
            }
           
            return gamesOutList.ToArray();
        }
        
        private List<int[]> GetPreviousGamesOut()
        {
           
            int offset = Constants.SKIP_SIZE - 1;
            var nums = this.GetNumbersSplitList();
            List<int[]> result = new List<int[]>();
            while (offset != -1)
            {
                result.Add(this.GetDrawGamesOut(nums[offset], offset + 1));
                
                offset -= 1;
            }
            result.Reverse();
            
            
            return result;
        }
        
        
        public Dictionary<int, int> GetTotalGamesOut()
        {
            List<int[]> gamesOut = this.GetPreviousGamesOut();
            Dictionary<int, int> result = new Dictionary<int, int>();
            for(int i = 0; i < Constants.SKIP_SIZE; i ++)
            {
                result.Add(i, 0);
                foreach(int[] miss in gamesOut)
                {
                    foreach(int num in miss)
                    {
                        if(num == i)
                        {
                            result[i] += 1;
                        }
                    }
                }
            }
            return result;
        }
        //------------------------------------- END OF GAMES OUT-----------------------------------------------------------//


        //-------------------------------------- HOT/COLD NUMBERS ----------------------------------------------------------//

        public int [] GetHotNumbers(int gamesOut)
        {
            List<int> results = new List<int>();
            List<int[]> draws = this.GetNumbersSplitList();
            for(int i = 1; i < 50; i++)
            {
                results.Add(i);
            }
            for(int i = 0; i < gamesOut; i ++)
            {
                for(int j = 0; j < draws[i].Length; j ++)
                {
                    int num = draws[i][j];
                    if(results.Count != 0 && results.Contains(num))
                    {
                        results.Remove(num);
                    }
                }
            }

            return results.ToArray();
        }

        public int [] GetColdNumbers()
        {
            return this.GetHotNumbers(Constants.COLD_GAMES_OUT);
        }


        //-------------------------------------- END OF HOT/COLD NUMBERS ----------------------------------------------------------//

        //-------------------------------------- REPEATED HITS ----------------------------------------------------------//

        
       
        
        public int GetTotalRepeatedHits(int num, int numHits)
        {
            // TODO: Redesign algorithm for getting repeated hits on numbers
            int hits = 0;
            int timesShown = 0;
            List<int[]> prevDraws = this.GetNumbersSplitList();
            foreach(int[] drawNums in prevDraws)
            {
                if (DrawNumContains(drawNums, num))
                {
                    if (timesShown > 0)
                    {
                        if (timesShown == numHits)
                        {
                            timesShown = 0;
                            hits += 1;
                        }
                        else
                            timesShown += 1;
                    }
                    else
                        timesShown += 1;
                }
                else
                    timesShown = 0;
            }
            return hits;
        }

        
        private bool DrawNumContains(int[] drawNum, int n)
        {
            bool doesContain = false;
            foreach(int num in drawNum)
            {
                if (num == n)
                    doesContain = true;
            }
            return doesContain;


        }
        //-------------------------------------- END OF REPEATED HITS ----------------------------------------------------------//

        //---------------------------------- I/O ---------------------------------------------------//

        public string OutputAllRepeatedHits(int numHits)
        {
            string result = "";
            int max = 0;
            int winner = 0;
            for (int i = 1; i < 50; i++)
            {
                int hits = this.GetTotalRepeatedHits(i, numHits);
                result += "NUMBER: " + i + " HITS: " + hits + "\n";
                if(hits > max)
                {
                    max = hits;
                    winner = i;
                }
            }
            return result + "MOST REPEATED HITS OF " + numHits + ": " + winner + " WITH " + max + " HITS";
        }

        /// <summary>
        /// Adds a new draw to the file and draw list.
        /// </summary>
        /// <param name="date">The date the numbers are drawn.</param>
        /// <param name="drawNums">The numbers that have been drawn.</param>
        public void AddDraw(string date, string drawNums)
        {
            // TODO: Write the date and numbers drawn into the CSV file

            // TODO: Update the draw list to include the new draw

        }

        /// <summary>
        /// Outputs a list's elements to the conole
        /// </summary>
        /// <param name="lst">The desired list to output elements.</param>
        public string OutputResult(List<int> lst)
        {
            string result = "";
            foreach(int o in lst)
            {
                result += o.ToString() + "\t";
            }
            return result;
        }

        /// <summary>
        /// Outputs a list's elements to the conole
        /// </summary>
        /// <param name="lst">The desired list to output elements.</param>
        public string OutputDict(Dictionary<List<int>, int> dict)
        {
            string result = "";
            int valCount = 0;
            int keyCount = 0;
            foreach (var map in dict)
            {
                string keyStr = this.OutputResult(map.Key);
                result += "[NUM GROUP: " + keyStr + ", COUNT: " + map.Value.ToString() + "]\n";
                valCount+= map.Value;
                keyCount += 1;
            }
            result += "TOTAL NUMBER OF NUM GROUPS: " + keyCount + "\nTOTAL OF ALL COUNTS: " + valCount +"\nTOTAL DRAWS: " + this.GetNumbersSplitList().Count;
            return result;
        }

        public string OutputGamesOut()
        {
            string result = string.Format(Constants.FORMAT, "\nPREVIOUS DRAWS", "GAMES OUT", "TYPE OF SKIP", "NUMBER OF OUTS");
            result += string.Format(Constants.FORMAT, "\n--------------", "---------", "------------", "-------------\n");
            List<int[]> prevOut = this.GetPreviousGamesOut();
            List<int[]> prevDraws = this.GetNumbersSplitList();
            Dictionary<int, int> dict = GetTotalGamesOut();
            for (int i = 0; i < prevOut.Count; i++)
            {
                string str1 = "";
                string str2 = "";
                for(int j = 0; j < prevDraws[i].Length; j++)
                {
                    str1 += prevDraws[i][j] + " ";
                }
                for(int j = 0; j < prevOut[i].Length; j++)
                {
                    str2 += prevOut[i][j] + " ";
                }
                str1 = i + ". " + str1.Remove(str1.Length - 1);
                str2 = str2.Remove(str2.Length - 1);
                result += string.Format(Constants.FORMAT, str1, str2, i, dict[i] + "\n");
            }
            return result;
    
        }

        public string OutputHotColdNums(int[] numbers)
        {
            string result = "\nNUMBERS: ";
            if (numbers.Length != 0)
            {
                foreach (int num in numbers)
                {
                    result += num + ", ";
                }
                result = result.Remove(result.Length - 2);
            }
            else
                result += "NONE";
            return result;
        }

        public string OutputPreviousOccurances(int num)
        {
            int[] draw = this.GetPreviousOccurances(num);
            string output = "";
            if(draw.Length == 0)
            {
                output = "No previous occurances of " + num + ".";
            }
            else
            {
                string drawStr = "";
                foreach(int n in draw)
                {
                    drawStr += n.ToString() + ", ";
                }
                drawStr = drawStr.Remove(drawStr.Length - 2);
                output = "The previous draw for " + num + " is " + drawStr;
            }
            return output;

        }

        public override string ToString()
        {
            string strResult = "";
            for(int i = 0; i < this.drawList.Count() - 1; i++)
            {
                strResult += this.drawList[i] + "\n";
            }
            return strResult;
        }
        //-------------------------------- END OF I/O ----------------------------------------------------------------//
    }
}
