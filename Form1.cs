using Controladores;  // Para acceder a GameController, InputHandler, etc.
using MallaGrid;  // Para acceder a Malla, Nodo.
using Modelos;  // Para Estela, Items, Moto, etc.
using itemsDelJuego;

namespace TronGame
{
    public partial class Form1 : Form
    {

        //-------------------------------Atributos(Inicio)----------------------------------------
        private Malla malla;
        private Moto moto;
        private TeclasPresionadas teclasPresionadas;
        private MotoJugador motoJugador;
        public List<Bots> bots = new List<Bots>(); //Lista de bots
        private Random random = new Random(); //Utilizado para poder escoger una ubicación de spawm aleatoria de los bots-

        //Esto inicializa la entrada del Form cuando se le da dotnet run.
        private System.Windows.Forms.Timer clockTimer;

        //-------------------------------Atributos(End)----------------------------------------

        public Form1() //Constructor que inicializa todos los componentes e instancias.
        {
            InitializeComponent(); //Inicializar todos los componentes.

            //Configuración de doble buffering: Sirve para reducir el parpadeo a la hora de dibujar elementos en la pantalla.
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle; //Para bloquear la opción del mouse de poder "estirar" o "comprimir" la ventana del forms una vez iniciado, ya que si no buguearía la lista enlazada

            malla = new Malla(40,40); //Inicializamos la malla con nodos de 40x40
            malla.InicializadadorDeNodos(); //Para crear los nodos
            malla.ConectarNodos();//Para conectarlos todos, formando así una malla con cada nodo con referencias de
            //izquierda, derecha, abajo y arriba.

            motoJugador = new MotoJugador(malla.Nodos[25,20], malla); //Se crea un nuevo objeto de la clase moto y se le pasa como valor de "Posición inicial" [0,0] es decir arriba a la izquierda
            moto = motoJugador; // Si `moto` debe ser `motoJugador`
            teclasPresionadas = new TeclasPresionadas(motoJugador,this);

            // Establecer el nivel inicial de la barra de progreso al nivel de combustible de la moto
            barraCantidadDeCombustible.Value = motoJugador.Combustible;

            InicializarBots(); // Inicializamos los bots para que se puedan dibujar

            //Timer para refrescar la llamada al método de mover las motos automáticamente cuando no se preciona nada:
            clockTimer = new System.Windows.Forms.Timer();
            clockTimer.Interval = 50; // 
            clockTimer.Tick += new EventHandler(ClockTimer_Tick);
            clockTimer.Tick += (s, e) => UpdateFuelBar(); //Actualizar la barra de combustible
            clockTimer.Tick += GenerarItem;

            // Generar algunos ítems iniciales
            malla.GenerarItemAleatorio();

            this.KeyDown += new KeyEventHandler((sender,e) =>teclasPresionadas.MoverMoto(e)); //Enviamos los eventos registrados como evento tipo KeyDowm
            this.Paint += new PaintEventHandler(DibujarMalla);//Luiego dibujamos todos los componentes.

            clockTimer.Start();

        }

        private void UpdateFuelBar()//Método para actualizar la barra de combustible
        {
            if (moto != null)
            {
                barraCantidadDeCombustible.Value = Math.Max(0, Math.Min(motoJugador.Combustible, barraCantidadDeCombustible.Maximum));
            }
        }
        
        //Método para llamar a las telasPresionadas pero cuando no se preciona nada:
        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            teclasPresionadas.MoverMoto(null);
            if (motoJugador.VerificarDentroDeLimites())
            {
                clockTimer.Stop();
                MessageBox.Show("¡Has chocado con un muro!");
                Environment.Exit(0);
                return;
            }

            if (motoJugador.VerificarColisionConBots(bots))
            {
                clockTimer.Stop();
                MessageBox.Show("Colisión con un bot");
                Environment.Exit(0);
                return;
            }


            if (motoJugador.VerificarCombustible())
            {
                clockTimer.Stop();
                MessageBox.Show("¡Combustible acabado!");  // Muestra el mensaje solo una vez
                Environment.Exit(0);  // Luego cierra la aplicación
                return;
            }
        
            List<Bots> botsParaEliminar = new List<Bots>(); //Esta lista sirve para  almacenar los bots que han chocado con alguna estela
            //para posterior eliminarlos de la lista.
        
            foreach (var bot in bots) //Se itera sonre la lista principal de bots
            {
                bot.MoverAleatoriamenteBots(bots, motoJugador);//Se ingresa la lista y la instancia de la moto del jugador para luego,
                //si un bot no está en movimiento, se agrega a la lista de eliminación.
                if (!bot.estaEnMovimiento)
                {
                    botsParaEliminar.Add(bot);//Aquí
                }
            }
        
            // Eliminar los bots que chocaron
            foreach (var bot in botsParaEliminar)
            {
                bots.Remove(bot);
            }
        
            this.Invalidate();//Refrescar el dibujado de objetos por pantalla para notar los cambios.
        }

        //Método para poder dibujar la malla por pantalla, no dibuja la linkedList como tal, si no que dibuja lineas entre las filas y columnas, dando la ilusión de celdas.
        private void DibujarMalla(object? sender,PaintEventArgs e) //El "?" Es para que sepa que puede recibir objetos vacios o nulos, simplemente para eliminar la alerta.
        {
            //Nota personal para el desarrollo posterior del juego:
            /*Esto se puede utilizar para dibujar contenido adicional en cada celda, como las motos, items, poderes y demás.
                Redibujar la Malla(A futuro): 
                    Al redibujar la malla (por ejemplo, después de mover una moto), llamar al método this.Invalidate() o this.Refresh(), 
                    lo que forzará al formulario a disparar el evento Paint nuevamente, refrescando así la ventana.
            */


            Graphics g = e.Graphics; /*Nota sobre uso de Graphics: La clase Graphics proporciona los métodos necesarios para dibujar en la pantalla.
            En este caso, DrawLine se utiliza para dibujar las líneas que forman la malla.*/
            
            Pen LineasSeparadoras = new Pen(Color.Black); //Color y grosor de las líneas para ver las celdas de la malla.
            float anchoCelda = (float)this.ClientSize.Width / malla.Columnas; //Se le hizo un cambio de int a float 
            float altoCelda = (float)this.ClientSize.Height / malla.Filas;//ya que con int se estaba perdiendo ciertos decimales escenciales para poder calcular las celdas exactas de la lista, entonces se usa float para tener la mayor precisión.
            

            /*Dibujar lineas horizontales (Filas de la malla)
            Aquí se hace el calculo del ancho y el alto de cada celda, se calculan dividiendo el tamaño del formulario entre el número de columnas 
            y filas respectivamente, de modo que  la malla se ajuste al tamaño de la ventana*/
            for (int i = 0; i < malla.Filas; i++)
            {
                g.DrawLine(LineasSeparadoras,0,i * altoCelda,this.ClientSize.Width, i * altoCelda);
            }

            for (int j = 0; j <= malla.Columnas; j++)
            {
                g.DrawLine(LineasSeparadoras, j * anchoCelda, 0, j * anchoCelda, this.ClientSize.Height);
            }

            if (moto != null) //Dibujar la Moto del jugador y su estela
            {
                SolidBrush motoBrush = new SolidBrush(Color.Red);
                SolidBrush estelaBrush = new SolidBrush(Color.Blue);
        
                var NodoEstelaMotoADibujar = moto.headEstela;
                while (NodoEstelaMotoADibujar != null)
                {
                    g.FillRectangle(estelaBrush, NodoEstelaMotoADibujar.Posicion.Y * anchoCelda, NodoEstelaMotoADibujar.Posicion.X * altoCelda, anchoCelda, altoCelda);
                    NodoEstelaMotoADibujar = NodoEstelaMotoADibujar.Siguiente;
                }
        
                g.FillRectangle(motoBrush, moto.PosicionActual.Y * anchoCelda, moto.PosicionActual.X * altoCelda, anchoCelda, altoCelda);
            }


            //Dibujar bots -->

            foreach (var bot in bots)
            {
                if (bot != null)
                {
                    SolidBrush botHeadBrush = new SolidBrush(Color.DarkGreen); // Color más oscuro para la cabeza del bot
                    SolidBrush botEstelaBrush = new SolidBrush(bot.ColorEstela); // Color más claro para la estela del bot

                    // Dibujar la estela del bot primero
                    var NodoEstelaBotADibujar = bot.headEstela;
                    while (NodoEstelaBotADibujar != null)
                    {
                        g.FillRectangle(botEstelaBrush, NodoEstelaBotADibujar.Posicion.Y * anchoCelda, NodoEstelaBotADibujar.Posicion.X * altoCelda, anchoCelda, altoCelda);
                        NodoEstelaBotADibujar = NodoEstelaBotADibujar.Siguiente;
                    }

                    g.FillRectangle(botHeadBrush, bot.PosicionActual.Y * anchoCelda, bot.PosicionActual.X * altoCelda, anchoCelda, altoCelda);
                }
            }

            // Dibujar los ítems
            if (malla.ItemsEnMalla != null)
            {
                foreach (var item in malla.ItemsEnMalla)
                {
                    if (item is ItemAumentarEstela aumentarEstela)
                    {
                        if (aumentarEstela.Imagen != null)
                        {
                            g.DrawImage(aumentarEstela.Imagen, 
                                aumentarEstela.PosicionEnMalla.Y * anchoCelda, 
                                aumentarEstela.PosicionEnMalla.X * altoCelda, 
                                anchoCelda, altoCelda);
                        }
                    }

                    if (item is ItemCombustible combustible)
                    {
                        if (combustible.Imagen != null)
                        {
                            g.DrawImage(combustible.Imagen, 
                                combustible.PosicionEnMalla.Y * anchoCelda, 
                                combustible.PosicionEnMalla.X * altoCelda, 
                                anchoCelda, altoCelda);
                        }
                    }
                }
            }
        }
        private void InicializarBots()
        {   
            //Creación de una lista que contiene algunos colores para los bots.
            List<Color> coloresDisponibles = new List<Color> { Color.LightGreen, Color.OrangeRed, Color.Yellow, Color.Violet };
            for (int i = 0; i < 4; i++)//Ciclo que crea cuanta cantidad de bots se necesite.
            {
                try
                { //posicionInicial es una de 5 posibles lugares de spawm definidas en una función que las da aleatoriamente.
                    Nodo posicionInicial = PosicionInicialAleatoriaBots();
                    if (posicionInicial != null)
                    {
                        
                        Color ColorEstela = coloresDisponibles[ i % coloresDisponibles.Count];//Asignación de un color de estela distinto para cada bot:
                        int velocidadBots = random.Next( 1, 3 ); //Velocidad aleatoria para bots de forma aleatoria. funciona similar a la del jugador.

                        Bots nuevoBot = new Bots(posicionInicial,malla,ColorEstela,velocidadBots);
                        bots.Add(nuevoBot);
                    }
                    else
                    {//Se creó esto para identificar porqué no se creaban los bots.
                        Console.WriteLine($"No se pudo crear el bot {i + 1}: posición inicial nula");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al crear el bot {i + 1}: {ex.Message}");
                }
            }
        }

        private void GenerarItem(object sender, EventArgs e)
        {
            if (malla.ItemsEnMalla.Count < 5) // Limitar el número máximo de ítems en la malla
            {
                malla.GenerarItemAleatorio();
            }

        }
        private Nodo PosicionInicialAleatoriaBots() //Método encargado de dar lugares de spawn randoms para lso bots.
        {
            int randomSpawn = random.Next(5); // Cambiado a 5 para incluir el caso default
            switch (randomSpawn)
            {
                case 0: return malla.Nodos[5, 5];
                case 1: return malla.Nodos[10, 10];
                case 2: return malla.Nodos[15, 15];
                case 3: return malla.Nodos[7, 7];
                default: return malla.Nodos[20, 20];
            }
        }
    }
}