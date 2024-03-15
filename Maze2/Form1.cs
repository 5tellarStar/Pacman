namespace Maze2
{
    /// <summary>
    /// Formul�ret som v�rt spel kommer att k�ras i
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Den f�rsta labyrinten som v�r Pacman ska k�mpa i.
        /// Siffrorna st�r f�r:
        /// 0 = Tomrum
        /// 1 = v�gg
        /// 2 = prick
        /// 3 = Pacman
        /// 4 = sp�ke
        /// </summary>
        int[,] mazeOriginal1 =
        {
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            { 1,2,2,2,2,2,2,2,2,1,2,2,2,2,2,2,2,2,1},
            { 1,2,1,1,2,1,1,1,2,1,2,1,1,1,2,1,1,2,1},
            { 1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
            { 1,2,1,1,2,1,2,1,1,1,1,1,2,1,2,1,1,2,1},
            { 1,2,2,2,2,1,2,2,2,1,2,2,2,1,2,2,2,2,1},
            { 1,1,1,1,2,1,1,1,0,1,0,1,1,1,2,1,1,1,1},
            { 0,0,0,1,2,1,0,0,0,4,0,0,0,1,2,1,0,0,0},
            { 1,1,1,1,2,1,0,1,1,1,1,1,0,1,2,1,1,1,1},
            { 0,0,0,0,2,0,0,1,0,0,0,1,0,0,2,0,0,0,0},
            { 1,1,1,1,2,1,0,1,1,1,1,1,0,1,2,1,1,1,1},
            { 0,0,0,1,2,1,0,0,0,0,0,0,0,1,2,1,0,0,0},
            { 1,1,1,1,2,1,0,1,1,1,1,1,0,1,2,1,1,1,1},
            { 1,2,2,2,2,2,2,2,2,1,2,2,2,2,2,2,2,2,1},
            { 1,2,1,1,2,1,1,1,2,1,2,1,1,1,2,1,1,2,1},
            { 1,2,2,1,2,2,2,2,2,3,2,2,2,2,2,1,2,2,1},
            { 1,1,2,1,2,1,2,1,1,1,1,1,2,1,2,1,2,1,1},
            { 1,2,2,2,2,1,2,2,2,1,2,2,2,1,2,2,2,2,1},
            { 1,2,1,1,1,1,1,1,2,1,2,1,1,1,1,1,1,2,1},
            { 1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1},
            { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
        };

        /// <summary>
        /// Det h�r �r en kopia av nuvarande labyrint och allt som k�rs
        /// kommer att �ndra i denna. Inget av �ndringarna kommer att
        /// p�verka v�ra "original"-labyrinter.
        /// </summary>
        int[,] maze;

        /// <summary>
        /// En lista som ska inneh�lla alla labyrinter som vi skapat
        /// </summary>
        List<int[,]> mazes = new List<int[,]>();

        /// <summary>
        /// Vilken labyrint �r aktiv just nu?
        /// </summary>
        int currentMaze = 0;

        /// <summary>
        /// Hur m�nga prickar finns i nuvarande labyrint?
        /// </summary>
        int numDots = 0;

        /// <summary>
        /// Storleken p� de grafikblock vi ritar ut
        /// OBS: const betyder allts� att det �r en konstant och d�rmed
        /// inte kan �ndras under tiden spelet k�rs.
        /// Varje block ska allts� vara 32 x 32 pixlar stort.
        /// </summary>
        const int _blockSize = 32;

        /// <summary>
        /// Konstanter som talar om vad siffrorna i labyrinten
        /// st�r f�r
        /// </summary>
        const int _empty = 0;
        const int _wall = 1;
        const int _dot = 2;
        const int _pacman = 3;
        const int _ghost = 4;

        /// <summary>
        /// Dessa konstanter anv�nds f�r att h�lla koll p� vilken riktning
        /// som sp�ken och Pacman ska r�ra sig.
        /// </summary>
        const int _noMotion = -1;
        const int _right = 0;
        const int _down = 1;
        const int _left = 2;
        const int _up = 3;

        /// <summary>
        /// Pacmans X-position
        /// </summary>
        int pacmanX;

        /// <summary>
        /// Pacmans Y-position
        /// </summary>
        int pacmanY;

        /// <summary>
        /// Den riktning som Pacman f�rdas i
        /// </summary>
        int pacmanDirection = _noMotion;

        int pacmanNewDirection = _noMotion;

        /// <summary>
        /// Lever Pacman?
        /// </summary>
        bool _alive = true;

        /// <summary>
        /// En lista som kommer att inneh�lla alla sp�ken
        /// som hittas i labyrinten
        /// </summary>
        List<Ghost> ghosts = new List<Ghost>();



        bool scatter = true;
        /// <summary>
        /// Spelarens po�ng
        /// </summary>
        int score = 0;

        /// <summary>
        /// Grundformul�ret som v�rt spel k�rs i
        /// </summary>
        public Form1()
        {
            // Starta upp formul�ret/f�nstret
            InitializeComponent();

            // L�gg till de labyrinter vi har i en lista
            mazes.Add(mazeOriginal1);

            // Kopiera �ver nuvarande oringallabyrint till den som 
            // ska anv�ndas i spelkoden
            InitMaze();
        }

        /// <summary>
        /// G�r allt grundl�ggande f�r att skapa en labyrint
        /// som spelet kan k�ras i
        /// </summary>
        void InitMaze()
        {
            // Skapa en kopia av nuvarande labyrint. I den kommer
            // spelet att �ndra och p� s� vis "f�rst�ra" den.
            // F�r att f� en ny/korrekt labyrint s� �verf�r vi allt
            // fr�n originallabyrinten till denna.
            maze = new int[
                mazes[currentMaze].GetLength(0),
                mazes[currentMaze].GetLength(1)];

            // Rensa bort eventuella sp�ken som �r aktiva fr�n f�reg�ende
            // bana
            ghosts.Clear();

            // G� igenom hela labyrinten:
            // Kopiera allt fr�n originallabyrinten
            // Leta fram sp�ken och l�gg till dem i deras lista
            // Leta fram Pacman och l�gg data i hans variabler
            // R�kna antalet prickar i labyrinten
            for (int i = 0; i < maze.GetLength(1); i++)
            {
                for (int j = 0; j < maze.GetLength(0); j++)
                {
                    maze[j, i] = mazes[currentMaze][j, i];

                    // �r det ett sp�ke?
                    if (maze[j, i] == _ghost)
                    {
                        // L�gg till sp�ket i listan
                        AddGhost(i, j, _noMotion, _empty, 17, 1);

                        // Sp�ken l�mnar alltid efter sig en prick n�r de
                        // startar s� vi �kar antalet prickar i labyrinten
                        // f�r att kompensera f�r det
                        numDots++;
                    }

                    // �r det en Pacman? (Man kan bara ha en)
                    // Skulle det r�ka finnas tv�, s� kommer det att
                    // bli den "sista" Pacmanen som blir den aktiva
                    // och eventuella som fanns f�re kommer bara
                    // att st� still i labyrinten. MEN om ett sp�ke
                    // g�r p� en av kopiorna s� kommer den aktiva
                    // Pacmannen att d�!
                    if (maze[j, i] == _pacman)
                    {
                        // Spara undan koordinaterna f�r Pacman
                        pacmanX = i;
                        pacmanY = j;
                    }

                    // �r det en prick?
                    if (maze[j, i] == _dot)
                    {
                        // �ka p� antalet prickar i labyrinten
                        numDots++;
                    }
                }
            }

            // G� vidare till n�sta labyrint. Det g�r att n�sta g�ng vi
            // k�r InitMaze s� kommer den att anv�nda den nya labyrinten
            currentMaze++;

            // Har vi kommit �ver maxantalet labyrinter? I s� fall
            // wrappar vi runt till 0. Finns det n�t b�ttre �n modulo?
            currentMaze %= mazes.Count;
        }

        /// <summary>
        /// Ritar ut all v�r grafik p� sk�rmen. N�r vi kommer hit
        /// s� �r hela formul�ret rensat, dvs det finns ingen grafik utritad
        /// Det g�r att vi aldrig beh�ver rita ut n�t "tomrum" eftersom
        /// allt redan �r tomt
        /// </summary>
        /// <param name="e">H�r finns info om formul�ret/f�nstret</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // F�rgerna som vi ska anv�nda oss av n�r vi ritar ut 
            // varje grej. SolidBrush �r till f�r att g�ra helfyllda
            // saker.
            SolidBrush wall = new SolidBrush(Color.Blue);
            SolidBrush dot = new SolidBrush(Color.White);
            SolidBrush pacman = new SolidBrush(Color.Yellow);
            SolidBrush blinky = new SolidBrush(Color.Red);
            SolidBrush pinky = new SolidBrush(Color.Pink);
            SolidBrush inky = new SolidBrush(Color.LightBlue);
            SolidBrush clyde = new SolidBrush(Color.Orange);
            // H�r tar vi ut data om vilket f�nster vi ska
            // rita i. Den h�r metoden skulle kunna anropas av olika
            // f�nster och d� beh�ver man kunna hantera vilket f�nster
            // som ska anv�ndas. I v�rt spel kommer det aldrig att h�nda
            Graphics g = e.Graphics;

            // Vi g�r loopar som g�r igenom hela labyrinten. Vi g�r igenom
            // rad f�r rad och p� varje rad g�r vi genom alla kolumner
            // som finns p� raden
            // Eftersom v�r maze �r tv�dimensionell m�ste vi k�ra med
            // GetLength(vilken dimension vi vill ha). Det g�r allts�
            // inte att anv�nda .length
            for (int i = 0; i < maze.GetLength(1); i++)
            {
                for (int j = 0; j < maze.GetLength(0); j++)
                {
                    // �r det en v�gg?
                    if (maze[j, i] == _wall)
                    {
                        // Rita ut en rektangel i r�tt f�rg.
                        g.FillRectangle(
                            wall,               // F�rg
                            i * _blockSize,     // X-position till blockets v�nstra kant
                                                // Varje block �r _blocksize brett
                                                // och d�rf�r m�ste vi multiplicera
                                                // med den storleken f�r att komma
                                                // till n�sta block.
                            j * _blockSize,     // Y-position till blockets �versta kant
                            _blockSize,         // Bred
                            _blockSize          // H�jd
                            );
                    }

                    // �r det en prick?
                    if (maze[j, i] == _dot)
                    {
                        g.FillEllipse(
                            dot,
                            // Prickarna �r h�lften s� stora som ett block.
                            // Vi r�knar ut blocket precis som ovan
                            // sen l�gger vi till ett fj�rdedels block p�
                            // positionen (det ska vara en fj�rdedel p� alla sidor
                            // eftersom vi ska rita ut en prick som �r ett halft block)
                            i * _blockSize + _blockSize / 4,
                            j * _blockSize + _blockSize / 4,
                            // Fyll halva blockets storlek
                            _blockSize / 2,
                            _blockSize / 2
                            );
                    }

                    // �r det Pacman?
                    if (maze[j, i] == _pacman)
                    {
                        // Rita en elips/cirkel som funkar precis som
                        // en v�gg, f�rutom att den �r rund
                        g.FillEllipse(
                            pacman,
                            i * _blockSize,
                            j * _blockSize,
                            _blockSize,
                            _blockSize
                            );
                    }
                }
            }
            for (int i = 0; i < ghosts.Count; i++)
            {
                if (i == 0)
                {
                    g.FillEllipse(blinky, ghosts[i].x * _blockSize, ghosts[i].y * _blockSize, _blockSize, _blockSize);
                    g.FillRectangle(blinky, ghosts[i].x * _blockSize, ghosts[i].y * _blockSize + _blockSize / 2, _blockSize, _blockSize / 2);
                }
                if (i == 1)
                {
                    g.FillEllipse(pinky, ghosts[i].x * _blockSize, ghosts[i].y * _blockSize, _blockSize, _blockSize);
                    g.FillRectangle(pinky, ghosts[i].x * _blockSize, ghosts[i].y * _blockSize + _blockSize / 2, _blockSize, _blockSize / 2);
                }
                if (i == 2)
                {
                    g.FillEllipse(inky, ghosts[i].x * _blockSize, ghosts[i].y * _blockSize, _blockSize, _blockSize);
                    g.FillRectangle(inky, ghosts[i].x * _blockSize, ghosts[i].y * _blockSize + _blockSize / 2, _blockSize, _blockSize / 2);
                }
                if (i == 3)
                {
                    g.FillEllipse(clyde, ghosts[i].x * _blockSize, ghosts[i].y * _blockSize, _blockSize, _blockSize);
                    g.FillRectangle(clyde, ghosts[i].x * _blockSize, ghosts[i].y * _blockSize + _blockSize / 2, _blockSize, _blockSize / 2);
                }
            }
        }

        /// <summary>
        /// Denna k�rs n�r en tangent tryckts ned.
        /// </summary>
        /// <param name="sender">Vad skapade den h�r h�ndelsen? (Form1)</param>
        /// <param name="e">Data om h�ndelsen (t.ex. vilken tangent som trycktes ned)</param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Om Pacman inte lever s� skippar vi styrningen.
            if (!_alive)
            {
                return;
            }

            // Vi utg�r fr�n att Pacman st�r stilla
            // Om ingen av de korrekta tangenttesterna triggas,
            // s� g�ller detta och Pacman st�r stilla. Om n�gon
            // av testerna triggas skrivs _noMotion �ver
            // riktnignen


            // Vi k�r WASD-styrning. 
            // �r A nedtryckt?
            if (e.KeyCode == Keys.A || pacmanNewDirection == _left)
            {
                pacmanNewDirection = _left;
            }

            if (e.KeyCode == Keys.D || pacmanNewDirection == _right)
            {
                pacmanNewDirection = _right;

            }

            if (e.KeyCode == Keys.W || pacmanNewDirection == _up)
            {
                pacmanNewDirection = _up;
            }

            if (e.KeyCode == Keys.S || pacmanNewDirection == _down)
            {
                pacmanNewDirection = _down;
            }
        }

        /// <summary>
        /// Denna k�rs med j�mna mellanrum s� att allt i spelet
        /// uppdateras samtidigt och med samma intervall
        /// Det �r timer-komponenten i v�rt formul�r som g�r att koden
        /// k�rs varje g�ng den r�knat 100 ms
        /// </summary>
        /// <param name="sender">Vad orsakade h�ndelsen? (Timer1)</param>
        /// <param name="e">Data om h�ndelsen (inget vi bryr oss i)</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if ( numDots == 121 && ghosts.Count == 2)
            {
                AddGhost(9, 7 , _left, _empty, 9, 7);
            }
            if (numDots == 100 && ghosts.Count == 3)
            {
                AddGhost(9, 7, _left, _empty, 9, 7);
            }
            // F�rst hanterar vi Pacman
            // Spara undan Pacmans nuvarande positione, innan vi k�r
            // koden f�r r�relsen. Det som �r nuvarande position blir
            // f�reg�ende position n�r Pacman v�l flyttar p� sig. Det �r
            // allts� d�rf�r det heter oldX/oldY.
            // Om n�got stoppar Pacman fr�n att r�ra sig, s�som en v�gg,
            // kommer vi att flytta tillbaka Pacman tillbaka Pacman till
            // denna position. F�r vi vet ju att den f�reg�ende positionen
            if (pacmanNewDirection == _left && maze[pacmanY, pacmanX - 1] != _wall)
            {
                pacmanDirection = _left;
            }

            if (pacmanNewDirection == _right && maze[pacmanY, pacmanX + 1] != _wall)
            {
                pacmanDirection = _right;
            }

            if (pacmanNewDirection == _up && maze[pacmanY - 1, pacmanX] != _wall)
            {
                pacmanDirection = _up;
            }

            if (pacmanNewDirection == _down && maze[pacmanY + 1, pacmanX] != _wall)
            {
                pacmanDirection = _down;
            }
            // var OK och inte blockerad p� n�gotvis.
            int oldX = pacmanX;
            int oldY = pacmanY;

            // Om Pacman lever s� k�r vi igenom koden som sk�ter honom
            // annars skippar vi koden och g� vidare med sp�kena
            if (_alive)
            {

                // Vilken riktning har vi f�tt fr�n keydown?
                if (pacmanDirection == _left)
                {
                    // F�r att g� v�nster minskar vi p� X-positionen
                    pacmanX--;
                }

                if (pacmanDirection == _right)
                {
                    // F�r att g� v�nster �kar vi p� X-positionen
                    pacmanX++;
                }

                if (pacmanDirection == _up)
                {
                    // F�r att g� upp minskar vi p� Y-positionen
                    pacmanY--;
                }

                if (pacmanDirection == _down)
                {
                    // F�r att g� ned �kar vi p� Y-positionen
                    pacmanY++;
                }

                // Krockade vi just med en v�gg?
                // Vi kollar det genom att titta i arrayen p� den
                // positionen vi just r�knat ut i f�reg�ende r�relsekod
                if (maze[pacmanY, pacmanX] == _wall)
                {
                    // Vi krockade med en v�gg, d� ska Pacman
                    // g� tillbaka tills sin f�reg�ende position
                    pacmanX = oldX;
                    pacmanY = oldY;
                }

                // Ligger det en prick dit Pacman r�r sig?
                if (maze[pacmanY, pacmanX] == _dot)
                {
                    // En prick!
                    // �ka po�ngen
                    score++;

                    // Minska hur m�nga prickar det finns i labyrinten
                    numDots--;
                }

                // S�tt ut ett tomrum d�r Pacman stod f�rut
                // Om Pacman flyttats tillbaka till oldX, oldY
                // s� kommer det att l�ggas ut ett tomrum som 
                // direkt, i raden efter denna, l�ggs �ver med
                // en Pacman. F�r d� �r ju oldY/oldX lika med
                // pacmanX/pacmanY
                maze[oldY, oldX] = _empty;
                // S�tt ut Pacman p� den nya positionen
                // eventuellt �r detta samma som den position han
                // kom fr�n om han flyttades tillbaka pga att
                // han krockade med en v�gg.

                if (pacmanX == 1 && pacmanY == 9)
                {
                    pacmanX = 17;
                }
                else if (pacmanX == 17 && pacmanY == 9)
                {
                    pacmanX = 1;
                }



                maze[pacmanY, pacmanX] = _pacman;

                // Visa den nya po�ngen


                // Om antalet prickar blivit noll, s� �r det dags
                // f�r n�sta labyrint
                if (numDots == 0)
                {
                    // Vi skapar/l�ser/osv en ny labyrint
                    InitMaze();

                    // Eftersom det �r en helt ny labyrint s� skippar vi
                    // att k�ra sp�kena denna g�ng.
                    return;
                }
            }

            // Koden f�r att hantera sp�kena
            // Vi g�r igeom alla sp�kena i listan ett efter ett
            // och g�r allt som ska g�ras f�r varje sp�ke innan
            // vi g�r vidare till n�sta
            for (int i = 0; i < ghosts.Count; i++)
            {
                // Spara undan nuvarande position. Precis som i fallet
                // med Pacman s� vill vi kunna flytta tillbaka sp�ket
                // om det kolliderar med n�got
                oldX = ghosts[i].x;
                oldY = ghosts[i].y;

                if (i == 0)
                {
                    if (scatter)
                    {
                        ghosts[i].targetX = 18;
                        ghosts[i].targetY = 0;
                    }
                    else
                    {
                        ghosts[i].targetX = pacmanX;
                        ghosts[i].targetY = pacmanY;
                    }
                }
                else if (i == 2)
                {
                    if (scatter)
                    {
                        ghosts[i].targetX = 18;
                        ghosts[i].targetY = 20;
                    }
                    else
                    {
                        if (pacmanDirection == _right)
                        {
                            ghosts[i].targetX = (pacmanX + 1 - ghosts[0].x) * 2;
                            ghosts[i].targetY = (pacmanY - ghosts[0].y * 2) * 2;
                        }
                        else if (pacmanDirection == _left)
                        {
                            ghosts[i].targetX = (pacmanX - 1 - ghosts[0].x) * 2;
                            ghosts[i].targetY = (pacmanY - ghosts[0].y * 2) * 2;
                        }
                        else if (pacmanDirection == _up)
                        {
                            ghosts[i].targetX = (pacmanX + 1 - ghosts[0].x) * 2;
                            ghosts[i].targetY = (pacmanY - 1 - ghosts[0].y) * 2;
                        }
                        else if (pacmanDirection == _down)
                        {
                            ghosts[i].targetX = (pacmanX - ghosts[0].x) * 2;
                            ghosts[i].targetY = (pacmanY + 1 - ghosts[0].y) * 2;
                        }
                    }
                }
                else if (i == 1)
                {
                    if (scatter)
                    {
                        ghosts[i].targetX = 0;
                        ghosts[i].targetY = 0;
                    }
                    else
                    {
                        if (pacmanDirection == _right)
                        {
                            ghosts[i].targetX = pacmanX + 3;
                            ghosts[i].targetY = pacmanY;
                        }
                        else if (pacmanDirection == _left)
                        {
                            ghosts[i].targetX = pacmanX - 3;
                            ghosts[i].targetY = pacmanY;
                        }
                        else if (pacmanDirection == _up)
                        {
                            ghosts[i].targetX = pacmanX + 3;
                            ghosts[i].targetY = pacmanY - 3;
                        }
                        else if (pacmanDirection == _down)
                        {
                            ghosts[i].targetX = pacmanX;
                            ghosts[i].targetY = pacmanY + 3;
                        }
                    }
                }
                else if (i == 3)
                {
                    if (scatter || MathF.Sqrt((ghosts[i].targetX - (ghosts[i].x)) * (ghosts[i].targetX - (ghosts[i].x)) + (ghosts[i].targetY - ghosts[i].y) * (ghosts[i].targetY - ghosts[i].y)) <= 8)
                    {
                        ghosts[i].targetX = 0;
                        ghosts[i].targetY = 20;
                    }
                    else
                    {
                        ghosts[i].targetX = pacmanX;
                        ghosts[i].targetY = pacmanY;
                    }
                }

                bool canWalkRight = maze[ghosts[i].y, ghosts[i].x + 1] != _wall && ghosts[i].direction != _left;
                bool canWalkLeft = maze[ghosts[i].y, ghosts[i].x - 1] != _wall && ghosts[i].direction != _right;
                bool canWalkUp = maze[ghosts[i].y - 1, ghosts[i].x] != _wall && ghosts[i].direction != _down;
                bool canWalkDown = maze[ghosts[i].y + 1, ghosts[i].x] != _wall && ghosts[i].direction != _up;

                int lengthToTargetRight = (ghosts[i].targetX - (ghosts[i].x + 1)) * (ghosts[i].targetX - (ghosts[i].x + 1)) + (ghosts[i].targetY - ghosts[i].y) * (ghosts[i].targetY - ghosts[i].y);
                int lengthToTargetLeft = (ghosts[i].targetX - (ghosts[i].x - 1)) * (ghosts[i].targetX - (ghosts[i].x - 1)) + (ghosts[i].targetY - ghosts[i].y) * (ghosts[i].targetY - ghosts[i].y);
                int lengthToTargetUp = (ghosts[i].targetX - ghosts[i].x) * (ghosts[i].targetX - ghosts[i].x) + (ghosts[i].targetY - (ghosts[i].y - 1)) * (ghosts[i].targetY - (ghosts[i].y - 1));
                int lengthToTargetDown = (ghosts[i].targetX - ghosts[i].x) * (ghosts[i].targetX - ghosts[i].x) + (ghosts[i].targetY - (ghosts[i].y + 1)) * (ghosts[i].targetY - (ghosts[i].y + 1));

                if ((lengthToTargetRight < lengthToTargetLeft || !canWalkLeft) && (lengthToTargetRight < lengthToTargetUp || !canWalkUp) && (lengthToTargetRight < lengthToTargetDown || !canWalkDown) && canWalkRight)
                {
                    this.Text = $"right {canWalkRight} {canWalkLeft} {canWalkUp} {canWalkDown}";
                    ghosts[i].direction = _right;
                }
                else if ((lengthToTargetLeft < lengthToTargetUp || !canWalkUp) && (lengthToTargetLeft < lengthToTargetDown || !canWalkDown) && (lengthToTargetLeft < lengthToTargetRight || !canWalkRight) && canWalkLeft)
                {
                    this.Text = $"left {canWalkRight} {canWalkLeft} {canWalkUp} {canWalkDown}";
                    ghosts[i].direction = _left;
                }
                else if ((lengthToTargetUp < lengthToTargetDown || !canWalkDown) && (lengthToTargetUp < lengthToTargetRight || !canWalkRight) && (lengthToTargetUp < lengthToTargetLeft || !canWalkLeft) && canWalkUp)
                {
                    this.Text = $"up {canWalkRight} {canWalkLeft} {canWalkUp} {canWalkDown}";
                    ghosts[i].direction = _up;
                }
                else if ((lengthToTargetDown < lengthToTargetRight || !canWalkRight) && (lengthToTargetDown < lengthToTargetLeft || !canWalkLeft) && (lengthToTargetDown < lengthToTargetUp || !canWalkUp) && canWalkDown)
                {
                    this.Text = $"down {canWalkRight} {canWalkLeft} {canWalkUp} {canWalkDown}";
                    ghosts[i].direction = _down;
                }
                else
                {
                    this.Text = $"no {canWalkRight} {canWalkLeft} {canWalkUp} {canWalkDown}";
                }


                // �t vilket h�ll �r sp�ket p� v�g?

                // Ska det �t h�ger?
                if (ghosts[i].direction == _right)
                {
                    // H�ger betyder att vi �kar X-positionen
                    ghosts[i].x++;
                }

                if (ghosts[i].direction == _down)
                {
                    ghosts[i].y++;
                }

                if (ghosts[i].direction == _left)
                {
                    ghosts[i].x--;
                }

                if (ghosts[i].direction == _up)
                {
                    ghosts[i].y--;
                }

                // K�rde vi in i en Pacman?
                if (maze[ghosts[i].y, ghosts[i].x] == _pacman)
                {
                    // D�da Pacman
                    _alive = false;

                    // Ta bort Pacman fr�n labyrinten
                    maze[pacmanY, pacmanX] = _empty;
                }

                // Har vi krockat med en v�gg eller ett annat sp�ke?
                if (maze[ghosts[i].y, ghosts[i].x] == _wall ||
                    maze[ghosts[i].y, ghosts[i].x] == _ghost)
                {
                    // Flytta tillbaka sp�ket till den tidigare
                    // positionen
                    ghosts[i].x = oldX;
                    ghosts[i].y = oldY;

                    // �ka i vilken riktning sp�ket ska g�.
                    // Sp�ket kan r�ra sig i rikningarna som vi angav
                    // som konstanter i b�rjan av koden. Dvs, _right = 0,
                    // osv. Om sp�ket var p� v�g �t h�ger s� �kar vi riktningen
                    // d� blir den 1, vilket betyder att den ska g� ner�t
                    // Osv...

                }

                // F�rst av allt m�ste vi l�gga tillbaka det som l�g
                // p� sp�kets f�rra position. Annars skulle det "�ta"
                // upp saker som prickarna. leaveBehind s�tt till att
                // det �r en prick n�r sp�ket skapas. S� f�rsta g�ngen
                // kommer sp�ket alltid att l�mnan en prick efter sig.
                maze[oldY, oldX] = ghosts[i].leaveBehind;

                // Spara undan det som ligger p� sp�kets nya position
                // s� vi inte �ter/skriver �ver det.
                ghosts[i].leaveBehind = maze[ghosts[i].y, ghosts[i].x];

                // S�tt ut sp�ket p� sin nya position
                maze[ghosts[i].y, ghosts[i].x] = _ghost;
            }
            this.Text = $"{numDots}";
            // H�r tvingar vi Windows att rita om formul�ret.
            // D� k�rs v�r OnPaint-metod som ritar ut hela labyrinten 
            Invalidate();
        }

        /// <summary>
        /// L�gger in ett nytt sp�ke i sp�klistan
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y postion</param>
        /// <param name="direction">Starting direction</param>
        /// <param name="leaveBehind">What to leave behind</param>
        void AddGhost(int x, int y, int direction, int leaveBehind, int targetX, int targetY)
        {
            // Vi skapar f�rst ett nytt objekt fr�n v�r klass (Ghost)
            // Det kommer att bli satt till de defaultv�rden vi har i
            // v�r klass-fil.
            // Sen l�ggs det till i sp�klistan direkt.
            ghosts.Add(new Ghost());

            // Vilket �r det sista (och d�rmed senaste) sp�ket i listan?
            int lastGhost = ghosts.Count - 1;

            // G�r inst�llningar f�r sp�ket. De tar allts� fr�n
            // de metodargumenten som skickas in till den h�r metoden
            ghosts[lastGhost].x = x;
            ghosts[lastGhost].y = y;
            ghosts[lastGhost].leaveBehind = leaveBehind;
            ghosts[lastGhost].direction = direction;
            ghosts[lastGhost].targetX = targetX;
            ghosts[lastGhost].targetY = targetY;
        }

        // N�r vi sl�pper upp en tangent ska Pacman sluta r�ra sig
        // Det h�r skulle nog kunna g�ras b�ttre, men...
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {

        }

        int scatterTimer = 0;
 
        private void mode_Tick(object sender, EventArgs e)
        {
            if (scatterTimer == 3)
            {
                AddGhost(9, 7, _left, _empty, 9, 7);
            }
            scatterTimer++;
            if(currentMaze == 1)
            {
                if(scatterTimer == 8 || scatterTimer == 35 || scatterTimer == 60 || scatterTimer == 85) 
                {
                    scatter = false;
                }
                if (scatterTimer == 28 || scatterTimer == 55 || scatterTimer == 80)
                {
                    scatter = true;
                }
            }
            if (currentMaze >= 2 && currentMaze <= 4)
            {
                if (scatterTimer == 8 || scatterTimer == 35 || scatterTimer == 60)
                {
                    scatter = false;
                }
                if (scatterTimer == 28 || scatterTimer == 55)
                {
                    scatter = true;
                }
            }
            else
            {
                if (scatterTimer == 6 || scatterTimer == 31 || scatterTimer == 56)
                {
                    scatter = false;
                }
                if (scatterTimer == 26 || scatterTimer == 51)
                {
                    scatter = true;
                }
            }
        }
    }
}