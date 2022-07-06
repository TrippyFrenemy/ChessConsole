using System;

namespace ChessRules
{
    class Moves
    {
        FigureMoving fm;
        Board board;

        public object StepY { get; private set; }

        public Moves(Board board)
        {
            this.board = board;
        }

        public bool CanMove(FigureMoving fm)
        {
            this.fm = fm;
            return CanMoveFrom() && CanMoveTo() && CanFigureMove();
        }
        private bool CanMoveTo()
        {
            return fm.to.OnBoard() &&
                   board.GetFigureAT(fm.to).GetColor() != board.moveColor;
        }
        private bool CanMoveFrom()
        {
            return fm.from.OnBoard() &&
                   fm.figure.GetColor() == board.moveColor &&
                   board.GetFigureAT(fm.from) == fm.figure;
        }
        private bool CanFigureMove()
        {
            switch (fm.figure)
            {
                case Figure.whiteKing:
                case Figure.blackKing:
                    return CanKingMove();

                case Figure.whiteQueen:
                case Figure.blackQueen:
                    return CanStraightMove();

                case Figure.whiteRook:
                case Figure.blackRook:
                    return (fm.SignX == 0 || fm.SignY == 0) &&
                           CanStraightMove();

                case Figure.whiteBishop:
                case Figure.blackBishop:
                    return (fm.SignX != 0 && fm.SignY != 0) &&
                           CanStraightMove();

                case Figure.whiteKnight:
                case Figure.blackKnight:
                    return CanKnightMove();

                case Figure.whitePawn:
                case Figure.blackPawn:
                    return CanPawnMove();

                default:
                    return false;
            }
        }

        bool CanKingMove()//
        {
            return (fm.AbsDeltaX <= 1) && (fm.AbsDeltaY) <= 1;
        }
        bool CanStraightMove()
        {
            Square at = fm.from;
            do
            {
                at = new Square(at.x + fm.SignX, at.y + fm.SignY);
                if (at == fm.to)
                    return true;
            } while (at.OnBoard() &&
                     board.GetFigureAT(at) == Figure.none);
            return false;

        }

        bool CanKnightMove()//
        {
            return ((fm.AbsDeltaX == 2) && (fm.AbsDeltaY) == 1) || ((fm.AbsDeltaX == 1) && (fm.AbsDeltaY) == 2);
        }
        private bool CanPawnMove()
        {
            if (fm.from.y < 1 || fm.from.y > 6)
                return false;
            int stepY = fm.figure.GetColor() == Color.white ? +1 : -1;
            return CanPawnGo(stepY) || // +1
                   CanPawnJump(stepY) || // +2
                   CanPawnEat(stepY); // take
        }
        private bool CanPawnGo(int stepY)
        {
            if (board.GetFigureAT(fm.to) == Figure.none)
                if (fm.DeltaX == 0)
                    if (fm.DeltaY == stepY)
                        return true;
            return false;
        }
        private bool CanPawnJump(int stepY)
        {
            if (board.GetFigureAT(fm.to) == Figure.none)
                if ((fm.from.y == 1 && stepY == +1) ||
                     fm.from.y == 6 && stepY == -1)
                    if (fm.DeltaX == 0)
                        if (fm.DeltaY == 2 * stepY)
                            if (board.GetFigureAT(new Square(fm.from.x, fm.from.y + stepY)) == Figure.none)
                                return true;
            return false;
        }
        private bool CanPawnEat(int stepY)
        {
            if (board.GetFigureAT(fm.to) != Figure.none)
                if (fm.AbsDeltaX == 1)
                    if (fm.DeltaY == stepY)
                        return true;
            return false;
        }
    }
}
