using System.Collections.Generic;

namespace ChessRules
{
    public class Chess
    {
        public string fen { get { return board.fen; } }
        public bool IsCheck { get; private set; }
        public bool IsCheckMate { get; private set; }
        public bool IsStaleMate { get; private set; }
        Moves moves;
        Board board;
        
        public Chess(string fen ="rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            board = new Board(fen);
            moves = new Moves(board);
            SetCheckFlags();
        }

        Chess(Board board)
        {
            this.board = board;
            moves = new Moves(board);
            SetCheckFlags();
        }

        void SetCheckFlags()
        {
            IsCheck = board.IsCheck();
            IsCheckMate = false;
            IsStaleMate = false;
            foreach (string moves in YieldValidMoves())
                return;
            if(IsCheck)
                IsCheckMate=true;
            else IsStaleMate=true;
        }

        public bool IsValidMove(string move)
        {
            FigureMoving fm = new FigureMoving(move);
            if (!moves.CanMove(fm))
                return false;
            if (board.IsCheckAfterMove(fm))
                return false;
            return true;
        }

        public Chess Move(string move)
        {
            FigureMoving fm = new FigureMoving(move);
            if (!IsValidMove(move))
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
        public char GetFigureAt(string xy)
        {
            Square square = new Square(xy);
            Figure figure = board.GetFigureAT(square);
            return figure == Figure.none ? '.' : (char)figure;
        }

        public IEnumerable<string> YieldValidMoves()
        {
            foreach (FigureOnSquare fs in board.YieldMyFiguresOnSquares())
                foreach (Square to in Square.YieldBoardSquares())
                    foreach (Figure promotion in fs.figure.YieldPromotions(to))
                    {
                        FigureMoving fm = new FigureMoving(fs, to, promotion);
                        if(moves.CanMove(fm))
                            if(!board.IsCheckAfterMove(fm))
                            yield return fm.ToString();
                    }           
        }
    }
}
