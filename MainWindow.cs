using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Tetris
{
    public partial class MainWindow : Form
    {
        // Initialize global variables - Inicializar variables globales
        Control[] activePiece = { null, null, null, null };
        Control[] activePiece2 = { null, null, null, null };
        Control[] nextPiece = { null, null, null, null };
        Control[] savedPiece = { null, null, null, null };
        Control[] Ghost = { null, null, null, null };
        List<int> PieceSequence = new List<int>();
        int timeElapsed = 0;
        int currentPiece;
        int nextPieceInt;
        int savedPieceInt = -1;
        int rotations = 0;
        Color pieceColor = Color.White;
        Color savedPieceColor = Color.White;
        int combo = 0;
        int score = 0;
        int clears = 0;
        int level = 0;
        bool gameOver = false;
        int PieceSequenceIteration = 0;

        readonly Color[] colorList = 
        {  
            Color.Cyan,     // I piece
            Color.Orange,   // L piece
            Color.Blue,     // J piece
            Color.Green,    // S piece
            Color.Red,      // Z piece
            Color.Yellow,   // O piece
            Color.Purple    // T piece
        };

        // Load main window - Cargar ventana principal
        public MainWindow()      
        {
            InitializeComponent();

            ScoreUpdateLabel.Text = "";
            SpeedTimer.Start();
            GameTimer.Start();

            // Initialize/reset ghost piece - Inicializar/restablecer pieza fantasma
            // box1 through box4 are invisible - box1 a box4 son invisibles
            activePiece2[0] = box1;
            activePiece2[1] = box2;
            activePiece2[2] = box3;
            activePiece2[3] = box4;

            // Generate piece sequence - Generar secuencia de piezas
            System.Random random = new System.Random();
            while (PieceSequence.Count < 7)
            {
                int x = random.Next(7);
                if (!PieceSequence.Contains(x))
                {
                    PieceSequence.Add(x);
                }
            }

            // Select first piece - Seleccione la primera pieza
            nextPieceInt = PieceSequence[0];
            PieceSequenceIteration++;

            DropNewPiece();
        }

        public void DropNewPiece()
        {
            // Reset number of times current piece has been rotated
            // Restablecer el número de veces que se ha girado la pieza actual
            rotations = 0;

            // Move next piece to current piece
            // Mover la pieza siguiente a la pieza actual
            currentPiece = nextPieceInt;

            // If last piece of PieceSequence, generate new PieceSequence
            // Si es la última pieza de PieceSequence, generar una nueva PieceSequence
            if (PieceSequenceIteration == 7)
            {
                PieceSequenceIteration = 0;

                // Scramble PieceSequence
                // Secuencia de piezas revueltas
                PieceSequence.Clear();
                System.Random random = new System.Random();
                while (PieceSequence.Count < 7)
                {
                    int x = random.Next(7);
                    if (!PieceSequence.Contains(x))
                    {
                        PieceSequence.Add(x);
                    }
                }
            }

            // Select next piece from PieceSequence 
            // Seleccione la siguiente pieza de PieceSequence
            nextPieceInt = PieceSequence[PieceSequenceIteration];
            PieceSequenceIteration++;

            // If not first move, clear next piece panel
            //Si no es el primer movimiento, despeja el panel de la siguiente pieza
            if (nextPiece.Contains(null) == false)
            {
                foreach (Control x in nextPiece)
                {
                    x.BackColor = Color.White;
                }
            }

            // Layout options for next piece
            // Opciones de diseño para la siguiente pieza
            Control[,] nextPieceArray = 
            {
                { box203, box207, box211, box215 }, // I piece
                { box202, box206, box210, box211 }, // L piece
                { box203, box207, box211, box210 }, // J piece
                { box206, box207, box203, box204 }, // S piece
                { box202, box203, box207, box208 }, // Z piece
                { box206, box207, box210, box211 }, // O piece
                { box207, box210, box211, box212 }  // T piece
            };

            // Retrieve layout for next piece
            // Recuperar diseño para la siguiente pieza
            for (int x = 0; x < 4; x++)
            {
                nextPiece[x] = nextPieceArray[nextPieceInt,x];
            }

            // Populate next piece panel with correct color
            // Rellene el panel de la siguiente pieza con el color correcto
            foreach (Control square in nextPiece)
            {
                square.BackColor = colorList[nextPieceInt];
            }

            // Layout options for falling piece
            // Opciones de diseño para la pieza que cae
            Control[,] activePieceArray =
            {
                { box6, box16, box26, box36 }, // I piece
                { box4, box14, box24, box25 }, // L piece
                { box5, box15, box25, box24 }, // J piece
                { box14, box15, box5, box6 },  // S piece
                { box5, box6, box16, box17 },  // Z piece
                { box5, box6, box15, box16 },  // O piece
                { box6, box15, box16, box17 }  // T piece
            };

            // Select falling piece
            // Seleccionar pieza que cae
            for (int x = 0; x < 4; x++)
            {
                activePiece[x] = activePieceArray[currentPiece, x];
            }

            // This is needed for DrawGhost()
            // Esto es necesario para DrawGhost()
            for (int x = 0; x < 4; x++)
            {
                activePiece2[x] = activePieceArray[currentPiece, x];
            }

            // Check for game over
            // Comprobar si el juego ha terminado
            foreach (Control box in activePiece)
            {
                if (box.BackColor != Color.White & box.BackColor != Color.LightGray)
                {
                    //Game over!
                    SpeedTimer.Stop();
                    GameTimer.Stop();
                    gameOver = true;
                    MessageBox.Show("Game over!");
                    return;
                }
            }

            // Draw ghost piece
            // Dibujar pieza fantasma
            DrawGhost();

            // Populate falling piece squares with correct color
            // Rellene los cuadrados de piezas que caen con el color correcto
            foreach (Control square in activePiece)
            {
                square.BackColor = colorList[currentPiece];
            }
        }

        // Test if a potential move (left/right/down) would be outside the grid or overlap another piece
        // Pruebe si un movimiento potencial (izquierda/derecha/abajo) estaría fuera de la cuadrícula o se superpondría a otra pieza
        public bool TestMove(string direction)
        {
            int currentHighRow = 21;
            int currentLowRow = 0;
            int currentLeftCol = 9;
            int currentRightCol = 0;

            int nextSquare = 0;

            Control newSquare = new Control();

            // Determine highest, lowest, left, and right rows of potential move
            // Determine las filas más alta, más baja, izquierda y derecha del movimiento potencial
            foreach (Control square in activePiece)
            {
                if (grid.GetRow(square) < currentHighRow)
                {
                    currentHighRow = grid.GetRow(square);
                }
                if (grid.GetRow(square) > currentLowRow)
                {
                    currentLowRow = grid.GetRow(square);
                }
                if (grid.GetColumn(square) < currentLeftCol)
                {
                    currentLeftCol = grid.GetColumn(square);
                }
                if (grid.GetColumn(square) > currentRightCol)
                {
                    currentRightCol = grid.GetColumn(square);
                }
            }

            // Test if any squares would be outside of grid
            // Pruebe si algún cuadrado estaría fuera de la cuadrícula
            foreach (Control square in activePiece)
            {
                int squareRow = grid.GetRow(square);
                int squareCol = grid.GetColumn(square);

                // Left- Izquierda
                if (direction == "left" & squareCol > 0)
                {
                    newSquare = grid.GetControlFromPosition(squareCol - 1, squareRow);
                    nextSquare = currentLeftCol;
                }
                else if (direction == "left" & squareCol == 0)
                {
                    // Move would be outside of grid, left
                    // El movimiento estaría fuera de la cuadrícula, a la izquierda
                    return false;
                }

                // Right - derecha
                else if (direction == "right" & squareCol < 9)
                {
                    newSquare = grid.GetControlFromPosition(squareCol + 1, squareRow);
                    nextSquare = currentRightCol;
                }
                else if (direction == "right" & squareCol == 9)
                {
                    // Move would be outside of grid, right
                    return false;
                }

                // Down
                else if (direction == "down" & squareRow < 21)
                {
                    newSquare = grid.GetControlFromPosition(squareCol, squareRow + 1);
                    nextSquare = currentLowRow;
                }
                else if (direction == "down" & squareRow == 21)
                {
                    return false;
                    // Move would be below grid
                }

                // Test if potential move would overlap another piece
                if ((newSquare.BackColor != Color.White & newSquare.BackColor != Color.LightGray) & activePiece.Contains(newSquare) == false & nextSquare > 0)
                {
                    return false;
                }

            }

            // All tests passed
            return true;
        }

        public void MovePiece(string direction)
        {
            // Erase old position of piece
            // and determine new position based on input direction
            int x = 0;
            foreach (PictureBox square in activePiece)
            {
                square.BackColor = Color.White;
                int squareRow = grid.GetRow(square);
                int squareCol = grid.GetColumn(square);
                int newSquareRow = 0;
                int newSquareCol = 0;
                if (direction == "left")
                {
                    newSquareCol = squareCol - 1;
                    newSquareRow = squareRow;
                }
                else if (direction == "right")
                {
                    newSquareCol = squareCol + 1;
                    newSquareRow = squareRow;
                }
                else if (direction == "down")
                {
                    newSquareCol = squareCol;
                    newSquareRow = squareRow + 1;
                }

                activePiece2[x] = grid.GetControlFromPosition(newSquareCol, newSquareRow);
                x++;
            }

            // Copy activePiece2 to activePiece
            x = 0;
            foreach (PictureBox square in activePiece2)
            {

                activePiece[x] = square;
                x++;
            }

            // Draw ghost piece (must be between erasing old position and drawing new position)
            DrawGhost();

            // Draw piece in new position
            x = 0;
            foreach (PictureBox square in activePiece2)
            {
                square.BackColor = colorList[currentPiece];
                x++;
            }
        }

        // Test if a potential rotation would overlap another piece
        private bool TestOverlap()
        {
            foreach (PictureBox square in activePiece2)
            {
                if ((square.BackColor != Color.White & square.BackColor != Color.LightGray) & activePiece.Contains(square) == false)
                {
                    return false;
                }
            }
            return true;
        }
        
        // Timer for piece movement speed - increases with game level
        // Speed is controlled by LevelUp() method
        private void SpeedTimer_Tick(object sender, EventArgs e)
        {
            if (CheckGameOver() == true)
            {
                SpeedTimer.Stop();
                GameTimer.Stop();
                MessageBox.Show("Game over!");
            }

            else
            {
                //Move piece down, or drop new piece if it can't move
                if (TestMove("down") == true)
                {
                    MovePiece("down");
                }
                else
                {
                    if (CheckGameOver() == true)
                    {
                        SpeedTimer.Stop();
                        GameTimer.Stop();
                        MessageBox.Show("Game over!");
                    }
                    if (CheckForCompleteRows() > -1)
                    {
                        ClearFullRow();
                    }
                    DropNewPiece();
                }
            }
        }

        // Game time (seconds elapsed)
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            timeElapsed++;
            TimeLabel.Text = "Time: " + timeElapsed.ToString();
        }

        // Clear lowest full row
        private void ClearFullRow()
        {
            int completedRow = CheckForCompleteRows();

            //Turn that row white
            for (int x = 0; x <= 9; x++)
            {
                Control z = grid.GetControlFromPosition(x, completedRow);
                z.BackColor = Color.White;
            }

            //Move all other squares down
            for (int x = completedRow - 1; x >= 0; x--) //For each row above cleared row
            {
                //For each square in row
                for (int y = 0; y <= 9; y++)
                {
                    //the square
                    Control z = grid.GetControlFromPosition(y, x);

                    //the square below it
                    Control zz = grid.GetControlFromPosition(y, x + 1);

                    zz.BackColor = z.BackColor;
                    z.BackColor = Color.White;
                }
            }

            UpdateScore();

            clears++;
            ClearsLabel.Text = "Clears: " + clears;

            if (clears % 2 == 0)
            {
                LevelUp();
            }

            if (CheckForCompleteRows() > -1)
            {
                ClearFullRow();
            }
        }

        private void UpdateScore()
        {
            // 1-3 line clear is worth 100 per line
            // Quad line clear (no combo) is worth 800
            // 2 or more quad line clears in a row is worth 1200 

            bool skipComboReset = false;

            // Single clear
            if (combo == 0)
            {
                score += 100;
                ScoreUpdateLabel.Text = "+100";
            }

            // Double clear
            else if (combo == 1)
            {
                score += 100;
                ScoreUpdateLabel.Text = "+200";
            }

            // Triple clear
            else if (combo == 2)
            {
                score += 100;
                ScoreUpdateLabel.Text = "+300";
            }

            // Quad clear, start combo
            else if (combo == 3)
            {
                score += 500;
                ScoreUpdateLabel.Text = "+800";
                skipComboReset = true;
            }

            // Single clear, broken combo
            else if (combo > 3 && combo % 4 == 0)
            {
                score += 100;
                ScoreUpdateLabel.Text = "+100";
            }

            // Double clear, broken combo
            else if (combo > 3 && ((combo - 1) % 4 == 0))
            {
                score += 100;
                ScoreUpdateLabel.Text = "+200";
            }

            // Triple clear, broken combo
            else if (combo > 3 && ((combo - 2) % 4 == 0))
            {
                score += 100;
                ScoreUpdateLabel.Text = "+300";
            }

            // Quad clear, continue combo
            else if (combo > 3 && ((combo - 3) % 4 == 0))
            {
                score += 900;
                ScoreUpdateLabel.Text = "+1200";
                skipComboReset = true;
            }

            if (CheckForCompleteRows() == -1 && skipComboReset == false)
            {
                // 1-3 line clear
                combo = 0;
            }
            else
            {
                // Quad clearl
                combo++;
            }

            ScoreLabel.Text = "Score: " + score.ToString();
            ScoreUpdateTimer.Start();
        }

        // Return row number of lowest full row
        // If no full rows, return -1
        private int CheckForCompleteRows()
        {
            // For each row
            for (int x = 21; x >= 2; x--)
            {
                // For each square in row
                for (int y = 0; y <= 9; y++)
                {
                    Control z = grid.GetControlFromPosition(y, x);
                    if (z.BackColor == Color.White)
                    {
                        break;
                    }
                    if (y == 9)
                    {
                        // Return full row number
                        return x;
                    }
                }
            }
            return -1; // "null"
        }

        // Increase fall speed
        private void LevelUp()
        {
            level++;
            LevelLabel.Text = "Level: " + level.ToString();

            // Milliseconds per square fall
            // Level 1 = 800 ms per square, level 2 = 716 ms per square, etc.
            int[] levelSpeed =
            {
                800, 700, 600, 500, 400, 300, 150, 100, 90, 80, 75
            };

            // Speed does not change after level 29
            if (level <= 10)
            {
                SpeedTimer.Interval = levelSpeed[level];
            }
        }

        // Game ends if a piece is in the top row when the next piece is dropped
        private bool CheckGameOver()
        {
            Control[] topRow = { box1, box2, box3, box4, box5, box6, box7, box8, box9, box10 };

            foreach (Control box in topRow)
            {
                if ((box.BackColor != Color.White & box.BackColor != Color.LightGray) & !activePiece.Contains(box))
                {
                    //Game over!
                    return true;
                }
            }

            if (gameOver == true)
            {
                return true;
            }

            return false;
        }

        // Clear score update notification every 2 seconds
        private void ScoreUpdateTimer_Tick(object sender, EventArgs e)
        {
                ScoreUpdateLabel.Text = "";
                ScoreUpdateTimer.Stop();
        }
    }   
}
