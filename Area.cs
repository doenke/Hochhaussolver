using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HochhausSolver
{
    class Area
    {
        public List<Box> Items = new List<Box>();
        public int size;

        public int ViewNumber = 0;

        public Field F;
        

        public Area ReverseArea;

        public Boolean IsMainArea = true;
        public Boolean IsLine = true;

        public Boolean HasView { get { return ViewNumber != 0; } }

        public BitArray Pattern(int zahl)
        {
            BitArray result = new BitArray(size);
            for (int i = 1; i <= size; i++)
            {
                result[i - 1] = Item(i).IsPossible(zahl);
            }

            return result;
        }

        public int Count(int zahl)
        {
            int result = 0;
            for (int i = 1; i <= size; i++)
            {
                if (Item(i).IsPossible(zahl)) result++;
            }
            return result;
        }

    

    public String StringPattern(int zahl)
        {
            String result = "";
            for (int i = 1; i <= size; i++)
            {
                if (Item(i).IsPossible(zahl)) result += "1"; else result += "0";
            }
            return result;
        }

        public Boolean ContainsOption(int zahl)
        {
            foreach(Box b in Items)
            {
                if (b.IsPossible(zahl)) return true;
            }
            return false;
        }

        public Boolean Valid() {
            foreach (Box b in SolvedItems())
            {
                foreach (Box c in SolvedItems())
                {
                    if ( c.Solution == b.Solution && b!=c)
                        return false;
                }
            }
            int cnt = 0;
            int min = 0;
            foreach (Box b in Items) { 
                if (!b.Valid()) return false;

                if (HasView && b.GetMax() > min)
                {
                    min = b.GetMax();
                    cnt++;
                    if (cnt > ViewNumber) return false;
                }


            }




            return true;
        }

        public Box Item(int m)
        {
            return Items[m - 1];
        }



        public List<Box> SolvedItems()
        {
            return Items.Where(x => x.Solved).ToList();
        }

        public List<Box> UnsolvedItems()
        {
            return Items.Where(x => !x.Solved).ToList();
        }

        public List<Box> ItemsUntilLastHighest()
        {
            // create copy of List
            List<Box> result = Items.Select(item => item).ToList();
             while (result.Count() > 0 && !result.Last().IsHighestPossible()) {
                result.Remove(result.Last());
            }
            return result;
        }


        public List<Box> ItemsUntilFirstHighest()
        {
            List<Box> result = new List<Box>();
            foreach (Box b in Items)
            {
                if (b.IsHighestPossible())
                    return result;
                result.Add(b);
            }
            return result;
        }

        public List<Box> ItemsUntilSolvedHighest()
        {
            
            List < Box > result = new List<Box>();
            foreach (Box b in Items)
            {
                if (b.IsHighestPossibleSolved())
                    return result;
                result.Add(b);
            }
            return new List<Box>();
        }


        private int GetLastHighest()
        {
            for (int i = size; i >= 1; i--)
                if (Item(i).IsPossible(size)) return i;

            return 1;
        }

        public Area(Field f,int c)
        {
            F = f;
            size = c;
            for(int i = 1; i <= size;i++)
                Items.Add(new Box(size));
        }

        

        public void PrintLine()
        {
            String s = "";
            Console.WriteLine(string.Concat(Enumerable.Repeat("-", size * 6 + 8)));
            for (int i = 1; i <= 4; i++)
            {
                s = "";
                if (i == 2 && ViewNumber != 0)
                    s += " " + ViewNumber.ToString() + " ";
                else
                    s += "   ";

                for (int n = 1; n <= size; n++)
                {
                    s += "I ";
                    s += Item(n).GetLine(i);
                    s += " ";
                }
                s += "I";
                if (i == 2 && ReverseArea.ViewNumber != 0)
                    s += " " + ReverseArea.ViewNumber.ToString() + " ";
                Console.WriteLine(s);

            }
        }
    }

}
