using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDraw
{
    class Program
    {

        static void Main(string[] args)
        {
            //DrawsFile df = new DrawsFile();
            bool programIsActive = true;
            DrawsFile df = new DrawsFile();
            DrawNumbers dn = new DrawNumbers();
            //Console.WriteLine(df.OutputDict(df.GetNumGroupCount()));
            //Console.WriteLine(df.OutputGamesOut());
            //Console.WriteLine(df.OutputHotColdNums(df.GetHotNumbers(Constants. MID_GAMES_OUT)));
            //Console.WriteLine(df.OutputHotColdNums(df.GetColdNumbers()));
            //Console.WriteLine(df.OutputAllRepeatedHits(1));
            //DrawNumbers dn = new DrawNumbers();
            Console.WriteLine(Constants.MAIN_MENU_STR);
            
            while (programIsActive)
            {
                
                Console.Write("Option: ");
                var option = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("Enter [M] to back to main menu or [E] to exit.\n");
                List<int> pickList = new List<int>();
                for (int i = 1; i <= 49; i++)
                {
                    pickList.Add(i);
                }
                int[] pick = pickList.ToArray();
                switch (option)
                {
                    case "1":
                        Console.WriteLine(df.OutputGamesOut());
                        break;
                    case "2":
                        Console.WriteLine("LOW: " + df.OutputHotColdNums(df.GetHotNumbers(Constants.LOW_GAMES_OUT)));
                        Console.WriteLine("MID: " + df.OutputHotColdNums(df.GetHotNumbers(Constants.MID_GAMES_OUT)));
                        Console.WriteLine("HIGH: " + df.OutputHotColdNums(df.GetHotNumbers(Constants.HIGH_GAMES_OUT)));
                        break;
                    case "3":
                        Console.WriteLine(df.OutputHotColdNums(df.GetColdNumbers()));
                        break;
                    case "4":
                        Console.Write("Select nummber of repeats: ");
                        int numRepeats = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine(df.OutputAllRepeatedHits(numRepeats));
                        break;
                    case "5":
                        Console.WriteLine(df.OutputDict(df.GetNumGroupCount()));
                        break;
                    case "6":
                        int numDraws = 10;
                        List<List<int>> y = dn.SelectDraws(pick.ToList(), Constants.DRAW_SIZE, numDraws);
                        foreach(List<int> draw in y) { Console.WriteLine(df.OutputResult(draw)); };
                        break;
                    case "7":
                        Console.Write("Select a number to check: ");
                        int num = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine(df.OutputPreviousOccurances(num));
                        break;
                    case "e":
                        programIsActive = false;
                        break;
                    case "m":
                        Console.WriteLine(Constants.MAIN_MENU_STR);
                        break;
                    default:
                        Console.WriteLine(Constants.DEFAULT_OPTION_UNKOWN_MSG);
                        break;
                }
            }
            
        }
    }
}
