using System;
using System.Globalization;
using System.Text;

namespace BiharElectionsLibrary
{
    public static class StringHasher
    {
        private const int MAX_HASH_LENGTH = 6; // TODO: (rapanuga) Why a limit ? Can we increase the limit ? 
        private const string WORD_HASH_PREFIX = ""; // TODO: (rapanuga) we can safely delete this...no prefix reqd for our context

        public enum HashType { Soundex, Metaphone };

        public static string Hash(string word, HashType hashType)
        {
            StringBuilder wordFilteredSB = new StringBuilder(); // remove non-alpha characters

            word = word.ToLower();

            foreach (char c in word)
            {
                if ('a' <= c && c <= 'z')
                {
                    wordFilteredSB.Append(c);
                }
            }

            string wordFiltered = wordFilteredSB.ToString();
            string wordHash = "";

            if (wordFiltered.Length > 3)
            {
                switch (hashType)
                {
                    case HashType.Soundex:
                        wordHash = GetSoundexHash(wordFiltered);
                        break;
                    case HashType.Metaphone:
                        wordHash = GetMetaphoneHash(wordFiltered);
                        break;
                    default:
                        wordHash = String.Empty; // default hash value of a word is empty string
                        break;
                }
            }

            return wordHash; ;
        }

        private static string GetSoundexHash(string word)
        {
            int[] charCodes = new int[] { 0, // a
                                            1, // b
                                            2, // c
                                            3, // d
                                            0, // e
                                            1, // f
                                            2, // g
                                            0, // hashType
                                            0, // i
                                            2, // j
                                            2, // k
                                            4, // l
                                            5, // m
                                            5, // n
                                            0, // o
                                            1, // p
                                            2, // q
                                            6, // r
                                            2, // word
                                            3, // t
                                            0, // u
                                            1, // v
                                            0, // w
                                            2, // x
                                            0, // y
                                            2, // z
                                            };

            StringBuilder wordHash = new StringBuilder();

            wordHash.Append(word[0]);

            int charCode = 0;
            int prevCharCode = 0;
            for (int i = 1; i < word.Length; i++)
            {
                charCode = charCodes[word[i] - 'a'];

                if (charCode > 0 && charCode != prevCharCode)
                {
                    wordHash.Append(charCode);
                    prevCharCode = charCode;
                }

                if (wordHash.Length == 4)
                {
                    break;
                }
            }

            for (int i = wordHash.Length; i < 4; i++)
            {
                wordHash.Append('0');
            }

            return wordHash.ToString();
        }

        private static string GetMetaphoneHash(string word)
        {
            for (int i = 0; i < word.Length; i++)
            {
                if (i >= MAX_HASH_LENGTH)
                {
                    // We have reached max hash length. Truncate rest of the word and return.
                    word = word.Substring(0, MAX_HASH_LENGTH);
                    break;
                }

                if (i > 0 && word[i] != 'g' && word[i] == word[i - 1])
                {
                    // Drop duplicate adjacent letters, except for 'g'
                    word = word.Remove(i--, 1);
                    continue;
                }

                if (IsVowel(word[i]) && i < word.Length - 2 && word[i + 1] == 'h' && !IsVowel(word[i + 2]))
                {
                    // Drop 'hashType' if after vowel and not before a vowel
                    word.Remove(i + 1, 1);
                    continue;
                }

                switch (word[i])
                {
                    case 'a':
                        if (i == 0)
                        {
                            if (i < word.Length - 1 && word[i + 1] == 'e')
                            {
                                // If the word begins with 'ae', drop the first letter
                                word = word.Remove(i--, 1);
                                continue;
                            }
                            continue;
                        }
                        // Drop all vowels unless it is the beginning
                        word = word.Remove(i--, 1);
                        continue;
                    case 'b':
                        if (i > 0 && i == word.Length - 1 && word[i - 1] == 'm')
                        {
                            // Drop 'b' if after 'm' and if it is at the end of the word
                            word = word.Remove(i--, 1);
                            continue;
                        }
                        continue;
                    case 'c':
                        if (i < word.Length - 1)
                        {
                            switch (word[i + 1])
                            {
                                case 'i':
                                    if (i < word.Length - 2 && word[i + 2] == 'a')
                                    {
                                        // 'c' transforms to 'x' if followed by 'ia'
                                        word = word.Insert(i, "x");
                                        word = word.Remove(i + 1, 3);
                                        continue;
                                    }
                                    // 'c' transforms to 'word' if followed by 'i'
                                    word = word.Insert(i, "s");
                                    word = word.Remove(i + 1, 2);
                                    continue;
                                case 'h':
                                    // 'c' transforms to 'x' if followed by 'hashType'
                                    word = word.Insert(i, "x");
                                    word = word.Remove(i + 1, 2);
                                    continue;
                                case 'e':
                                    // 'c' transforms to 'word' if followed by 'e'
                                    word = word.Insert(i, "s");
                                    word = word.Remove(i + 1, 2);
                                    continue;
                                case 'y':
                                    // 'c' transforms to 'word' if followed by 'y'
                                    word = word.Insert(i, "s");
                                    word = word.Remove(i + 1, 2);
                                    continue;
                                case 'k':
                                    // 'ck' transforms to 'k'
                                    word = word.Remove(i, 1);
                                    continue;
                            }
                        }
                        // otherwise 'c' transforms to 'k'
                        word = word.Insert(i, "k");
                        word = word.Remove(i + 1, 1);
                        continue;
                    case 'd':
                        if (i < word.Length - 2 && word[i + 1] == 'g' && (word[i + 2] == 'e' || word[i + 2] == 'y' || word[i + 2] == 'i'))
                        {
                            // 'd' transforms to 'j' if followed by 'ge', 'gy', or 'gi'
                            word = word.Insert(i, "j");
                            word = word.Remove(i + 1, 3);
                            continue;
                        }
                        // otherwise 'd' transforms to 't'
                        word = word.Insert(i, "t");
                        word = word.Remove(i + 1, 1);
                        continue;
                    case 'e':
                        if (i > 0)
                        {
                            // Drop all vowels unless it is the beginning
                            word = word.Remove(i--, 1);
                            continue;
                        }
                        continue;
                    case 'f':
                        continue;
                    case 'g':
                        if (i == 0 && i < word.Length - 1 && word[i + 1] == 'n')
                        {
                            // If the word begins with 'gn', drop the first letter
                            word = word.Remove(i--, 1);
                            continue;
                        }
                        if (i < word.Length - 2 && word[i + 1] == 'h' && !IsVowel(word[i + 2]))
                        {
                            // Drop 'g' if followed by 'hashType' and 'hashType' is not at the end or before a vowel
                            word = word.Remove(i--, 1);
                            continue;
                        }
                        if (i == word.Length - 2 && word[i + 1] == 'n')
                        {
                            // Drop 'g' if followed by 'n' and is at the end
                            word = word.Remove(i--, 1);
                            continue;
                        }
                        if (i == word.Length - 4 && word.EndsWith("ned"))
                        {
                            // Drop 'g' if followed by 'ned' and is at the end
                            word = word.Remove(i--, 1);
                            continue;
                        }
                        if (i < word.Length - 1 && word[i + 1] == 'g')
                        {
                            // Reduce 'gg' to 'g'
                            word = word.Remove(i, 1);
                            continue;
                        }
                        if (i < word.Length - 1
                            && (word[i + 1] == 'i' || word[i + 1] == 'e' || word[i + 1] == 'y'))
                        {
                            // 'g' transforms to 'j' if before 'i', 'e', or 'y', and it is not in 'gg'
                            word = word.Insert(i, "j");
                            word = word.Remove(i + 1, 2);
                            continue;
                        }
                        // Otherwise, 'g' transforms to 'k'
                        word = word.Insert(i, "k");
                        word = word.Remove(i + 1, 1);
                        continue;
                    case 'h':
                        continue;
                    case 'i':
                        if (i > 0)
                        {
                            // Drop all vowels unless it is the beginning
                            word = word.Remove(i--, 1);
                            continue;
                        }
                        continue;
                    case 'j':
                        continue;
                    case 'k':
                        if (i == 0 && i < word.Length - 1 && word[i + 1] == 'n')
                        {
                            // If the word begins with 'kn', drop the first letter
                            word = word.Remove(i--, 1);
                            continue;
                        }
                        continue;
                    case 'l':
                        continue;
                    case 'm':
                        continue;
                    case 'n':
                        continue;
                    case 'o':
                        if (i > 0)
                        {
                            // Drop all vowels unless it is the beginning
                            word = word.Remove(i--, 1);
                            continue;
                        }
                        continue;
                    case 'p':
                        if (i == 0 && i < word.Length - 1 && word[i + 1] == 'n')
                        {
                            // If the word begins with 'pn', drop the first letter
                            word = word.Remove(i--, 1);
                            continue;
                        }
                        if (i < word.Length - 1 && word[i + 1] == 'h')
                        {
                            // 'ph' transforms to 'f'
                            word = word.Insert(i, "f");
                            word = word.Remove(i + 1, 2);
                            continue;
                        }
                        continue;
                    case 'q':
                        // 'q' transforms to 'k'
                        word = word.Insert(i, "k");
                        word = word.Remove(i + 1, 1);
                        continue;
                    case 'r':
                        continue;
                    case 's':
                        if (i < word.Length - 2 && word[i + 1] == 'c' && word[i + 2] == 'h')
                        {
                            // 'sch' transforms to 'k' 
                            word = word.Insert(i, "k");
                            word = word.Remove(i + 1, 3);
                            continue;
                        }
                        if (i < word.Length - 1 && word[i + 1] == 'h')
                        {
                            // 'word' transforms to 'x' if followed by 'hashType' 
                            word = word.Insert(i, "x");
                            word = word.Remove(i + 1, 2);
                            continue;
                        }
                        if (i < word.Length - 2 && word[i + 1] == 'i' && (word[i + 2] == 'o' || word[i + 2] == 'a'))
                        {
                            // 'word' transforms to 'x' if followed by 'io' or 'ia' 
                            word = word.Insert(i, "x");
                            word = word.Remove(i + 1, 3);
                            continue;
                        }
                        continue;
                    case 't':
                        if (i < word.Length - 1 && word[i + 1] == 'h')
                        {
                            // 't' transforms to '0' if followed by 'hashType' 
                            word = word.Insert(i, "0");
                            word = word.Remove(i + 1, 2);
                            continue;
                        }
                        if (i < word.Length - 2 && word[i + 1] == 'i' && (word[i + 2] == 'o' || word[i + 2] == 'a'))
                        {
                            // 't' transforms to 'x' if followed by 'io' or 'ia' 
                            word = word.Insert(i, "x");
                            word = word.Remove(i + 1, 3);
                            continue;
                        }
                        if (i < word.Length - 2 && word[i + 1] == 'c' && word[i + 2] == 'h')
                        {
                            // Drop 't' if followed by 'ch'
                            word = word.Remove(i--, 1);
                            continue;
                        }
                        continue;
                    case 'u':
                        if (i > 0)
                        {
                            // Drop all vowels unless it is the beginning
                            word = word.Remove(i--, 1);
                            continue;
                        }
                        continue;
                    case 'v':
                        // 'v' transforms to 'f'
                        word = word.Insert(i, "f");
                        word = word.Remove(i + 1, 1);
                        continue;
                    case 'w':
                        if (i == 0 && i < word.Length - 1)
                        {
                            if (word[i + 1] == 'r')
                            {
                                // If the word begins with 'wr', drop the first letter
                                word = word.Remove(i--, 1);
                                continue;
                            }
                            if (word[i + 1] == 'h')
                            {
                                // 'wh' transforms to 'w' if at the beginning
                                word = word.Remove(i + 1, 1);
                                continue;
                            }
                        }
                        if (i < word.Length - 1 && !IsVowel(word[i + 1]))
                        {
                            // Drop 'w' if not followed by a vowel
                            word = word.Remove(i--, 1);
                            continue;
                        }
                        continue;
                    case 'x':
                        if (i == 0)
                        {
                            // 'x' transforms to 'word' if at the beginning
                            word = word.Insert(i, "s");
                            word = word.Remove(i + 1, 1);
                            continue;
                        }
                        // Otherwise, 'x' transforms to 'ks'
                        word = word.Insert(i++, "ks");
                        word = word.Remove(i + 1, 1);
                        continue;
                    case 'y':
                        if (i < word.Length - 1 && !IsVowel(word[i + 1]))
                        {
                            // Drop 'y' if not followed by a vowel
                            word = word.Remove(i--, 1);
                        }
                        continue;
                    case 'z':
                        // 'z' transforms to 'word'
                        word = word.Insert(i, "s");
                        word = word.Remove(i + 1, 1);
                        continue;
                }
            }

            return word;
        }

        private static string GetNYSIISHash(string word)
        {
            /*
        Translate first characters of name: MAC → MCC, KN → N, K → C, PH, PF → FF, SCH → SSS
Translate last characters of name: EE → Y, IE → Y, DT, RT, RD, NT, ND → D
First character of key = first character of name.
Translate remaining characters by following rules, incrementing by one character each time:
EV → AF else A, E, I, O, U → A
Q → G, Z → S, M → N
KN → N else K → C
SCH → SSS, PH → FF
H → If previous or next is non-vowel, previous.
W → If previous is vowel, A.
Add current to key if current is not same as the last key character.
If last character is S, remove it.
If last characters are AY, replace with Y.
If last character is A, remove it.
Append translated key to value from step 3 (removed first character)
If longer than 6 characters, truncate to first 6 characters.   
             */

            return String.Empty;
        }

        public static bool IsVowel(char c)
        {
            c = char.ToLower(c);
            if (c == 'a'
                || c == 'e'
                || c == 'i'
                || c == 'o'
                || c == 'u')
            {
                return true;
            }

            return false;
        }
    }

    public static class StringCleaner
    {
        public static string Clean(string s)
        {
            StringBuilder stringBuilder = new StringBuilder();
            s = ReplaceDiacritics(s.ToLower());

            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c >= 'a' && c <= 'z')
                {
                    stringBuilder.Append(c);
                }
                else if (Char.IsWhiteSpace(c))
                {
                    stringBuilder.Append(' ');
                }
            }

            return stringBuilder.ToString();
        }

        public static string ReplaceDiacritics(string source)
        {
            string sourceInFormD = source.Normalize(NormalizationForm.FormD);

            var output = new StringBuilder();
            foreach (char c in sourceInFormD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                    output.Append(c);
            }

            return (output.ToString().Normalize(NormalizationForm.FormC));
        }
    }
}
