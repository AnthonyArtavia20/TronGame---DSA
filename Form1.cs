using Controladores;  // Para acceder a BotsControllers, Teclas presionadas, etc.
using MallaGrid;  // Para acceder a Malla, Nodo.
using Modelos;  // Para Estela, Moto, etc.
using itemsDelJuego;
using poderesDelJuego; //Para poder acceder a los Items

namespace TronGame
{
    public partial class Form1 : Form
    {

        //-------------------------------Atributos(Inicio)----------------------------------------
        private Malla malla;
        private Moto moto;
        private TeclasPresionadas teclasPresionadas;
        private MotoJugador motoJugador; //Dato tipo motoJugador.
        public List<Bots> bots = new List<Bots>(); //Lista de bots
        private Random random = new Random(); //Utilizado para poder escoger una ubicación de spawm aleatoria de los bots-
        private System.Windows.Forms.Timer clockTimer;
        private int contadorTicks = 0; //Para poder controlar la generación de items en el juego.


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
            clockTimer.Interval = 100; // 
            clockTimer.Tick += new EventHandler(ClockTimer_Tick);
            clockTimer.Tick += (s, e) => UpdateFuelBar(); //Actualizar la barra de combustible

            for (int i = 0; i < 10; i++) // Genera 10 ítems al inicio
            {
                malla.GenerarObjetoAleatorio();
            }

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler((sender,e) =>teclasPresionadas.MoverMoto(e)); //Enviamos los eventos registrados como evento tipo KeyDowm
            this.Paint += new PaintEventHandler(DibujarMalla);//Luiego dibujamos todos los componentes.

            clockTimer.Start();
        }

        // Sobrescribe el método OnPaint para dibujar la pila de poderes

    private void DibujarPilaDePoderes(Graphics g)
    {
        if (motoJugador.poderesPila.Tope == null) return; // No dibujar si la pila está vacía
    
        int y = 10;
        int x = 1000;
        int width = 150;
        int height = 20;
    
        // Dibujar un fondo para la lista de poderes
        g.FillRectangle(new SolidBrush(Color.FromArgb(128, 0, 0, 0)), x, y, width, height * 5);
    
        var nodoActual = motoJugador.poderesPila.Tope;
        while (nodoActual != null)
        {
            if (nodoActual.PoderAlmacenado == null)
            {
                return; //Se agregó esto para evitar el aviso de que podía haber una referencia nula.
            }
            string poderNombre = nodoActual.PoderAlmacenado.GetType().Name;
            g.DrawString(poderNombre, this.Font, Brushes.Yellow, new Point(x + 5, y + 5));
            y += height;
            nodoActual = nodoActual.siguiente;
        }
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
                //MessageBox.Show("¡Has chocado con un muro!");
                //Environment.Exit(0);
                //return;
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
        
            foreach (var bot in bots) //Se itera sobre la lista principal de bots
            {
                bot.MoverAleatoriamenteBots(bots, motoJugador);//Se ingresa la lista y la instancia de la moto del jugador para luego,
                //si un bot no está en movimiento, se agrega a la lista de eliminación.
                if (!bot.estaEnMovimiento)
                {
                    botsParaEliminar.Add(bot);//Aquí se agrega el bot colisionado(se puso en false el movimiento) en una lista, para luego eliminarlos.
                }
            }
        
            // Eliminar los bots que chocaron, eliminandolos de la nueva lista creada solo para almacenarlos.
            foreach (var bot in botsParaEliminar)
            {
                bots.Remove(bot);
            }

            contadorTicks++;
            if (contadorTicks >= 5) // Genera un nuevo ítem cada 10 ticks
            {
                malla.GenerarObjetoAleatorio();
                contadorTicks = 0;
            }

            this.Invalidate(); // Redibujar la pantalla
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
                SolidBrush estelaBrush = new SolidBrush(moto.ColorEstela);
        
                var NodoEstelaMotoADibujar = moto.headEstela;
                while (NodoEstelaMotoADibujar != null)
                {
                    if (NodoEstelaMotoADibujar.Posicion == null)
                        {
                            return; //Se agregó esto para evitar el aviso de que podía haber una referencia nula.
                        }
                    g.FillRectangle(estelaBrush, NodoEstelaMotoADibujar.Posicion.Y * anchoCelda, NodoEstelaMotoADibujar.Posicion.X * altoCelda, anchoCelda, altoCelda);
                    NodoEstelaMotoADibujar = NodoEstelaMotoADibujar.Siguiente;
                }
        
                g.FillRectangle(motoBrush, moto.PosicionActual.Y * anchoCelda, moto.PosicionActual.X * altoCelda, anchoCelda, altoCelda);
            }
            DibujarPilaDePoderes(e.Graphics);


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
                        if (NodoEstelaBotADibujar.Posicion == null)
                        {
                            return; //Se agregó esto para evitar el aviso de que podía haber una referencia nula.
                        }
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
                        if (aumentarEstela.PosicionEnMalla == null)
                        {
                            return;
                        }
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
                        if (combustible.PosicionEnMalla == null)
                        {
                            return; //Se agregó esto para evitar que diera u aviso de que podía ser null
                        }
                        if (combustible.Imagen != null)
                        {
                            g.DrawImage(combustible.Imagen, 
                                combustible.PosicionEnMalla.Y * anchoCelda, 
                                combustible.PosicionEnMalla.X * altoCelda, 
                                anchoCelda, altoCelda);
                        }
                    }

                    if (item is ItemBomba bomba)
                    {
                        if (bomba.PosicionEnMalla == null)
                        {
                            return; //Se agregó esto para evitar que diera u aviso de que podía ser null
                        }
                        if (bomba.Imagen != null)
                        {
                            g.DrawImage(bomba.Imagen, 
                                bomba.PosicionEnMalla.Y * anchoCelda, 
                                bomba.PosicionEnMalla.X * altoCelda, 
                                anchoCelda, altoCelda);
                        }
                    }
                }
            }

            if (malla.PoderesEnMalla != null)
            {
                foreach (var poder in malla.PoderesEnMalla)
                {
                    if (poder is HiperVelocidad velocidadAumentada)
                    {
                        if (velocidadAumentada.PosicionEnMalla == null)
                        {
                            return; //Se agregó esto para evitar que diera u aviso de que podía ser null
                        }
                        if (velocidadAumentada.Imagen != null)
                        {
                            g.DrawImage(velocidadAumentada.Imagen, 
                                velocidadAumentada.PosicionEnMalla.Y * anchoCelda, 
                                velocidadAumentada.PosicionEnMalla.X * altoCelda, 
                                anchoCelda, altoCelda);
                        }
                    }
                    if (poder is Invensibilidad Invensibilidad)
                    {
                        if (Invensibilidad.PosicionEnMalla == null)
                        {
                            return; //Se agregó esto para evitar que diera u aviso de que podía ser null
                        }
                        if (Invensibilidad.Imagen != null)
                        {
                            g.DrawImage(Invensibilidad.Imagen, 
                                Invensibilidad.PosicionEnMalla.Y * anchoCelda, 
                                Invensibilidad.PosicionEnMalla.X * altoCelda, 
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