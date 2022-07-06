using System;
using System.Collections.Generic;
using System.Text;

namespace ChessRules
{
    class Board
    {
        public string fen { get; protected set; }

        protected Figure[,] figures;
        public Color moveColor { get; protected set; }
        public bool canCastleA1 { get; protected set; }//Q
        public bool canCastleH1 { get; protected set; }//K
        public bool canCastleA8 { get; protected set; }//q
        public bool canCastleH8 { get; protected set; }//k
        public Square enpassant { get; protected set; }
        public int drawNumber { get; protected set; }
        public int moveNumber { get; protected set; }

        public Board(string fen)
        {
            this.fen = fen;
            figures = new Figure[8, 8];
            Init();
        }
        public Board Move(FigureMoving fm)
        {
            return new NextBoard(fen, fm);
        }
        public bool IsCheck()
        {
            return IsCheckAfterMove(FigureMoving.none);
        }
        public bool IsCheckAfterMove(FigureMoving fm)
        {
            Board after = Move(fm);
            return after.CanEatKing();
        }
        bool CanEatKing()
        {
            Square badKing = FindBadKing();
            Moves moves = new Moves(this);
            foreach (FigureOnSquare fs in YieldMyFiguresOnSquares())
                if(moves.CanMove(new FigureMoving(fs, badKing)))
                    return true;
            return false;
        }

        Square FindBadKing()
        {
            Figure badKing = moveColor == Color.white ? Figure.blackKing : Figure.whiteKing;
            foreach (Square square in Square.YieldBoardSquares())
                if (GetFigureAT(square) == badKing)
                    return square;
            return Square.none;
        }

        //rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
        //0                                           1 2    3 4 5
        void Init()
        {
            string[] parts = fen.Split();
            InitFigures(parts[0]);
            InitMoveColor(parts[1]);
            InitCastleFlags(parts[2]);
            InitEnpassant(parts[3]);
            InitDrawNumber(parts[4]);
            InitMoveNumber(parts[5]);
        }
        void InitFigures(string v)
        {
            for (int j = 8; j >= 2; j--)
                v = v.Replace(j.ToString(), (j - 1).ToString() + "1");
            v = v.Replace('1', (char)Figure.none);
            string[] lines = v.Split('/');
            for (int y = 7; y >= 0; y--)
                for (int x = 0; x < 8; x++)
                    figures[x, y] = (Figure)lines[7 - y][x];
        }

        

        void InitMoveColor(string v)
        {
            moveColor = (v == "b") ? Color.black : Color.white;
        }
        void InitCastleFlags(string v)
        {
            canCastleA1 = v.Contains("Q");
            canCastleH1 = v.Contains("K");
            canCastleA8 = v.Contains("q");
            canCastleH8 = v.Contains("k");
        }
        void InitEnpassant(string v)
        {
            enpassant = new Square(v);
        }
        void InitDrawNumber(string v)
        {
            drawNumber = int.Parse(v);
        }
        void InitMoveNumber(string v)
        {
            moveNumber = int.Parse(v);
        } 

        public Figure GetFigureAT(Square square)
        {
            if (square.OnBoard())
                return figures[square.x, square.y];
            else return Figure.none;
        }

        public IEnumerable<FigureOnSquare> YieldMyFiguresOnSquares()
        {
            foreach (var square in Square.YieldBoardSquares())
                if (GetFigureAT(square).GetColor() == moveColor)
                    yield return new FigureOnSquare(GetFigureAT(square), square);
        }
    }
}
