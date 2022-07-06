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
        bool CanMoveTo()
        {
            return fm.to.OnBoard() &&
                   board.GetFigureAT(fm.to).GetColor() != board.moveColor;
        }
        bool CanMoveFrom()
        {
            return fm.from.OnBoard() &&
                   fm.figure.GetColor() == board.moveColor &&
                   board.GetFigureAT(fm.from) == fm.figure;
        }

        bool CanFigureMove()
        {
            switch (fm.figure)
            {
                case Figure.whiteKing:
                case Figure.blackKing:
                    return CanKingMove() || 
                           CanKingCastle();

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
        bool CanKingMove()
        {
            return (fm.AbsDeltaX <= 1) && (fm.AbsDeltaY) <= 1;
        }
        bool CanKingCastle()
        {
            if(fm.figure == Figure.whiteKing)
            {
                if (fm.from == new Square("e1"))
                if (fm.to == new Square("g1"))
                if (board.canCastleH1)
                if (board.GetFigureAT(new Square("h1")) == Figure.whiteRook)
                if (board.GetFigureAT(new Square("f1")) == Figure.none)
                if (board.GetFigureAT(new Square("g1")) == Figure.none)
                if(!board.IsCheck())
                if(!board.IsCheckAfterMove(new FigureMoving("Ke1f1")))
                return true;

                if (fm.from == new Square("e1"))
                if (fm.to   == new Square("c1"))
                if (board.canCastleA1)
                if (board.GetFigureAT(new Square("a1")) == Figure.whiteRook)
                if (board.GetFigureAT(new Square("d1")) == Figure.none)
                if (board.GetFigureAT(new Square("c1")) == Figure.none)
                if (board.GetFigureAT(new Square("b1")) == Figure.none)
                if(!board.IsCheck())
                if(!board.IsCheckAfterMove(new FigureMoving("Ke1d1")))
                return true;
            }
            if (fm.figure == Figure.blackKing)
            {
                if (fm.from == new Square("e8"))
                if (fm.to   == new Square("g8"))
                if (board.canCastleH8)
                if (board.GetFigureAT(new Square("h8")) == Figure.blackRook)
                if (board.GetFigureAT(new Square("f8")) == Figure.none)
                if (board.GetFigureAT(new Square("g8")) == Figure.none)
                if(!board.IsCheck())
                if(!board.IsCheckAfterMove(new FigureMoving("ke8f8")))
                return true;

                if (fm.from == new Square("e8"))
                if (fm.to   == new Square("c8"))
                if (board.canCastleA8)
                if (board.GetFigureAT(new Square("a8")) == Figure.blackRook)
                if (board.GetFigureAT(new Square("d8")) == Figure.none)
                if (board.GetFigureAT(new Square("c8")) == Figure.none)
                if (board.GetFigureAT(new Square("b8")) == Figure.none)
                if(!board.IsCheck())
                if(!board.IsCheckAfterMove(new FigureMoving("ke8d8")))
                return true;
            }
            return false;
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
        bool CanKnightMove()
        {
            return ((fm.AbsDeltaX == 2) && (fm.AbsDeltaY) == 1) || ((fm.AbsDeltaX == 1) && (fm.AbsDeltaY) == 2);
        }
        bool CanPawnMove()
        {
            if (fm.from.y < 1 || fm.from.y > 6)
                return false;
            int stepY = fm.figure.GetColor() == Color.white ? +1 : -1;
            return CanPawnGo(stepY) || // +1
                   CanPawnJump(stepY) || // +2
                   CanPawnEat(stepY) ||
                   CanPawnEnpassant(stepY); // take
        }

        bool CanPawnGo(int stepY)
        {
            if (board.GetFigureAT(fm.to) == Figure.none)
                if (fm.DeltaX == 0)
                    if (fm.DeltaY == stepY)
                        return true;
            return false;
        }
        bool CanPawnJump(int stepY)
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
        bool CanPawnEat(int stepY)
        {
            if (board.GetFigureAT(fm.to) != Figure.none)
                if (fm.AbsDeltaX == 1)
                    if (fm.DeltaY == stepY)
                        return true;
            return false;
        }
        bool CanPawnEnpassant(int stepY)
        {
            if (fm.to == board.enpassant)
                if (board.GetFigureAT(fm.to) == Figure.none)
                    if (fm.DeltaY == stepY)
                        if (fm.AbsDeltaX == 1)
                            if ((stepY == +1 && fm.from.y == 4) ||
                                (stepY == -1 && fm.from.y == 3))
                                return true;
            return false;

        }

    }
}
