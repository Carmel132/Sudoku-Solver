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
        public static bool operator !=(Value val1, Value val2)
        {
            return val1.Val != val2.Val;
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
            //Console.WriteLine("Regression");
            if (pointer.Item3 > 0)
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
            Board temp = board.GetCopy() ; // Safety reasons
            temp.Update();
            (int, int, int) pointer = (0, 0, 0);
            int[] numbers = {1, 2, 3, 4, 5, 6, 7, 8, 9 };
            while (!temp.Solved())
            {
                //Print(temp);
                //Console.WriteLine(temp.Solved());
                //Console.WriteLine(temp[pointer].Val);
                Console.WriteLine("\n" + Convert.ToString(pointer));
                Console.Write("Works: ");
                temp[pointer].Works.ForEach(Console.Write);
                Console.Write("\n");
                if (temp.Data[pointer.Item1].Data[pointer.Item2][pointer.Item3].access)
                {
                    
                    Print(temp);
                    foreach (int i in numbers)
                    {
                        if (temp.IsLegal(pointer.ToTuple(), Value.Int(i)) && i >= temp.Read(pointer.ToTuple()) && temp[pointer].access)
                        {
                            if (temp[pointer].Val == 9) 
                            {
                                if (temp.IsLegal(pointer.ToTuple(), Value.Int(i)))
                                {
                                    temp[pointer].Val = 1;
                                    break;
                                }
                                else
                                {
                                    temp[pointer].Val = 0; Regress(ref pointer);
                                    break;
                                }
                            }
                            
                            Console.WriteLine(temp.CheckRow(1));
                            Console.WriteLine("Is legal: " + temp.IsLegal(pointer.ToTuple(), Value.Int(i)).ToString());
                            temp.Write(pointer.ToTuple(), Value.Int(i));
                            Console.WriteLine("Data[" + Convert.ToString(pointer) + "] = " + Convert.ToString(i));
                            Progress(ref pointer);
                            break;
                        }
                        else if (temp.Data[pointer.Item1].Data[pointer.Item2][pointer.Item3].Works.Count == 0)
                        {
                            temp.Write(pointer.ToTuple(), Value.Int(0));
                            Console.WriteLine(pointer.ToString() + " = " + 0.ToString());
                            do { Regress(ref pointer); } while (!temp.Data[pointer.Item1].Data[pointer.Item2][pointer.Item3].access || temp[pointer].Val == 9);
                            break;
                        }
                    }
                    
                }
                else { Progress(ref pointer); }
                temp.Update();
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
            board = Solve();
            Console.WriteLine("\n\n");
            Print(board);
        }
    }
}

