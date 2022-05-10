using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Scrabble.Model
{
    public class AllGreekTiles : AllTilesBase, IAllTiles
    {
        public AllGreekTiles()
        {
            MakeTiles();
        }

        private string Letters = "αβγδεζηθικλμνξοπρσςτυφχψω";
        private string Accents = "'`~";
        private string UnaccentedVowels = "αεηιουω";
        private static string Capitals = "ΑΒΓΔΕΖΗΘΙΚΛΜΝΞΟΠΡΣΤΥΦΧΨΩ";
        private static string Lowers = "αβγδεζηθικλμνξοπρςστυφχψω";
        private static string accentedα = "ἀἁἂἃἄἅἆἇἈἉἊἋἌἍἎἏὰάᾀᾁᾂᾃᾄᾅᾆᾇᾈᾉᾊᾋᾌᾍᾎᾏᾰᾱᾲᾳᾴᾶᾷᾸᾹᾺΆᾼ";
        private static string accentedε = "ἐἑἒἓἔἕἘἙἚἛἜἝὲέῈΈΈ";
        private static string accentedη = "ἠἡἢἣἤἥἦἧἨἩἪἫἬἭἮἯὴήᾐᾑᾒᾓᾔᾕᾖᾗᾘᾙᾚᾛᾜᾝᾞᾟῂῃῄῆῇῊΉῌΉ";
        private static string accentedι = "ἰἱἲἳἴἵἶἷἸἹἺἻἼἽἾἿὶίῐῑῒΐῖῗῘῙῚΊΊΐ";
        private static string accentedο = "ὀὁὂὃὄὅὈὉὊὋὌὍὸόῸΌΌ";
        private static string accentedυ = "ὐὑὒὓὔὕὖὗὙὛὝὟὺύῠῡῢΰῦῧῨῩῪΎΎ";
        private static string accentedω = "ὠὡὢὣὤὥὦὧὨὩὪὫὬὭὮὯὼώᾠᾡᾢᾣᾤᾥᾦᾧᾨᾩᾪᾫᾬᾭᾮᾯῲῳῴῶῷῺΏῼΏ";

        public void MakeTiles()
        {
            ListTiles = new List<Tile>();
            foreach (char c in Letters)
            {
                for (int x = 0; x < NumOfLetters(c); x++)
                {
                    Tile t = new Tile(c, ScoreOfLetter(c));
                    ListTiles.Add(t);
                }
            }
            Tile b = new Tile('-', 0);
            ListTiles.Add(b); // blank
            ListTiles.Add(b);
        }

        /*
         * Tiles distribution
         * 98 tiles ?
         * 2 blank tiles(?)
         */
        public static int ScoreOfLetter(char c)
        {
            var s = c.ToString();
            if ("αεηιουωνρτλσς".Contains(s)) return 1;
            else if ("δγ".Contains(s)) return 2;
            else if ("βμπ".Contains(s)) return 3;
            else if ("θφψ".Contains(s)) return 4;
            else if ("κ".Contains(s)) return 5;
            else if ("χ".Contains(s)) return 8;
            else if ("ξζ".Contains(s)) return 10;
            else if ("-".Contains(s)) return 0;
            else return 0;
        }

        [ExcludeFromCodeCoverage]
        private static int NumOfLetters(char c)
        {
            var s = c.ToString();
            if ("ζξχ".Contains(s)) return 1;
            else if ("πμκβθφψ".Contains(s)) return 2;
            else if ("δσςυλ".Contains(s)) return 4;
            else if ("γ".Contains(s)) return 3;
            else if ("νρτ".Contains(s)) return 6;
            else if ("οω".Contains(s)) return 8;
            else if ("αι".Contains(s)) return 9;
            else if ("εη".Contains(s)) return 12;
            else return 0;
        }

        public class WordGroup
        {
            public string Word { get; set; }
            public int Frequency { get; set; }
            public string Definition { get; set; }
            public string Conjugations { get; set; }
            public string Cleaned { get; set; }
            public override string ToString()
            {
                return $"{Word}: {Definition}";
            }
        }
        private static IEnumerable<WordGroup> _words;
        public static IEnumerable<string> GetWords()
        {
            if (_words == null || !_words.Any())
            {
                _words = File.ReadAllLines(@"Model\Word\BBG Vocabulary.txt")
                .Skip(1)
                .Where(x => x.Trim().Length > 0)
                .Select(x => new WordGroup
                {
                    Word = x.Split('\t')
                        .Skip(1)
                        .First()
                    .Split(',')
                        .First()
                        .Trim(), 
                    Cleaned = RemoveDiacritics(x.Split('\t')
                        .Skip(1)
                        .First()
                    .Split(',')
                        .First()
                        .Trim()),
                    Definition = x.Split('\t').Skip(2).First(),
                    Conjugations = x.Split('\t').Skip(1).First(),
                    Frequency = int.Parse(x.Split('\t').First())
                }
                );
            }
            return _words.Select(x => x.Word);
            //Alternatively, use RemoveDiacritics when loading the word list then do a straight comparison
        }

        public static string Definition(string word)
        {
            return _words.FirstOrDefault(x => x.Cleaned == word)?.Definition;
        }
        public static int? Frequency(string word)
        {
            return _words.FirstOrDefault(x => x.Cleaned == word)?.Frequency;
        }
        public static string Conjugations(string word)
        {
            return _words.FirstOrDefault(x => x.Cleaned == word)?.Conjugations;
        }
        public static WordGroup Information(string word)
        {
            return _words.FirstOrDefault(x => x.Cleaned == word);
        }

        public static IEnumerable<string> Cheat(IEnumerable<char> letters)
        {
            return GetWords()
                .Where(x => !RemoveDiacritics(x).ToCharArray().Any(c => !letters.Contains(c)));
            //This works too:
            //.Where(x => RemoveDiacritics(x).ToCharArray().Count(c => letters.Contains(c)) == x.Length);
        }

        public new static bool WordEqual(string s1, string s2, bool strict)
        {
            if (strict)
                return s1.Equals(s2, StringComparison.OrdinalIgnoreCase);
            else
                return string.Compare(s1, s2, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace) == 0;
            
            //This works too but is likely less efficient
            //return RemoveDiacritics(s1).Equals(RemoveDiacritics(s2));
        }

        public static string RemoveDiacritics(string text)
        {
            return string.Concat(
                text.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) !=
                                              UnicodeCategory.NonSpacingMark)
              ).Normalize(NormalizationForm.FormC);
        }
    }
}
