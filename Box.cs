using System;
using System.Collections;
using System.Collections.Generic;

namespace HochhausSolver
{

        class Box
    {
        protected Boolean[] Possible = new bool[10];

        public int Solution;

        public Boolean Solved = false;

        protected readonly int size;


        public List<int> Options
        {
            get {
                
                    List<int> result = new List<int>();
                    for (int n = 1; n <= size; n++)
                        if (IsPossible(n))
                            result.Add(n);
                    return result;
                } }

        public Boolean Valid()
        {
            foreach (Boolean b in Possible)
                if (b) return true;
            return false;
        }

        public int GapValue(int GapFrom)
        {
            int result = 0;
            for (int i = GapFrom; i <= size; i++)
                if (!IsPossible(i))
                    result += (int)Math.Pow(10, size - GapFrom+1);
            return result;
        }

        public int SolveUniqueInBox()
        {
            if (!Solved && GetMin() == GetMax())
            {
                Solve(GetMin());
                return 1;
            }
            return 0;
        }

        public void Solve(int result)
        {
            if (result < 1 || result > size) return;
            Solution = result;
            Solved = true;
            for( int n = 0; n <= size-1; n++)
            {
                Possible[n] = false;
            }
            Possible[result-1] = true;
        }

        public int GetMax(int max = 99)
        {
            if (max == 99) max = size;
            for (int n = max; n > 0; n--)
            {
                if (IsPossible(n)) return n;
            }
            return size;
        }

        public int GetMin(int least = 1)
        {
            for (int n = least; n <= size; n++)
            {
                if (IsPossible(n)) return n;
            }
            return 0;
        }


        public int GetClosestAbove(int max)
        {
            for (int n = max + 1; n <= size; n++)
            {
                if (IsPossible(n)) return n;
            }
            return 0;
        }

        public int MaxPossible(int max)
        {
            int result = 0;
            for (int i = max + 1; i <= size; i++)
            {
                result += Uncheck(i);
            }
            return result;
        }

        public int MaxPossibleButHighest(int max)
        {
            int result = 0;
            for (int i = max + 1; i <= size-1; i++)
            {
                result += Uncheck(i);
            }
            return result;

           
        }

        public int MinPossible(int min)
        {
            int result = 0;
            for (int i = 1; i < min; i++)
            {
                result += Uncheck(i);
            }
            return result;
        }

        public int Uncheck(int number)
        {
            if ( Solution != number && number >= 1 && number <= size && Possible[number - 1])
            {
                Possible[number-1] = false;
                return 1;
            }
            return 0;
        }

        public Boolean IsPossible(int i)
        {
            if (Solution == i) return true;
            if (i >= 1 && i <= size) return Possible[i-1];
            return false;
        }


        public Boolean IsHighestPossible()
        {
            return IsPossible(size);
        }

        public Boolean IsHighestPossibleSolved()
        {
            return IsPossible(size) && Solved;
        }

        public Box(int c)
        {
            size = c;
            for (int i = 0; i <= size-1; i++)
            {
                Possible[i] = true;
            }
        }


        public int Count()
        {
                return Options.Count;
        }

        public String GetLine(int line)
        {
            if (Solved)
            {

                if (line == 2) 
                    if (Solution >= 10) 
                        return "*" + Solution.ToString(); 
                    else  
                        return "*" + Solution.ToString() + "*";
                else return "***";
            }

            String s = "";
            if (line == 4)
            {
                s += " ";
                if (IsPossible(10)) s += 10;
                else s += "  ";
            }
            else
                for (int i = 1; i <= 3; i++)
                    if (IsPossible((line - 1) * 3 + i)) s += ((line - 1) * 3 + i).ToString(); else s += " ";
            return s;
        }
    

    public Boolean Matches(BitArray ar)
        {
            // Wenn ein Testbit nicht möglich ist, dann stimmt das Pattern nicht überein
            for(int cnt = 0; cnt <= size-1;cnt++)
            {
                if (ar.Get(cnt) && !Possible[cnt]) return false;
            }

            return true;
        }
        public Boolean MatchesNot(BitArray ar)
        {
            // wenn kein Testbit möglich ist, dann passt das Pattern nicht  -  true
            // Sobald ein Testbit möglich ist, dann passt das Pattern nicht nicht   -  false
            for (int cnt = 0; cnt <= size - 1; cnt++)
            {
                if (ar.Get(cnt) && Possible[cnt]) return false;
            }

            return true;
        }
    }
}
