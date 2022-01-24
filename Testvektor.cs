using System;
using System.Collections.Generic;
using System.Text;

namespace HochhausSolver
{
    class Testvektor
    {

        public int[] vektor;
        public int Size;
        public int dynamic=1;
        public Area A;

        public Testvektor (int size)
        {
            Size = size;
            vektor = new int[Size];
        }

        public Testvektor(Testvektor v) 
        {
            
            Size = v.Size;
            vektor = new int[Size];
            for (int i = 1; i <= Size; i++)
                Set(i,v.Get(i));
            A=v.A;

            dynamic=v.dynamic+1;
        }


        public void Set(int pos,int i)
        {
            vektor[pos - 1] = i;
        }

        public int Get(int pos)
        {
            return vektor[pos - 1];
        }

        public int CheckView(int[] v)
        {           
            int cnt = 0;
            int min = 0;
            for (int i = 0; i <= Size-1; i++)
            {
                if (v[i] > min)
                {
                    min = v[i];
                    cnt++;
                }
            }
            return cnt;
        }
        public Boolean Check()
        {
            //if (A.ViewNumber == 4 && A.Item(1).Solution == 7 && dynamic == Size + 1)
            //{
            //    foreach (int i in vektor) Console.Write(i.ToString() + " ");
            //    Console.WriteLine("");
            //}

            if (dynamic > Size)
            {
                //if (A.ViewNumber == 4 && A.Item(1).Solution == 7 && Get(9) == 9)
                //{
                //    foreach (int i in vektor) Console.Write(i.ToString() + " ");
                //    Console.WriteLine(CheckView(vektor).ToString());
                //}
                if (A.HasView && CheckView(vektor) != A.ViewNumber) return false;
                if (A.ReverseArea.HasView) 
                { 
                    int[] r = (int[])vektor.Clone();
                    Array.Reverse(r);
                    if (CheckView(r) != A.ReverseArea.ViewNumber) return false;
                }

                return true;
            }

            Testvektor t = new Testvektor(this);

            if (Get(dynamic) > 0)
            {                
                if (t.Check()) return true;
            }
            else
            foreach (int versuch in A.Item(dynamic).Options)
            {
                if (Array.IndexOf(vektor, versuch) >= 0) continue;
                t.Set(dynamic, versuch);
                
                if ( t.Check()) return true;
            }
            return false;
        }
    }
}
