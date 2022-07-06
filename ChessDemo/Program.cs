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
