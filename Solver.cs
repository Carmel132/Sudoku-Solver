using System.Collections.Generic;
using System.Linq;
using System;

namespace Sudoku_Solver
{
#pragma warning disable CS0660
#pragma warning disable CS0661
    class Value // 1 x 1 square
#pragma warning restore CS0661
#pragma warning restore CS0660
    {
        public int Val { get; set; }
        public List<int> Works;
        public bool access;
        public Value(int StartVal)
        {
            Val = StartVal;
            Works = new List<int>();
            access = StartVal == 0;
        }
        public static bool operator ==(Value val1, Value val2)
        {
            return val1.Val == val2.Val;
        }
        public static bool operator ==(Value val1, int val2)
        {
            return val1.Val == val2;
        }
        public static bool operator !=(Value val1, Value val2)
        {
            return val1.Val != val2.Val;
        }
        public static bool operator !=(Value val1, int val2)
        {
            return val1.Val != val2;
        }
        public static Value Int(int num)
        {
            return new Value(num);
        }
    }


    class Solver
    {
        public Board board;

        public Solver(Board StartData)
        {
            board = StartData;
        }
        public void Progress(ref (int, int, int) pointer)
        {
            //Console.WriteLine("Progression");
            if (pointer == (8, 2, 2)) {pointer = (0, 0, 0); return; } 
            if (pointer.Item3 >= 2)
            {
                pointer.Item3 = 0;
                //Console.WriteLine("Pointer.Item3 = 0");
                if ((pointer.Item1 + 1) % 3 == 0)
                {
                    if (pointer.Item2 < 2)
                    {
                        pointer.Item1 -= 2;
                        pointer.Item2++;
                        pointer.Item3 = 0;
                  //      Console.WriteLine("Pointer.Item1 -= 1\nPointer.Item2 += 1\nPointer.Item3 = 0");
                    }
                    else
                    {
                        pointer.Item1++;
                        pointer.Item2 = 0;
                        pointer.Item3 = 0;
                //        Console.WriteLine("Pointer.Item1 += 1\nPointer.Item2 = 0\nPointer.Item3 = 0");
                    }
                }
                else
                {
                    pointer.Item1++;
                  //  Console.WriteLine("Pointer.Item1 += 1");
                }
            }
            else
            {
                pointer.Item3++;
              //  Console.WriteLine("Pointer.Item3 += 1");
            }
            //Console.WriteLine(pointer);
            //Console.Write("\n");
        }
        public void Regress(ref (int, int, int) pointer)  
        {
            if (pointer.Equals( (0, 0, 0))) { pointer = (8, 2, 2); return;}
            //Console.WriteLine("Regression");
            if (pointer.Item3 < 1)
            {
                pointer.Item3 = 2;
                //Console.WriteLine("Pointer.Item3 = 2");
                if (pointer.Item1 % 3 == 0)
                {
                    if (pointer.Item2 > 0)
                    {
                        pointer.Item1 += 2;
                        pointer.Item2--;
                        pointer.Item3 = 2;
                        //          Console.WriteLine("Pointer.Item1 += 2\nPointer.Item2 -= 1\n Pointer.Item3 = 2");
                    }
                    else
                    {
                        pointer.Item1--;
                        //        Console.WriteLine("Pointer.Item1 -= 1");
                    }
                }
                else
                {
                    pointer.Item1--;
                    pointer.Item3 = 2;
                    //  Console.WriteLine("Pointer.Item1 -= 1\nPointer.Item3 = 2");
                }
            }
            else { pointer.Item3--; }//Console.WriteLine("Pointer.Item3 -= 1"); 
          //  Console.WriteLine(pointer);
            //Console.Write("\n");
        }
        public Board Solve()
        {
            Board temp = board.GetCopy(); // Safety reasons
            temp.Update();
            (int, int, int) pointer = (0, 0, 0);
            while (!temp.Solved())
            {
                temp.Update();
                //Console.WriteLine(pointer.ToString());
                if (temp[pointer].access)
                {//under assumption trying to increase current pointer by 1 work value
                    if (temp[pointer].Works.Count > 0)
                    {
                    //Print(temp);
                    //Console.Write("\n" + pointer.ToString() + " Works = " );
                    //temp[pointer].Works.ForEach(Console.Write);
                    //Console.WriteLine();
                        if (temp[pointer] == 0) { temp[pointer].Val = temp[pointer].Works[0];/* Console.WriteLine(pointer.ToString() + " = " + temp[pointer].Val.ToString());*/ Progress(ref pointer);}
                        else
                        {
                            if ( temp[pointer] == temp[pointer].Works[ temp[pointer].Works.Count - 1 ] ) // hell (is last value of works)
                            {
                                temp[pointer].Val = 0;
                                do
                                {
                                    temp.Update();
                                    if (temp[pointer].access) { temp[pointer].Val = 0; /*Console.WriteLine(pointer.ToString() + " = 0"));*/}
                                    Regress(ref pointer);
                                    //Console.WriteLine(pointer.ToString());
                                } 
                                while ( temp[pointer].access == false && temp[pointer].Works.Count > 0 ); // might change > 1
                            }
                            else
                            {
                                temp[pointer].Val = temp[pointer].Works[ temp[pointer].Works.IndexOf(temp[pointer].Val) + 1 ];
                                //Console.WriteLine(pointer.ToString() + " = " + temp[pointer].Val.ToString());
                                Progress(ref pointer);
                            }
                        }
                    }
                    else
                    {
                        temp[pointer].Val = 0;
                        do
                        {
                            temp.Update();
                            if (temp[pointer].access) { temp[pointer].Val = 0;}
                            Regress(ref pointer);
                            //Console.WriteLine(pointer.ToString());
                        } 
                        while ( temp[pointer].access == false && temp[pointer].Works.Count > 0 ); // might change > 1
                    }
                }
                else { Progress(ref pointer);}
            }
            return temp;
        }
        public void Print(Board board)
        {
            (int, int, int) pointer = (0, 0, 0);
            Console.WriteLine("--------------------------");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            //Console.WriteLine(pointer);
                            Console.Write(Convert.ToString(board[pointer].Val) + " ");
                            Progress(ref pointer);

                        }
                        Console.Write(" | ");
                    }
                    Console.Write("\n");
                }
                Console.WriteLine("--------------------------");
            }
        }
        public void Run()
        {
            Console.WriteLine("Unsolved: \n");
            Print(board);
            Console.WriteLine("\n\nSolved: \n");
            Print(Solve());
        }
    }
}

