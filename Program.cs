using System;
using System.Collections.Generic;
using System.Linq;

namespace HochhausSolver
{
    class Program
    {

        static void Main(string[] args)
        {
            Field F = new Field(10);
            F.Lines[1].ViewNumber = 4;
            F.Lines[2].ViewNumber = 2;
            F.Lines[4].ViewNumber = 8;
            F.Lines[5].ViewNumber = 3;
            F.Lines[7].ViewNumber = 6;
            F.Lines[8].ViewNumber = 6;

            F.ReverseLines[0].ViewNumber = 6;
            F.ReverseLines[2].ViewNumber = 5;
            F.ReverseLines[3].ViewNumber = 2;
            F.ReverseLines[6].ViewNumber = 7;
            F.ReverseLines[7].ViewNumber = 2;
            F.ReverseLines[9].ViewNumber = 4;

            F.Columns[0].ViewNumber = 2;
            F.Columns[2].ViewNumber = 7;
            F.Columns[5].ViewNumber = 8;
            F.Columns[6].ViewNumber = 2;
            F.Columns[9].ViewNumber = 5;

            F.ReverseColumns[3].ViewNumber = 7;
            F.ReverseColumns[4].ViewNumber = 6;
            F.ReverseColumns[5].ViewNumber = 2;
            F.ReverseColumns[7].ViewNumber = 4;
            F.ReverseColumns[8].ViewNumber = 5;
            F.ReverseColumns[9].ViewNumber = 2;

            F.FastStrategies.Add(new RemoveSolvedNumbers());
            F.FastStrategies.Add(new UniqueInArea());
            F.FastStrategies.Add(new UniqueInBox());

            F.FastStrategies.Add(new SecondTwo());
            F.FastStrategies.Add(new DistanceToBorders());
            F.FastStrategies.Add(new HighestStartPossible());
            F.FastStrategies.Add(new LowestStartPossibleOnTwo());
            //F.FastStrategies.Add(new KnownFieldCounter());
            //F.FastStrategies.Add(new AscendingOrder());
            //F.FastStrategies.Add(new StarlLowRemove10());
            //F.FastStrategies.Add(new KnownFieldCounter_Reverse());
            //F.FastStrategies.Add(new CheckAllHighest());
            F.FastStrategies.Add(new CheckForGroup());
            F.FastStrategies.Add(new Square());
            //F.FastStrategies.Add(new Impossible());

            F.SlowStrategies.Add(new Testvectors());
            //F.SlowStrategies.Add(new Impossible());

            F.AllStrategies.AddRange(F.FastStrategies);
            F.AllStrategies.AddRange(F.SlowStrategies);

            F.Solve();
            
            F.Print();


            foreach (Strategy S in F.AllStrategies)
            {
                 S.Print();               
            }

            Console.WriteLine("Aktionen: " + F.CountActions().ToString());

        }
    }


    class Field
    {
        public List<Area> Areas = new List<Area>();

        public List<Area> Lines = new List<Area>();
        public List<Area> ReverseLines = new List<Area>();

        public List<Area> Columns = new List<Area>();
        public List<Area> ReverseColumns = new List<Area>();

        public int size;

        public List<Strategy> AllStrategies = new List<Strategy>();
        public List<Strategy> FastStrategies = new List<Strategy>();
        public List<Strategy> SlowStrategies = new List<Strategy>();

        public int CountActions()
        {
            int result=0;
            foreach (Strategy S in AllStrategies)
                result += S.TotalActionsize;
            return result;
        }

        public void Check()
        {
            Boolean isvalid = true;
            foreach (Area A in Areas)
            {
                if (!A.Valid()) isvalid = false;
                break;
            }
            if (isvalid) Console.WriteLine("Alles OK"); else Console.WriteLine("!!!!!!!!!!!!!!!!!!!! Fehler !!!!!!!!!!!!!!!!!1"); 
        }
        public void Solve()
        {
            int DoneActions = 0;
            int DoneActionsTot = 0;
            do {
                DoneActionsTot = 0;

                do
                {
                    DoneActions = 0;

                    foreach (Area A in Areas)
                    {
                        foreach (Strategy S in FastStrategies)
                        {
                            int a = DoneActions;
                            DoneActions += S.Solve(A);
                            if (DoneActions != a) A.PrintLine();
                            if (S.notpossible)
                            {
                                return;
                            }
                        }
                    }
                    DoneActionsTot += DoneActions;
                } while (DoneActions != 0);
                Console.WriteLine("Done all fast strategies");

                do {
                    DoneActions = 0;
                    foreach (Area A in Areas)
                    {
                        foreach (Strategy S in SlowStrategies)
                        {
                            int a = DoneActions;
                            DoneActions += S.Solve(A);
                            if (DoneActions != a) A.PrintLine();
                            if (S.notpossible)
                            {
                                return;
                            }
                        }
                    }
                    DoneActionsTot += DoneActions;
                } while (DoneActions != 0);
                Console.WriteLine("Done all slow strategies");
                
            } while (DoneActionsTot != 0);
        }
        public void Print()
        {
            Console.WriteLine("");
            String s = "   ";
            foreach (Area c in Columns)
            {
                s += "   ";
                if (c.ViewNumber != 0)
                    s += c.ViewNumber.ToString();
                else s += " ";
                s += "  ";
            }
            Console.WriteLine(s);
            foreach (Area l in Lines)
            {
                l.PrintLine();
            }
            Console.WriteLine(string.Concat(Enumerable.Repeat("-", size * 6 + 8)));
            s = "   ";
            foreach (Area c in Columns)
            {
                s += "   ";
                if (c.ReverseArea.ViewNumber != 0)
                    s += c.ReverseArea.ViewNumber.ToString();
                else s += " ";
                s += "  ";
            }
            Console.WriteLine(s);
        }

        public Field(int rows)
        {

            size = rows;

            // i = Zeilen
            for (int i = 1; i <= rows; i++)
            {
                Area A = new Area(this,rows);
                Area R = new Area(this,rows);
                R.IsMainArea = false;
                A.ReverseArea = R;
                R.ReverseArea = A;
                A.IsLine = true;
                

                // n = Spalten
                for (int n = 0; n <= rows-1; n++)
                {
                    Box b = new Box(rows);
                    A.Items[n] = b;
                    R.Items[rows-n-1] = b;
                }
                Areas.Add(A);
                Areas.Add(R);

                Lines.Add(A);
                ReverseLines.Add(R);
            }

            // columns:
            for (int i = 1; i <= rows; i++)
            {
                Area A = new Area(this,rows);
                Area R = new Area(this,rows);
                R.IsMainArea = false;

                A.ReverseArea = R;
                R.ReverseArea = A;

                for (int n = 0; n <= rows-1; n++)
                {
                    A.Items[n] = Lines[n].Item(i);
                    R.Items[n] = Lines[rows -1 - n].Item(i);
                }
                Areas.Add(A);
                Areas.Add(R);

                Columns.Add(A);
                ReverseColumns.Add(R);
            }
        }
    }


    

   
}
