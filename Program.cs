using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> cells = new List<string>
            {
                "5 0 3 0 0 6 1 0 0",
                "2 0 0 0 0 7 3 0 0",
                "1 0 0 8 0 2 7 0 5",
                "3 5 1 0 0 7 4 6 0",
                "0 0 2 0 4 0 8 0 1",
                "0 8 4 6 1 0 0 0 0",
                "0 0 0 0 0 2 8 0 5",
                "0 0 8 4 1 5 0 6 0",
                "5 6 1 0 7 0 0 0 0"
            };

            Board board = new Board(cells);
            Solver s = new Solver(board);
            //s.Print(s.board);
            s.Run();
            Console.ReadLine();
        }
    }
}
