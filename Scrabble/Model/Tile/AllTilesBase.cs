using System.Collections.Generic;
using System.IO;

namespace Scrabble.Model
{
    public class AllTilesBase
    {
        public List<Tile> ListTiles;

        public bool Empty()
        {
            if (ListTiles.Count > 0) return false;
            else return true;
        }

        public static bool WordEqual(string s1, string s2, bool strict)
        {
            if (strict)
                return s1.Equals(s2);
            else
                return s1.Equals(s2, System.StringComparison.OrdinalIgnoreCase);
        }

        public static IEnumerable<string> Getwords()
        {
            return File.ReadAllLines(@"Model\Word\wordlist.txt");
        }
    }
}