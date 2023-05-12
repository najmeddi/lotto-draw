using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LottoDraw
{
    class Constants
    {
        public const int DRAW_SIZE = 6;
        public const string FILE_LOCATION = "./numbers.csv";
        public const string FORMAT = "{0,-25} {1, -20} {2, 10} {3, 20}";
        public static string MAIN_MENU_STR = "MENU\n----\n" + "\n[M]To main menu\n[1]Display games out\n[2]Display hot numbers\n[3]Display cold numbers\n[4]Display repeated numbers\n[5]Display number-group count\n[6]Display Draws\n[7]Display occurances of number\n[E]Exit";
        public const string ODD_EVEN_ERR_MSG = "Could not pick even odd numbers. Draw number list not initialzed.";
        public const string DEFAULT_OPTION_UNKOWN_MSG = "The option selected is unrecognized";
        public const int BONUS_NUM_INDEX = DRAW_SIZE;
        public const int DATE_INDEX = 0;
        public const char CSV_SPLIT_CHAR = ',';
        public const int SKIP_SIZE = 35;
        public const int LOW_GAMES_OUT = 5;
        public const int MID_GAMES_OUT = 10;
        public const int HIGH_GAMES_OUT = 17;
        public const int COLD_GAMES_OUT = 70;

    }
}
