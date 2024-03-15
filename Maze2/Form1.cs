namespace Maze2
{
    /// <summary>
    /// Formuläret som vårt spel kommer att köras i
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Den första labyrinten som vår Pacman ska kämpa i.
        /// Siffrorna står för:
        /// 0 = Tomrum
        /// 1 = vägg
        /// 2 = prick
        /// 3 = Pacman
        /// 4 = spöke
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
        /// Det här är en kopia av nuvarande labyrint och allt som körs
        /// kommer att ändra i denna. Inget av ändringarna kommer att
        /// påverka våra "original"-labyrinter.
        /// </summary>
        int[,] maze;

        /// <summary>
        /// En lista som ska innehålla alla labyrinter som vi skapat
        /// </summary>
        List<int[,]> mazes = new List<int[,]>();

        /// <summary>
        /// Vilken labyrint är aktiv just nu?
        /// </summary>
        int currentMaze = 0;

        /// <summary>
        /// Hur många prickar finns i nuvarande labyrint?
        /// </summary>
        int numDots = 0;

        /// <summary>
        /// Storleken på de grafikblock vi ritar ut
        /// OBS: const betyder alltså att det är en konstant och därmed
        /// inte kan ändras under tiden spelet körs.
        /// Varje block ska alltså vara 32 x 32 pixlar stort.
        /// </summary>
        const int _blockSize = 32;

        /// <summary>
        /// Konstanter som talar om vad siffrorna i labyrinten
        /// står för
        /// </summary>
        const int _empty = 0;
        const int _wall = 1;
        const int _dot = 2;
        const int _pacman = 3;
        const int _ghost = 4;

        /// <summary>
        /// Dessa konstanter används för att hålla koll på vilken riktning
        /// som spöken och Pacman ska röra sig.
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
        /// Den riktning som Pacman färdas i
        /// </summary>
        int pacmanDirection = _noMotion;

        int pacmanNewDirection = _noMotion;

        /// <summary>
        /// Lever Pacman?
        /// </summary>
        bool _alive = true;

        /// <summary>
        /// En lista som kommer att innehålla alla spöken
        /// som hittas i labyrinten
        /// </summary>
        List<Ghost> ghosts = new List<Ghost>();



        bool scatter = true;
        /// <summary>
        /// Spelarens poäng
        /// </summary>
        int score = 0;

        /// <summary>
        /// Grundformuläret som vårt spel körs i
        /// </summary>
        public Form1()
        {
            // Starta upp formuläret/fönstret
            InitializeComponent();

            // Lägg till de labyrinter vi har i en lista
            mazes.Add(mazeOriginal1);

            // Kopiera över nuvarande oringallabyrint till den som 
            // ska användas i spelkoden
            InitMaze();
        }

        /// <summary>
        /// Gör allt grundläggande för att skapa en labyrint
        /// som spelet kan köras i
        /// </summary>
        void InitMaze()
        {
            // Skapa en kopia av nuvarande labyrint. I den kommer
            // spelet att ändra och på så vis "förstöra" den.
            // För att få en ny/korrekt labyrint så överför vi allt
            // från originallabyrinten till denna.
            maze = new int[
                mazes[currentMaze].GetLength(0),
                mazes[currentMaze].GetLength(1)];

            // Rensa bort eventuella spöken som är aktiva från föregående
            // bana
            ghosts.Clear();

            // Gå igenom hela labyrinten:
            // Kopiera allt från originallabyrinten
            // Leta fram spöken och lägg till dem i deras lista
            // Leta fram Pacman och lägg data i hans variabler
            // Räkna antalet prickar i labyrinten
            for (int i = 0; i < maze.GetLength(1); i++)
            {
                for (int j = 0; j < maze.GetLength(0); j++)
                {
                    maze[j, i] = mazes[currentMaze][j, i];

                    // Är det ett spöke?
                    if (maze[j, i] == _ghost)
                    {
                        // Lägg till spöket i listan
                        AddGhost(i, j, _noMotion, _empty, 17, 1);

                        // Spöken lämnar alltid efter sig en prick när de
                        // startar så vi ökar antalet prickar i labyrinten
                        // för att kompensera för det
                        numDots++;
                    }

                    // Är det en Pacman? (Man kan bara ha en)
                    // Skulle det råka finnas två, så kommer det att
                    // bli den "sista" Pacmanen som blir den aktiva
                    // och eventuella som fanns före kommer bara
                    // att stå still i labyrinten. MEN om ett spöke
                    // går på en av kopiorna så kommer den aktiva
                    // Pacmannen att dö!
                    if (maze[j, i] == _pacman)
                    {
                        // Spara undan koordinaterna för Pacman
                        pacmanX = i;
                        pacmanY = j;
                    }

                    // Är det en prick?
                    if (maze[j, i] == _dot)
                    {
                        // Öka på antalet prickar i labyrinten
                        numDots++;
                    }
                }
            }

            // Gå vidare till nästa labyrint. Det gör att nästa gång vi
            // kör InitMaze så kommer den att använda den nya labyrinten
            currentMaze++;

            // Har vi kommit över maxantalet labyrinter? I så fall
            // wrappar vi runt till 0. Finns det nåt bättre än modulo?
            currentMaze %= mazes.Count;
        }

        /// <summary>
        /// Ritar ut all vår grafik på skärmen. När vi kommer hit
        /// så är hela formuläret rensat, dvs det finns ingen grafik utritad
        /// Det gör att vi aldrig behöver rita ut nåt "tomrum" eftersom
        /// allt redan är tomt
        /// </summary>
        /// <param name="e">Här finns info om formuläret/fönstret</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Färgerna som vi ska använda oss av när vi ritar ut 
            // varje grej. SolidBrush är till för att göra helfyllda
            // saker.
            SolidBrush wall = new SolidBrush(Color.Blue);
            SolidBrush dot = new SolidBrush(Color.White);
            SolidBrush pacman = new SolidBrush(Color.Yellow);
            SolidBrush blinky = new SolidBrush(Color.Red);
            SolidBrush pinky = new SolidBrush(Color.Pink);
            SolidBrush inky = new SolidBrush(Color.LightBlue);
            SolidBrush clyde = new SolidBrush(Color.Orange);
            // Här tar vi ut data om vilket fönster vi ska
            // rita i. Den här metoden skulle kunna anropas av olika
            // fönster och då behöver man kunna hantera vilket fönster
            // som ska användas. I vårt spel kommer det aldrig att hända
            Graphics g = e.Graphics;

            // Vi gör loopar som går igenom hela labyrinten. Vi går igenom
            // rad för rad och på varje rad går vi genom alla kolumner
            // som finns på raden
            // Eftersom vår maze är tvådimensionell måste vi köra med
            // GetLength(vilken dimension vi vill ha). Det går alltså
            // inte att använda .length
            for (int i = 0; i < maze.GetLength(1); i++)
            {
                for (int j = 0; j < maze.GetLength(0); j++)
                {
                    // Är det en vägg?
                    if (maze[j, i] == _wall)
                    {
                        // Rita ut en rektangel i rätt färg.
                        g.FillRectangle(
                            wall,               // Färg
                            i * _blockSize,     // X-position till blockets vänstra kant
                                                // Varje block är _blocksize brett
                                                // och därför måste vi multiplicera
                                                // med den storleken för att komma
                                                // till nästa block.
                            j * _blockSize,     // Y-position till blockets översta kant
                            _blockSize,         // Bred
                            _blockSize          // Höjd
                            );
                    }

                    // Är det en prick?
                    if (maze[j, i] == _dot)
                    {
                        g.FillEllipse(
                            dot,
                            // Prickarna är hälften så stora som ett block.
                            // Vi räknar ut blocket precis som ovan
                            // sen lägger vi till ett fjärdedels block på
                            // positionen (det ska vara en fjärdedel på alla sidor
                            // eftersom vi ska rita ut en prick som är ett halft block)
                            i * _blockSize + _blockSize / 4,
                            j * _blockSize + _blockSize / 4,
                            // Fyll halva blockets storlek
                            _blockSize / 2,
                            _blockSize / 2
                            );
                    }

                    // Är det Pacman?
                    if (maze[j, i] == _pacman)
                    {
                        // Rita en elips/cirkel som funkar precis som
                        // en vägg, förutom att den är rund
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
        /// Denna körs när en tangent tryckts ned.
        /// </summary>
        /// <param name="sender">Vad skapade den här händelsen? (Form1)</param>
        /// <param name="e">Data om händelsen (t.ex. vilken tangent som trycktes ned)</param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // Om Pacman inte lever så skippar vi styrningen.
            if (!_alive)
            {
                return;
            }

            // Vi utgår från att Pacman står stilla
            // Om ingen av de korrekta tangenttesterna triggas,
            // så gäller detta och Pacman står stilla. Om någon
            // av testerna triggas skrivs _noMotion över
            // riktnignen


            // Vi kör WASD-styrning. 
            // Är A nedtryckt?
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
        /// Denna körs med jämna mellanrum så att allt i spelet
        /// uppdateras samtidigt och med samma intervall
        /// Det är timer-komponenten i vårt formulär som gör att koden
        /// körs varje gång den räknat 100 ms
        /// </summary>
        /// <param name="sender">Vad orsakade händelsen? (Timer1)</param>
        /// <param name="e">Data om händelsen (inget vi bryr oss i)</param>
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
            // Först hanterar vi Pacman
            // Spara undan Pacmans nuvarande positione, innan vi kör
            // koden för rörelsen. Det som är nuvarande position blir
            // föregående position när Pacman väl flyttar på sig. Det är
            // alltså därför det heter oldX/oldY.
            // Om något stoppar Pacman från att röra sig, såsom en vägg,
            // kommer vi att flytta tillbaka Pacman tillbaka Pacman till
            // denna position. För vi vet ju att den föregående positionen
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
            // var OK och inte blockerad på någotvis.
            int oldX = pacmanX;
            int oldY = pacmanY;

            // Om Pacman lever så kör vi igenom koden som sköter honom
            // annars skippar vi koden och gå vidare med spökena
            if (_alive)
            {

                // Vilken riktning har vi fått från keydown?
                if (pacmanDirection == _left)
                {
                    // För att gå vänster minskar vi på X-positionen
                    pacmanX--;
                }

                if (pacmanDirection == _right)
                {
                    // För att gå vänster ökar vi på X-positionen
                    pacmanX++;
                }

                if (pacmanDirection == _up)
                {
                    // För att gå upp minskar vi på Y-positionen
                    pacmanY--;
                }

                if (pacmanDirection == _down)
                {
                    // För att gå ned ökar vi på Y-positionen
                    pacmanY++;
                }

                // Krockade vi just med en vägg?
                // Vi kollar det genom att titta i arrayen på den
                // positionen vi just räknat ut i föregående rörelsekod
                if (maze[pacmanY, pacmanX] == _wall)
                {
                    // Vi krockade med en vägg, då ska Pacman
                    // gå tillbaka tills sin föregående position
                    pacmanX = oldX;
                    pacmanY = oldY;
                }

                // Ligger det en prick dit Pacman rör sig?
                if (maze[pacmanY, pacmanX] == _dot)
                {
                    // En prick!
                    // Öka poängen
                    score++;

                    // Minska hur många prickar det finns i labyrinten
                    numDots--;
                }

                // Sätt ut ett tomrum där Pacman stod förut
                // Om Pacman flyttats tillbaka till oldX, oldY
                // så kommer det att läggas ut ett tomrum som 
                // direkt, i raden efter denna, läggs över med
                // en Pacman. För då är ju oldY/oldX lika med
                // pacmanX/pacmanY
                maze[oldY, oldX] = _empty;
                // Sätt ut Pacman på den nya positionen
                // eventuellt är detta samma som den position han
                // kom från om han flyttades tillbaka pga att
                // han krockade med en vägg.

                if (pacmanX == 1 && pacmanY == 9)
                {
                    pacmanX = 17;
                }
                else if (pacmanX == 17 && pacmanY == 9)
                {
                    pacmanX = 1;
                }



                maze[pacmanY, pacmanX] = _pacman;

                // Visa den nya poängen


                // Om antalet prickar blivit noll, så är det dags
                // för nästa labyrint
                if (numDots == 0)
                {
                    // Vi skapar/läser/osv en ny labyrint
                    InitMaze();

                    // Eftersom det är en helt ny labyrint så skippar vi
                    // att köra spökena denna gång.
                    return;
                }
            }

            // Koden för att hantera spökena
            // Vi går igeom alla spökena i listan ett efter ett
            // och gör allt som ska göras för varje spöke innan
            // vi går vidare till nästa
            for (int i = 0; i < ghosts.Count; i++)
            {
                // Spara undan nuvarande position. Precis som i fallet
                // med Pacman så vill vi kunna flytta tillbaka spöket
                // om det kolliderar med något
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


                // Åt vilket håll är spöket på väg?

                // Ska det åt höger?
                if (ghosts[i].direction == _right)
                {
                    // Höger betyder att vi ökar X-positionen
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

                // Körde vi in i en Pacman?
                if (maze[ghosts[i].y, ghosts[i].x] == _pacman)
                {
                    // Döda Pacman
                    _alive = false;

                    // Ta bort Pacman från labyrinten
                    maze[pacmanY, pacmanX] = _empty;
                }

                // Har vi krockat med en vägg eller ett annat spöke?
                if (maze[ghosts[i].y, ghosts[i].x] == _wall ||
                    maze[ghosts[i].y, ghosts[i].x] == _ghost)
                {
                    // Flytta tillbaka spöket till den tidigare
                    // positionen
                    ghosts[i].x = oldX;
                    ghosts[i].y = oldY;

                    // Öka i vilken riktning spöket ska gå.
                    // Spöket kan röra sig i rikningarna som vi angav
                    // som konstanter i början av koden. Dvs, _right = 0,
                    // osv. Om spöket var på väg åt höger så ökar vi riktningen
                    // då blir den 1, vilket betyder att den ska gå neråt
                    // Osv...

                }

                // Först av allt måste vi lägga tillbaka det som låg
                // på spökets förra position. Annars skulle det "äta"
                // upp saker som prickarna. leaveBehind sätt till att
                // det är en prick när spöket skapas. Så första gången
                // kommer spöket alltid att lämnan en prick efter sig.
                maze[oldY, oldX] = ghosts[i].leaveBehind;

                // Spara undan det som ligger på spökets nya position
                // så vi inte äter/skriver över det.
                ghosts[i].leaveBehind = maze[ghosts[i].y, ghosts[i].x];

                // Sätt ut spöket på sin nya position
                maze[ghosts[i].y, ghosts[i].x] = _ghost;
            }
            this.Text = $"{numDots}";
            // Här tvingar vi Windows att rita om formuläret.
            // Då körs vår OnPaint-metod som ritar ut hela labyrinten 
            Invalidate();
        }

        /// <summary>
        /// Lägger in ett nytt spöke i spöklistan
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y postion</param>
        /// <param name="direction">Starting direction</param>
        /// <param name="leaveBehind">What to leave behind</param>
        void AddGhost(int x, int y, int direction, int leaveBehind, int targetX, int targetY)
        {
            // Vi skapar först ett nytt objekt från vår klass (Ghost)
            // Det kommer att bli satt till de defaultvärden vi har i
            // vår klass-fil.
            // Sen läggs det till i spöklistan direkt.
            ghosts.Add(new Ghost());

            // Vilket är det sista (och därmed senaste) spöket i listan?
            int lastGhost = ghosts.Count - 1;

            // Gör inställningar för spöket. De tar alltså från
            // de metodargumenten som skickas in till den här metoden
            ghosts[lastGhost].x = x;
            ghosts[lastGhost].y = y;
            ghosts[lastGhost].leaveBehind = leaveBehind;
            ghosts[lastGhost].direction = direction;
            ghosts[lastGhost].targetX = targetX;
            ghosts[lastGhost].targetY = targetY;
        }

        // När vi släpper upp en tangent ska Pacman sluta röra sig
        // Det här skulle nog kunna göras bättre, men...
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