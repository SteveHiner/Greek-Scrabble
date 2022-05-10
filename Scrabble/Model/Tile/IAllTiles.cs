namespace Scrabble.Model
{
    public interface IAllTiles
    {
        bool Empty();
        void MakeTiles();
        //static bool WordEqual(string s1, string s2, bool strict);
    }
}