using Controladores;  // Para acceder a GameController, InputHandler, etc.
using MallaGrid;  // Para acceder a Malla, Nodo.
using Modelos;  // Para Estela, Items, Moto, etc.

namespace TronGame
{
    public partial class Form1 : Form
    {
        private Malla malla;
        private Moto moto;
        private TeclasPresionadas teclasPresionadas;
        private MotoJugador motoJugador;
        public List<Bots> bots = new List<Bots>(); //Lista de bots
        private Random random = new Random(); //Utilizado para poder escoger una ubicación de spawm aleatoria de los bots-

        //Esto inicializa la entrada del Form cuando se le da dotnet run.
        private System.Windows.Forms.Timer clockTimer;

        public Form1()
        {
            InitializeComponent(); //Inicializar todos los componentes.

            //Configuración de doble buffering: Sirve para reducir el parpadeo a la hora de dibujar elementos en la pantalla.
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle; //Para bloquear la opción del mouse de poder "estirar" o "comprimir" la ventana del forms una vez iniciado, ya que si no buguearía la lista enlazada

            malla = new Malla(40,40); //Inicializamos la malla con nodos de 40x40
            malla.InicializadadorDeNodos(); //Para crear los nodos
            malla.ConectarNodos();//Para conectarlos todos, formando así una malla con cada nodo con referencias de
            //izquierda, derecha, abajo y arriba.

            InicializarBots(); // Inicializamos los bots para que se puedan dibujar


            //Timer para refrescar la llamada al método de mover las motos automáticamente cuando no se preciona nada:
            clockTimer = new System.Windows.Forms.Timer();
            clockTimer.Interval = 100; // Ajuste este valor según la velocidad deseada
            clockTimer.Tick += new EventHandler(ClockTimer_Tick);
            clockTimer.Start();
            
            motoJugador = new MotoJugador(malla.Nodos[20,20]); //Se crea un nuevo objeto de la clase moto y se le pasa como valor de "Posición inicial" [0,0] es decir arriba a la izquierda
            moto = motoJugador; // Si `moto` debe ser `motoJugador`
            teclasPresionadas = new TeclasPresionadas(motoJugador,this);

            this.KeyDown += new KeyEventHandler((sender,e) =>teclasPresionadas.MoverMoto(e));
            this.Paint += new PaintEventHandler(DibujarMalla);
        }
        
        //Método para llamar a las telasPresionadas pero cuando no se preciona nada:
        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            teclasPresionadas.MoverMoto(null);
            if (motoJugador.VerificarColisionConBots(bots))
            {
                clockTimer.Stop();
                return;
            }
        
            List<Bots> botsParaEliminar = new List<Bots>();
        
            foreach (var bot in bots)
            {
                bot.MoverAleatoriamenteBots(bots, motoJugador);
                if (!bot.estaEnMovimiento)
                {
                    botsParaEliminar.Add(bot);
                }
            }
        
            // Eliminar los bots que chocaron
            foreach (var bot in botsParaEliminar)
            {
                bots.Remove(bot);
            }
        
            this.Invalidate();
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

            if (moto != null)
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


            SolidBrush botBrush = new SolidBrush(Color.Green); // O cualquier otro color que prefieras para los bots
            foreach (var bot in bots)
            {
                if (bot != null)
                {
                        // Dibujar el bot
                    g.FillRectangle(botBrush, bot.PosicionActual.Y * anchoCelda, bot.PosicionActual.X * altoCelda, anchoCelda, altoCelda);

                    // Dibujar la estela del bot
                    SolidBrush estelaBotrush = new SolidBrush(Color.LightGreen); // O cualquier otro color para la estela de los bots
                    var NodoEstelaBotADibujar = bot.headEstela;
                    
                    while (NodoEstelaBotADibujar != null)
                    {
                        g.FillRectangle(estelaBotrush, NodoEstelaBotADibujar.Posicion.Y * anchoCelda, NodoEstelaBotADibujar.Posicion.X * altoCelda, anchoCelda, altoCelda);
                        NodoEstelaBotADibujar = NodoEstelaBotADibujar.Siguiente;
                    }
                }
            }
        }
        private void InicializarBots()
        {
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    Nodo posicionInicial = PosicionInicialAleatoriaBots();
                    if (posicionInicial != null)
                    {
                        Bots nuevoBot = new Bots(posicionInicial);
                        bots.Add(nuevoBot);
                    }
                    else
                    {
                        Console.WriteLine($"No se pudo crear el bot {i + 1}: posición inicial nula");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al crear el bot {i + 1}: {ex.Message}");
                }
            }
        }

        private Nodo PosicionInicialAleatoriaBots()
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