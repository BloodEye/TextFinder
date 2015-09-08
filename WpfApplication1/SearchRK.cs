using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using Xceed.Wpf.Toolkit;

namespace TextFinder
{
    class SearchRK
    {
        private String[] pat = null;      // the pattern  // needed only for Las Vegas
        private long[] patHash;    // pattern hash value
        private int M;           // pattern length
        private long Q;          // a large prime, small enough to avoid long overflow
        private int R;           // radix
        private long RM;         // R^(M-1) % Q
        private bool IgnoreCase;    //flag to Ignore Case Sensitive or not

        public SearchRK(String[] pat, bool IgnoreCase = false)
        {  
            R = 256;
            M = pat[0].Length;
            Q = RandomPrime();
            this.IgnoreCase = IgnoreCase;

            this.pat = pat;
            for (int i = 0; i < this.pat.Length; i++)
                this.pat[i] = this.IgnoreCase ? this.pat[i].Trim().ToLower() : this.pat[i].Trim();
   
            // precompute R^(M-1) % Q for use in removing leading digit
            RM = 1;
            for (int i = 1; i <= M - 1; i++)
                RM = (R * RM) % Q;

            this.patHash = new long[this.pat.Length];

            //hash all pattern
            for (int i = 0; i < this.pat.Length; i++)
                this.patHash[i] = hash(this.pat[i], this.M);
        }

        // Compute hash for key[0..M-1]. 
        private long hash(String key, int M)
        {
            long h = 0;
            for (int j = 0; j < M; j++)
                h = (R * h + key[j]) % Q;

            return h;
        }

        // Las Vegas version: does pat[] match txt[i..i-M+1] ?
        private Boolean check(String txt, int M, String pat, int i)
        {
            for (int j = 0; j < M; j++)
                if (pat[j] != txt[i + j])
                    return false;
            return true;
        }

        // Monte Carlo version: always return true
        private Boolean check(int i)
        {
            return true;
        }

        // check for exact match
        public Result[] search(String txt, bool all = false)
        {
            if (this.pat.Length == 0)
                return null;
            
            String txtLocal = this.IgnoreCase ? txt.Trim().ToLower() : txt.Trim();
            int N = txtLocal.Length;

            List<Result> result = new List<Result>();

            if (N < M)
                return null;

            long txtHash = hash(txtLocal, M);

            // check for match at offset 0
            if (Array.IndexOf(patHash, txtHash) != -1)
            {
                String[] sample = getSample(0, txt, M);
                //String[] sample = new string[] { "", "", "" };

                result.Add(new Result() { Line = 1, Index = 0, Length = M, TextStart = sample[0], Pattern = sample[1], TextEnd = sample[2] });


                if (!all)
                    return result.ToArray();
            }

            // check for hash match; if hash match, check for exact match
            for (int i = M; i < N; i++)
            {
                // Remove leading digit, add trailing digit, check for match. 
                txtHash = (txtHash + Q - RM * txtLocal[i - M] % Q) % Q;
                txtHash = (txtHash * R + txtLocal[i]) % Q;
                
                //check hash in hash array
                if (Array.IndexOf(patHash, txtHash) != -1) 
                {
                    // match
                    int offset = i - M + 1;

                    String[] sample = getSample(offset, txt, M);
                    result.Add(new Result() { Line = getLine(offset, txtLocal), Index = offset, Length = M, TextStart = sample[0], Pattern = sample[1], TextEnd = sample[2] });

                    if (!all)
                        return result.ToArray();
                }
            }

            return result.Count > 0 ? result.ToArray() : null;
        }

        // a random 31-bit prime
        private int RandomPrime()
        {
            Random rand = new Random();
            int randNumber = 0;

            do
            {
                randNumber = rand.Next(0, Int32.MaxValue - 1);
            } while (!isPrime(randNumber));

            return randNumber;
        }

        //check primability
        private Boolean isPrime(int Number)
        {
            if (Number <= 3)
            {
                return Number > 1;
            }
            else if (Number % 2 == 0 || Number % 3 == 0)
            {
                return false;
            }
            else
            {
                for (int i = 5; i * i <= Number; i += 6)
                {
                    if (Number % i == 0 || Number % (i + 2) == 0)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        private int getLine(int pos, string text)
        {
            return text.Substring(0, pos).Count(c => c == '\n') + 1; //get line number in position pos
        }

        private String[] getSample(int offset, String text, int patLength)
        {
            int start = 0, end = 30;

            if (offset != 0)
            {
                for (int i = offset - 1; i >= offset - 30; i--)
                {
                    if (i == 0)
                    {
                        start = i;
                        break;
                    }

                    if ((int)text[i] == 13 | (int)text[i] == 10)
                    {
                        start = i + 1;
                        break;
                    }

                    start = i;
                }
            }         

            if (text[start].Equals(" "))
            {
                start++;
            }

            int N = offset + patLength;

            if (offset != text.Length)
            {
                for (int i = N + 1; i <= N + 30; i++)
                {
                    if (i == text.Length)
                    {
                        end = text.Length - N - 1;
                        break;
                    }

                    if ((int)text[i] == 13 | (int)text[i] == 10)
                    {
                        end = i - N;
                        break;
                    }
                }
            }            

            if (text[N + end].Equals(" "))
            {
                end--;
            }

            String[] result = new String[3];
            result[0] = start > 0 ? "..." + text.Substring(start, offset - start).Replace("\r\n", string.Empty) : text.Substring(start, offset - start).Replace("\r\n", string.Empty);
            result[1] = text.Substring(offset, patLength).Replace("\r\n", string.Empty);
            result[2] = end < text.Length ? text.Substring(offset + patLength, end).Replace("\r\n", string.Empty) + "..." : text.Substring(offset + patLength, end).Replace("\r\n", string.Empty);

            return result;
        }
    }
}
