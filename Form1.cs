using Controladores;  // Para acceder a GameController, InputHandler, etc.
using EstructurasDeDatos;  // Para las estructuras de datos.
using MallaGrid;  // Para acceder a Malla, Nodo.
using Modelos;  // Para Estela, Items, Moto, etc.

namespace TronGame
{
    public partial class Form1 : Form
    {
        private Malla malla;
        private Moto moto;
        private TeclasPresionadas teclasPresionadas;

        //Esto inicializa la entrada del Form cuando se le da dotnet run.
        private System.Windows.Forms.Timer clockTimer;

        public Form1()
        {
            InitializeComponent();
            malla = new Malla(40,40);
            malla.InicializadadorDeNodos();
            malla.ConectarNodos();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; //Para bloquear la opción del mouse de poder "estirar" o "comprimir" la ventana del forms una vez iniciado, ya que si no buguearía la lista enlazada

            //Timer para refrescar la llamada al método de mover las motos automáticamente cuando no se preciona nada:
            clockTimer = new System.Windows.Forms.Timer();
            clockTimer.Interval = 100; // Ajuste este valor según la velocidad deseada
            clockTimer.Tick += new EventHandler(ClockTimer_Tick);
            clockTimer.Start();
            
            moto = new Moto(malla.Nodos[1,1]); //Se crea un nuevo objeto de la clase moto y se le pasa como valor de "Posición inicial" [0,0] es decir arriba a la izquierda
            teclasPresionadas = new TeclasPresionadas(moto,this);

            this.KeyDown += new KeyEventHandler((sender,e) =>teclasPresionadas.MoverMoto(e));
            this.Paint += new PaintEventHandler(DibujarMalla);
        }

        //Método para llamar a las telasPresionadas pero cuando no se preciona nada:
        private void ClockTimer_Tick(object sender,EventArgs e)
        {
            teclasPresionadas.MoverMoto(null);
            this.Invalidate(); //Re-dibujar el forms para mostrar los cambios
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

            //Dibujar la moto
            SolidBrush motoBrush = new SolidBrush(Color.Red);
            //Dibujar la estela de la moto
            SolidBrush estelaBrush = new SolidBrush(Color.Blue);
            
            //Ciclo for para ir dibujando cada nodo de la estela.
            var NodoEstelaMotoADibujar = moto.headEstela; //Se tuvo que hacer pública la clase "NodoEstelaMoto" para poder hacer público
            //la cabeza de la linkedlist headEstela, ya que al estar private, no dejaba acceder a él.
            while (NodoEstelaMotoADibujar != null)
            {
                g.FillRectangle(estelaBrush, NodoEstelaMotoADibujar.Posicion.Y * anchoCelda, NodoEstelaMotoADibujar.Posicion.X * altoCelda, anchoCelda, altoCelda);
                NodoEstelaMotoADibujar = NodoEstelaMotoADibujar.Siguiente;
            }

            g.FillRectangle(motoBrush, moto.PosicionActual.Y * anchoCelda, moto.PosicionActual.X * altoCelda, anchoCelda, altoCelda);
        }
    }

}