using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
    public partial class MainWindow : Form
    {
        // Handle inputs - triggered on any keypress
        //Manejar entradas: se activa con cualquier pulsación de tecla
        // Cleanup needed - limpieza necesaria
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (!CheckGameOver() & ((e.KeyCode == Keys.Left | e.KeyCode == Keys.A) & TestMove("left") == true))
            {
                MovePiece("left");
            }
            else if (!CheckGameOver() & ((e.KeyCode == Keys.Right | e.KeyCode == Keys.D) & TestMove("right") == true))
            {
                MovePiece("right");
            }
            else if ((e.KeyCode == Keys.Down | e.KeyCode == Keys.S) & TestMove("down") == true)
            {
                MovePiece("down");
            }
            else if (e.KeyCode == Keys.Up | e.KeyCode == Keys.W)
            {
                //Rotate - Rotar

                int square1Col = grid.GetColumn(activePiece[0]);
                int square1Row = grid.GetRow(activePiece[0]);

                int square2Col = grid.GetColumn(activePiece[1]);
                int square2Row = grid.GetRow(activePiece[1]);

                int square3Col = grid.GetColumn(activePiece[2]);
                int square3Row = grid.GetRow(activePiece[2]);

                int square4Col = grid.GetColumn(activePiece[3]);
                int square4Row = grid.GetRow(activePiece[3]);

                if (currentPiece == 0) //The line piece - la pieza de linea
                {
                    //Test if piece is too close to edge of board 
                    //Pruebe si la pieza está demasiado cerca del borde del tablero
                    if (rotations == 0 & (square1Col == 0 | square1Col == 1 | square1Col == 9))
                    {
                        return;
                    }
                    else if (rotations == 1 & (square3Col == 0 | square3Col == 1 | square3Col == 9))
                    {
                        return;
                    }

                    //If test passes, rotate piece - Si pasa la prueba, gire la pieza
                    if (rotations == 0)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col - 2, square1Row);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col - 1, square2Row - 1);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col, square3Row - 2);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col + 1, square4Row - 3);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        //Pruebe si la nueva posición se superpone a otra pieza. Si lo hace, cancele la rotación.
                        if (TestOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 1)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col + 2, square1Row);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col + 1, square2Row + 1);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col, square3Row + 2);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col - 1, square4Row + 3);

                        if (TestOverlap() == true)
                        {
                            rotations = 0;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else if (currentPiece == 1) //The normal L - La L normal
                {
                    //Test if piece is too close to edge of board 
                    //Pruebe si la pieza está demasiado cerca del borde del tablero
                    if (rotations == 0 & (square1Col == 8 | square1Col == 9))
                    {
                        return;
                    }
                    else if (rotations == 2 & (square1Col == 9))
                    {
                        return;
                    }

                    //If test passes, rotate piece - Si pasa la prueba, gire la pieza
                    if (rotations == 0)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col, square1Row + 2);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col + 1, square2Row + 1);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col + 2, square3Row);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col + 1, square4Row - 1);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        //Pruebe si la nueva posición se superpone a otra pieza. Si lo hace, cancele la rotación.
                        if (TestOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 1)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col + 1, square1Row);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col, square2Row - 1);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col - 1, square3Row - 2);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col - 2, square4Row - 1);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        //Pruebe si la nueva posición se superpone a otra pieza. Si lo hace, cancele la rotación.
                        if (TestOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 2)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col + 1, square1Row - 1);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col, square2Row);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col - 1, square3Row + 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col, square4Row + 2);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        //Pruebe si la nueva posición se superpone a otra pieza. Si lo hace, cancele la rotación.
                        if (TestOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 3)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col - 2, square1Row - 1);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col - 1, square2Row);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col, square3Row + 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col + 1, square4Row);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        //Pruebe si la nueva posición se superpone a otra pieza. Si lo hace, cancele la rotación.
                        if (TestOverlap() == true)
                        {
                            rotations = 0;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else if (currentPiece == 2) //The backwards L - La L al revés
                {
                    //Test if piece is too close to edge of board
                    //Pruebe si la pieza está demasiado cerca del borde del tablero
                    if (rotations == 0 & (square1Col == 0 | square1Col == 1))
                    {
                        return;
                    }
                    else if (rotations == 2 & square1Col == 0)
                    {
                        return;
                    }

                    //If test passes, rotate piece - Si pasa la prueba, gire la pieza
                    if (rotations == 0)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col - 2, square1Row + 1);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col - 1, square2Row);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col, square3Row - 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col + 1, square4Row);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        //Pruebe si la nueva posición se superpone a otra pieza. Si lo hace, cancele la rotación.
                        if (TestOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 1)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col + 1, square1Row + 1);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col, square2Row);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col - 1, square3Row - 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col, square4Row - 2);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        //Pruebe si la nueva posición se superpone a otra pieza. Si lo hace, cancele la rotación.
                        if (TestOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 2)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col + 1, square1Row);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col, square2Row + 1);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col - 1, square3Row + 2);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col - 2, square4Row + 1);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        //Pruebe si la nueva posición se superpone a otra pieza. Si lo hace, cancele la rotación.
                        if (TestOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 3)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col, square1Row - 2);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col + 1, square2Row - 1);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col + 2, square3Row);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col + 1, square4Row + 1);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        //Pruebe si la nueva posición se superpone a otra pieza. Si lo hace, cancele la rotación.
                        if (TestOverlap() == true)
                        {
                            rotations = 0;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else if (currentPiece == 3) //The normal S - La S normal
                {
                    //Test if piece is too close to edge of board
                    //Pruebe si la pieza está demasiado cerca del borde del tablero
                    if (rotations == 0 & (square1Row == 1 | square1Col == 9))
                    {
                        return;
                    }
                    else if (rotations == 1 & square1Col == 0)
                    {
                        return;
                    }

                    //If test passes, rotate piece - Si pasa la prueba, gire la pieza
                    if (rotations == 0)
                    {

                        activePiece2[0] = grid.GetControlFromPosition(square1Col + 1, square1Row - 2);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col, square2Row - 1);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col + 1, square3Row);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col, square4Row + 1);


                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        //Pruebe si la nueva posición se superpone a otra pieza. Si lo hace, cancele la rotación.
                        if (TestOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 1)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col - 1, square1Row + 2);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col, square2Row + 1);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col - 1, square3Row);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col, square4Row - 1);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        //Pruebe si la nueva posición se superpone a otra pieza. Si lo hace, cancele la rotación.
                        if (TestOverlap() == true)
                        {
                            rotations = 0;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else if (currentPiece == 4) //The backwards S - La S al reves
                {
                    //Test if piece is too close to edge of board
                    //Pruebe si la pieza está demasiado cerca del borde del tablero
                    if (rotations == 1 & square1Col == 8)
                    {
                        return;
                    }

                    //If test passes, rotate piece - Si pasa la prueba, gire la pieza
                    if (rotations == 0)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col, square1Row + 1);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col - 1, square2Row);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col, square3Row - 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col - 1, square4Row - 2);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        //Pruebe si la nueva posición se superpone a otra pieza. Si lo hace, cancele la rotación.
                        if (TestOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 1)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col, square1Row - 1);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col + 1, square2Row);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col, square3Row + 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col + 1, square4Row + 2);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        //Pruebe si la nueva posición se superpone a otra pieza. Si lo hace, cancele la rotación.
                        if (TestOverlap() == true)
                        {
                            rotations = 0;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else if (currentPiece == 5) //The square - Cuadrado
                {
                    //The square cannot rotate - El cuadrado no puede girar.
                    return;
                }
                else if (currentPiece == 6) //The pyramid - Piramide
                {
                    //Test if piece is too close to edge of board
                    //Pruebe si la pieza está demasiado cerca del borde del tablero
                    if (rotations == 1 & square1Col == 9)
                    {
                        return;
                    }
                    else if (rotations == 3 & square1Col == 0)
                    {
                        return;
                    }

                    //If test passes, rotate piece - Si pasa la prueba, gire la pieza
                    if (rotations == 0)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col, square1Row);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col, square2Row - 2);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col - 1, square3Row - 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col - 2, square4Row);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        //Pruebe si la nueva posición se superpone a otra pieza. Si lo hace, cancele la rotación.
                        if (TestOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 1)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col, square1Row);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col + 2, square2Row);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col + 1, square3Row - 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col, square4Row - 2);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        //Pruebe si la nueva posición se superpone a otra pieza. Si lo hace, cancele la rotación.
                        if (TestOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 2)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col, square1Row);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col, square2Row + 2);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col + 1, square3Row + 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col + 2, square4Row);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        //Pruebe si la nueva posición se superpone a otra pieza. Si lo hace, cancele la rotación.
                        if (TestOverlap() == true)
                        {
                            rotations++;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else if (rotations == 3)
                    {
                        activePiece2[0] = grid.GetControlFromPosition(square1Col, square1Row);
                        activePiece2[1] = grid.GetControlFromPosition(square2Col - 2, square2Row);
                        activePiece2[2] = grid.GetControlFromPosition(square3Col - 1, square3Row + 1);
                        activePiece2[3] = grid.GetControlFromPosition(square4Col, square4Row + 2);

                        //Test if new position overlaps another piece. If it does, cancel rotation.
                        //Pruebe si la nueva posición se superpone a otra pieza. Si lo hace, cancele la rotación.
                        if (TestOverlap() == true)
                        {
                            rotations = 0;
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                //Set old position of piece to white - Establecer la posición anterior de la pieza en blanco
                foreach (PictureBox square in activePiece)
                {
                    square.BackColor = Color.White;
                }

                DrawGhost();

                //Set new position of piece to that piece's color
                //Establecer nueva posición de pieza al color de esa pieza
                int x = 0;
                foreach (PictureBox square in activePiece2)
                {
                    square.BackColor = colorList[currentPiece];
                    activePiece[x] = square;
                    x++;
                }
            }
            else if (!CheckGameOver() & e.KeyCode == Keys.ShiftKey)
            {
                rotations = 0;

                // Layout options for saved piece
                //Opciones de diseño para la pieza guardada
                Control[,] savedPieceArray =
                {
                        { box219, box223, box227, box231 }, // I piece - pieza I
                        { box218, box222, box226, box227 }, // L piece - pieza L
                        { box219, box223, box227, box226 }, // J piece - pieza J
                        { box222, box223, box219, box220 }, // S piece - pieza S
                        { box218, box219, box223, box224 }, // Z piece - pieza Z
                        { box222, box223, box226, box227 }, // O piece - pieza O
                        { box223, box226, box227, box228 }  // T piece - pieza T
                };

                // Layout options for falling piece
                //Opciones de diseño para la pieza que cae
                Control[,] activePieceArray =
                {
                        { box6, box16, box26, box36 }, // I piece - pieza I
                        { box4, box14, box24, box25 }, // L piece - pieza L
                        { box5, box15, box25, box24 }, // J piece - pieza J
                        { box14, box15, box5, box6 },  // S piece - pieza S
                        { box5, box6, box16, box17 },  // Z piece - pieza Z
                        { box5, box6, box15, box16 },  // O piece - pieza O
                        { box6, box15, box16, box17 }  // T piece - pieza T
                };

                // Erase falling piece - Borrar pieza que cae
                foreach (Control x in activePiece)
                {
                    x.BackColor = Color.White;
                }

                // If no piece has been saved yet - Si aún no se ha guardado ninguna pieza
                if (savedPieceInt == -1)
                {
                    // Retrieve layout for saved piece - Recuperar el diseño de la pieza guardada
                    savedPieceInt = currentPiece;
                    for (int x = 0; x < 4; x++)
                    {
                        savedPiece[x] = savedPieceArray[savedPieceInt, x];
                    }

                    // Draw saved piece - Dibujar pieza guardada
                    savedPieceColor = colorList[savedPieceInt];
                    foreach (Control x in savedPiece)
                    {
                        x.BackColor = savedPieceColor;
                    }

                    DropNewPiece();
                }

                // If a piece has already been saved - Si una pieza ya ha sido guardada
                else
                {

                    // Erase saved piece - Borrar pieza guardada
                    foreach (Control x in savedPiece)
                    {
                        x.BackColor = Color.White;
                    }

                    // Swap pieces - intercambiar piezas
                    int savedPieceTemp = currentPiece;
                    currentPiece = savedPieceInt;
                    savedPieceInt = savedPieceTemp;
                    for (int x = 0; x < 4; x++)
                    {
                        savedPiece[x] = savedPieceArray[savedPieceInt, x];
                        activePiece2[x] = activePieceArray[currentPiece, x];
                    }

                    // Draw saved piece - Dibujar pieza guardada
                    savedPieceColor = colorList[savedPieceInt];
                    foreach (Control x in savedPiece)
                    {
                        x.BackColor = savedPieceColor;
                    }

                    // Draw falling piece - Dibujar pieza que cae
                    pieceColor = colorList[currentPiece];
                    foreach (Control square in activePiece2)
                    {
                        square.BackColor = pieceColor;
                    }

                    DrawGhost();

                    for (int x = 0; x < 4; x++)
                    {
                        activePiece[x] = activePiece2[x];
                    }
                }
            }

            else if (!CheckGameOver() & e.KeyCode == Keys.Space)
            {
                // Hard drop - caída fuerte
                for (int x = 0; x < 4; x++)
                {
                    Ghost[x].BackColor = colorList[currentPiece];
                    activePiece[x].BackColor = Color.White;
                }
                if (CheckForCompleteRows() > -1)
                {
                    ClearFullRow();
                }
                DropNewPiece();
            }
        }
    }
}