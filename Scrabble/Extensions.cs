using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Scrabble2018.Helpers
{
    public static class Extensions
    {

        public static Int32Rect FindButton(this Button[,] board, Button button)
        {
            for (int y = 0; y < board.GetLength(0); ++y)
            {
                for (int x = 0; x < board.GetLength(1); ++x)
                {
                    if (board[y, x] == button)
                    {
                        return new Int32Rect(x, y, 0, 0);
                    }
                }
            }
            return new Int32Rect(-1, -1, -1, -1);
        }

        public static string ExtractVerticalWord(this Button[,] board, int X, int Y)
        {
            string vertWord = "";
            for (int y = Y; y > 0; --y)
            {
                if (y == 0 || (char)board[y - 1, X].Content == '\0')
                {
                    for (int y2 = y; y2 < board.GetLength(0) && (char)board[y2, X].Content != '\0'; ++y2)
                        vertWord += (char)board[y2, X].Content;
                    break;
                }
            }
            return vertWord;
        }

        public static string ExtractHorizontalWord(this Button[,] board, int X, int Y)
        {
            string horWord = "";
            for (int x = Y; x > 0; --x)
            {
                if (x == 0 || (char)board[Y, x - 1].Content == '\0')
                {
                    for (int x2 = x; x2 < board.GetLength(1) && (char)board[Y, x2].Content != '\0'; ++x2)
                        horWord += (char)board[Y, x2].Content;
                    break;
                }
            }
            return horWord;
        }

    }
}
