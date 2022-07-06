using System.Collections.Generic;

namespace ChessRules
{
    public class Chess
    {
        public string fen { get { return board.fen; } }
        
        Moves moves;
        Board board;
        
        public Chess(string fen ="rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            board = new Board(fen);
            moves = new Moves(board);
        }

        Chess( Board board)
        {
            this.board = board;
            moves = new Moves(board);
        }

        public Chess Move(string move)
        {
            FigureMoving fm = new FigureMoving(move);
            if (!moves.CanMove(fm)) 
                return this;
            
            Board nextBoard = board.Move(fm);
            var nextChess = new Chess(nextBoard);
            return nextChess;
        }

        public char GetFigureAt(int x, int y)
        {
            Square square = new Square(x, y);
            Figure figure = board.GetFigureAT(square);
            return figure == Figure.none ? '.' : (char)figure;
        } 

        public IEnumerable<string> YieldValidMoves()
        {
            foreach (FigureOnSquare fs in board.YieldMyFiguresOnSquares())
                foreach (Square to in Square.YieldBoardSquares())
                {
                    FigureMoving fm = new FigureMoving(fs, to);
                    if(moves.CanMove(fm))
                        yield return fm.ToString();
                }
                
            
        }

    }
}
