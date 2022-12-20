using ChessRules;
using System;
using System.Text;

namespace ChessDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Chess chess = new Chess();
            
            while(true)
            {
                Console.WriteLine(chess.fen);
                Print(ChessToAscii(chess));
                foreach(string moves in chess.YieldValidMoves())
                    Console.WriteLine(moves);
                string move = Console.ReadLine();
                if(move == "") { break; }
                chess = chess.Move(move);
            }
        }

        //static int NextMoves(int step, Chess chess)
        //{
        //    if (step == 0) return 1;
        //    int count = 0;
        //    foreach (string moves in chess.YieldValidMoves())
        //        count += NextMoves(step - 1, chess.Move(moves));
        //    return count;
        //}

        static string ChessToAscii(Chess chess)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("  +-----------------+");
            for(int y = 7; y >= 0; y--)
            {
                sb.Append(y + 1);
                sb.Append(" | ");
                for(int x = 0; x<8; x++)
                    sb.Append(chess.GetFigureAt(x, y) + " ");
                sb.AppendLine("|");

            }
            sb.AppendLine("  +-----------------+");
            sb.AppendLine("    a b c d e f g h ");
            
            if (chess.IsCheckMate) sb.AppendLine("IS CHECKMATE");
            else if (chess.IsCheck) sb.AppendLine("IS CHECK");
            if (chess.IsStaleMate) sb.AppendLine("IS STALEMATE");
            return sb.ToString();
        }

        static void Print(string text)
        {
            ConsoleColor old = Console.ForegroundColor;
            foreach (char x in text)
            {
                if(x >= 'a' && x <= 'z') Console.ForegroundColor = ConsoleColor.Red;
                else if(x >= 'A' && x <= 'Z') Console.ForegroundColor = ConsoleColor.White;
                else Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(x);
            }
            Console.ForegroundColor = old;
        }
    }
}
