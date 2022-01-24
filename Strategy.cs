using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HochhausSolver
{
    class Square : Strategy
    {
        public Square()
        {
            Name = "Square";
        }
        protected override void SolveAction()
        {
            if (A.IsLine)
            {

                //Wähle eine Zahl
                for (int zahl = 1; zahl <= A.size; zahl++)
                {
                    foreach(Area line1 in A.F.Lines)
                    {
                        if(line1.Count(zahl) == 2)
                            foreach(Area line2 in A.F.Lines)
                            {
                                if (line1 == line2) continue;
                                if(line1.StringPattern(zahl) == line2.StringPattern(zahl))
                                {
                                    for (int i = 1; i <= A.size; i++)
                                    {
                                        if (line1.Pattern(zahl)[i - 1])
                                        {
                                            foreach (Area a in A.F.Lines)
                                            {
                                                if (!(a.Item(i) == line1.Item(i) || a.Item(i) == line2.Item(i))) Actionsize += a.Item(i).Uncheck(zahl);
                                            }
                                        }
                                    }

                                }
                            }
                    }
                }
                //Wähle eine Zeile
                //Bilde den Vektor
                //Bilde die UND-Verknüpfung mit anderen Zeillen
                //Wenn Anzahl Bits und Anzahl verknüpfter Zeilen gleich, dann isses das
            }
        }
    }

    class Impossible : Strategy
    {
        public Impossible()
        {
            Name = "Impossible";
        }
        protected override void SolveAction()
        {
            if (!A.Valid())
            {
                notpossible = true;
                Actionsize = 1;
            }
                return;
        }
    }
    class Testvectors : Strategy
    {
        public Testvectors()
        {
            Name = "Testvectors";
        }
        protected override void SolveAction()
        {
            if ((A.HasView || A.ReverseArea.HasView) && A.IsMainArea) {

                for (int position = 1; position <= A.size; position++)
                {
                    Testvektor t = new Testvektor(A.size);
                    //for (int i = 1; i <= A.size; i++)
                    //    if (A.Item(i).Solved)
                    //        t.Set(i, A.Item(i).Solution);
                    t.A = A;
                    t.dynamic = 1;
                    
                    foreach (int versuch in A.Item(position).Options)
                    {
                        t.Set(position, versuch);

                        if (!t.Check())
                        {
                            Actionsize += A.Item(position).Uncheck(versuch);
                            //return;
                        }
                    }
                }
            }
        }
    }



        class CheckForGroup : Strategy
    {
        public CheckForGroup()
        {
            Name = "Check for closed groups";
        }
        protected override void SolveAction()
        {
            if (!A.IsMainArea) return;
            int bits = 2;
            int len = 2;
            int cnt;
            BitArray ba;

            while (len < 10)
            {
                bits++;
                ba = new BitArray(BitConverter.GetBytes(bits));
                len = countSetBits(bits);
                if (len == 1 || len >=5 ) continue;

                cnt = A.size;
                foreach(Box bo in A.Items)
                {
                    if (bo.MatchesNot(ba)) cnt--;  
                    if (cnt < len) break;
                }
                
                if (cnt == len )
                {
                    foreach (Box bo in A.Items)
                    {
                        if (!bo.MatchesNot(ba))
                        {
                            for (int i = 0; i <= A.size - 1; i++)
                                if (!ba.Get(i))
                                {
                                    Actionsize += bo.Uncheck(i +1);
                                }
                        }   
                    }
                }
            }
        }

        static int countSetBits(int n)
        {
            int count = 0;
            while (n > 0)
            {
                count += n & 1;
                n >>= 1;
            }
            return count;
        }
    }



    class CheckAllHighest : Strategy
    {
        public CheckAllHighest()
        {
            Name = "Check if the highest Number can be that late";
        }
        protected override void SolveAction()
        {
            if (A.HasView)
            {
                
                for (int checkitem = 1; checkitem <= A.size;checkitem++  )
                {
                    int col = 0;
                    int cnt = 0;
                    int min = 0;
                    int checknumber = 0;
                    foreach (Box b in A.Items)
                    {
                        col++;
                        if (col < checkitem)
                        {
                            cnt++;
                        }
                        else
                        if (col == checkitem)
                        {
                            min = b.GetMax();
                            checknumber = min;
                            cnt++;
                        }
                        else
                        {
                            if (b.GetMin(min+1) > 0)
                            {
                                min = b.GetMin(min+1);
                                cnt++;
                            }
                        }
                    }
                    if (cnt < A.ViewNumber)
                    {
                        Actionsize += A.Item(checkitem).Uncheck(checknumber);
                    }

                }

            }    
        }
    }
    class KnownFieldCounter_Reverse : Strategy
    {
        public KnownFieldCounter_Reverse()
        {
            Name = "Check if the highest Number can be that late";
        }
        protected override void SolveAction()
        {
            if (A.HasView)
            {
                int cnt = 0;
                int max = 0;
                
                foreach (Box B in A.Items)
                {
                    if (cnt == 0)
                    {
                        cnt++;
                        max = B.GetMin();
                    }
                    if (B.Solved && B.Solution > max && cnt < A.ViewNumber)
                    {
                        cnt++;
                        max = B.Solution;
                    }

                    if (!B.Solved && cnt >= A.ViewNumber)
                    {
                        B.MaxPossible(9);
                    }

                }
            }
        }
    }

    class StarlLowRemove10 : Strategy
    {
        public StarlLowRemove10()
        {
            Name = "Start low to check if 10 is possible";
        }
        protected override void SolveAction()
        {
            if (A.HasView)
            {
                int min = 0;
                int cnt = 0;
                foreach(Box B in A.Items)
                {
                    if (B.GetMin(min+1) > min)
                    {
                        min = B.GetMin(min+1);
                        cnt++;
                    }
                    if (cnt < A.ViewNumber) { B.MaxPossible(9); }
                }
            }
        }
    }


    class AscendingOrder : Strategy
    {
        public AscendingOrder()
        {
            Name = "Check if the order must be ascending";
        }
        protected override void SolveAction()
        {
            if (A.HasView)
            {
                List<Box> relevant = A.ItemsUntilSolvedHighest();

                if (relevant.Count() == A.ViewNumber - 1 && A.ViewNumber > 2)
                {
                    int min = 0;
                    foreach(Box b in relevant)
                    {
                        Actionsize += b.MinPossible(min+1);
                        min = b.GetMin();
                    }
                }
            }
        }
    }


    class KnownFieldCounter : Strategy
    {
        public KnownFieldCounter()
        {
            Name = "Check if the lowest Number at the beginning is possible";
        }
        protected override void SolveAction()
        {
            if (A.ViewNumber > 2 )
            {
                
                int remaining = A.ViewNumber;
                int min = A.Item(1).GetMin();
                foreach (Box f in A.Items)
                {
                    if (remaining == A.ViewNumber) 
                        remaining--;
                    else
                        if (f.GetMin() > min)
                            {
                                remaining--;
                                min = f.GetMin();
                            }

                    if (remaining < 1)
                    { Actionsize += f.MaxPossible(9);
                        
                    }


                }


                    
            }
        }
    }
    class LowestStartPossibleOnTwo : Strategy
    {
        public LowestStartPossibleOnTwo()
        {
            Name = "Check if the lowest Number at the beginning is possible";
        }
        protected override void SolveAction()
        {
            

            if (A.ViewNumber==2)
            {
                if (A.Item(2).IsHighestPossible()) return;
                Box FirstItem = A.Item(1);
                int startmin = 99;
                List<Box> relevant2;
                List<Box> relevant = A.ItemsUntilFirstHighest();
                relevant.Remove(FirstItem);

                foreach (Box b in relevant)
                    if (startmin > b.GetMin()) startmin = b.GetMin();

                while (relevant.Count() > 0 && startmin < 10)
                {
                    startmin++;
                    relevant2 = new List<Box>();

                    foreach (Box b in relevant)
                        if (b.IsPossible(startmin))
                            relevant2.Add(b);

                    if (relevant2.Count() > 0)
                    {
                        relevant2.Sort(delegate (Box x, Box y)
                            {
                                if (x.GapValue(startmin) < x.GapValue(startmin)) return -1;
                                else if (x.GapValue(startmin) > x.GapValue(startmin)) return 1;
                                else return 0;
                            });


                        relevant.Remove(relevant2.First());                       
                    }
                    
                }
                Actionsize += FirstItem.MinPossible(startmin);

            }
        }
    }

    class HighestStartPossible : Strategy
    {
        public HighestStartPossible()
        {
            Name = "Check if the highest Number at the beginning is possible";
        }
        protected override void SolveAction()
        {
            int count = 0;
            int donecount = 0;
            int currentmax = 0;
            int startmax = 0;
            int skip = 0;
            
            if (A.HasView)
            {
                foreach (Box start in A.ItemsUntilLastHighest())
                {
                    currentmax = start.GetMax();
                    if (startmax == 0) startmax = currentmax;
                    else if (start.GetClosestAbove(currentmax) == 0) continue; else startmax = start.GetClosestAbove(currentmax);

                    donecount++;
                    count = donecount;
                    skip++;
                    
                    foreach(Box seq in A.ItemsUntilLastHighest().Skip(skip))
                    {
                        int next = seq.GetClosestAbove(currentmax);
                        if (next > 0)
                        {
                            count++;
                            currentmax = next;
                        }
                    }
                    if (count < A.ViewNumber)
                    {
                        Actionsize += start.Uncheck(startmax);
                    }
                    
                }
            }
        }
    }

    class DistanceToBorders : Strategy
    {
        public DistanceToBorders() {
            Name = "Clean big numbers close to the border";
            }
        protected override void SolveAction()
        {
            // Wenn eine zwei vorne steht, entferne bis einschließlich zur ersten 10 den größten Wert der ersten Box
            if (A.HasView)
            {
                for (int n = 1; n < A.ViewNumber; n++)
                {
                    Actionsize += A.Item(n).MaxPossible(A.size - A.ViewNumber + n);
                }
            }
        }
    }

    class SecondTwo : Strategy
    {
        public SecondTwo()
        {
            Name = "If the viewnumber is two, remove the max until the highest";
        }
        protected override void SolveAction()
        {
            // Wenn eine zwei vorne steht, entferne bis einschließlich zur ersten 10 den größten Wert der ersten Box
            if (A.ViewNumber == 2)
            {
                int max = A.Item(1).GetMax();
                for (int n = 2; n < A.size; n++)
                {
                    Actionsize += A.Item(n).MaxPossibleButHighest(max - 1);
                    if (A.Item(n).IsHighestPossible()) break;
                }
            }
        }
    }


   
    class UniqueInBox : Strategy
    {
        public UniqueInBox()
        {
            Name = "If just one option remains in an box, this is it.";
        }
        protected override void SolveAction()
        {
            foreach (Box B in A.UnsolvedItems())
            {
                Actionsize += B.SolveUniqueInBox();
            }
        }
    }

    class UniqueInArea : Strategy
    {
        public UniqueInArea() {
            Name = "If an option is just once in the area, this is the solution"; 
        }

        protected override void SolveAction() {
                for  (int loesung = 1; loesung <= A.size; loesung++)
                {
                // Elemente finden, bei denen loesung eine zulässige Lösung ist
                List<Box> Versuch = A.UnsolvedItems().Where(x => x.IsPossible(loesung)).ToList();
                if (Versuch.Count == 1) { 
                        Versuch[0].Solve(loesung);
                        Actionsize ++;
                    }
                }
            
        }
    }

    class RemoveSolvedNumbers : Strategy
    {
        public RemoveSolvedNumbers()
        {
            Name = "Remove solved numbers from line";
        }
        protected override void SolveAction()
        {
            foreach (Box geloest in A.SolvedItems())
            {
                foreach (Box R in A.Items)
                {
                    Actionsize += R.Uncheck(geloest.Solution);
                }
            }
        }
    }





    abstract class Strategy
        
        {
        public int Actionsize = 0;
        public int Ticks = 0;

        public int TotalActionsize = 0;
        public int TotalTicks = 0;

        public String Name = "noch kein Name";

        public Boolean notpossible = false;

        public Area A;

        


        public Strategy() { }

        protected abstract void SolveAction();

        public void Print()
        {
            Console.WriteLine((TotalActionsize.ToString() + "        ").Substring(0, 6) + Name);
        }

        public int Solve(Area a)
        {
            A = a;
            Actionsize = 0;
            Ticks = 0;
            SolveAction();
            if (Actionsize > 0)
                Console.WriteLine(Name + ": " + Actionsize.ToString() + " Aktionen");
            TotalActionsize += Actionsize;
            TotalTicks += Ticks;
            return Actionsize;
    }

    }
}
