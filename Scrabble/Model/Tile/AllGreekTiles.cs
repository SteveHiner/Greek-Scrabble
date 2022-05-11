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
         * ?? tiles
         * 2 blank tiles
         */
        public static int ScoreOfLetter(char c)
        {
            var s = c.ToString();
            if ("αειονρ".Contains(s)) return 1;
            else if ("υωτγ".Contains(s)) return 2;
            else if ("ηλσςμπ".Contains(s)) return 3;
            else if ("θφ".Contains(s)) return 4;
            else if ("κβδ".Contains(s)) return 5;
            else if ("χζ".Contains(s)) return 8;
            else if ("ξψ".Contains(s)) return 10;
            else if ("-".Contains(s)) return 0;
            else return 0;
        }

        /*
         α: 881
         β: 71
         γ: 137
         δ: 147
         ε: 590
         ζ: 70
         η: 225
         θ: 101
         ι: 651
         κ: 248
         λ: 273
         μ: 299
         ν: 364
         ξ: 26
         ο: 674
         π: 321
         ρ: 380
         ς: 351
         σ: 280
         τ: 331
         υ: 273
         φ: 76
         χ: 100
         ψ: 12
         ω: 445

         α: 881
         ο: 674
         ι: 651
         ε: 590
         ω: 445
         ρ: 380
         ν: 364
         ς: 351
         τ: 331
         π: 321
         μ: 299
         σ: 280
         λ: 273
         υ: 273
         κ: 248
         η: 225
         δ: 147
         γ: 137
         θ: 101
         χ: 100
         φ: 76
         β: 71
         ζ: 70
         ξ: 26
         ψ: 12
        */

        [ExcludeFromCodeCoverage]
        private static int NumOfLetters(char c)
        {
            var s = c.ToString();
            if ("ξψ".Contains(s)) return 1; //2
            else if ("θχφβζ".Contains(s)) return 2; //10
            else if ("υκηδγ".Contains(s)) return 4; //20
            else if ("μσλ".Contains(s)) return 3; //9
            else if ("νςτπ".Contains(s)) return 6; //24
            else if ("εωρ".Contains(s)) return 8; //24
            else if ("οι".Contains(s)) return 9; //18
            else if ("α".Contains(s)) return 12; //12
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
                        .Trim()).ToLowerInvariant(),
                    Definition = x.Split('\t').Skip(2).First(),
                    Conjugations = x.Split('\t').Skip(1).First(),
                    Frequency = int.Parse(x.Split('\t').First())
                }
                );
                //var letters = GetLetterFrequency();
                //foreach (var item in letters.OrderByDescending(x => x.Value))
                //    System.Diagnostics.Debug.WriteLine($"{item.Key}: {item.Value}");
            }
            return _words.Select(x => x.Word);
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
        public static WordGroup GetWordGroup(string word)
        {
            return _words.FirstOrDefault(x => x.Cleaned == word);
        }

        public static IEnumerable<KeyValuePair<char, int>> GetLetterFrequency()
        {
            var everything = _words.Aggregate(new StringBuilder(), (result, x) => result.Append(x.Cleaned)).ToString();
            return everything.ToCharArray().GroupBy(x => x, (x, y) => new KeyValuePair<char, int>(x, y.Count()));
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
