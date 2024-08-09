using System;
using System.Windows.Forms;
using Controladores;  // Para acceder a GameController, InputHandler, etc.
using EstructurasDeDatos;  // Para las estructuras de datos.
using MallaGrid;  // Para acceder a Malla, Nodo.
using Modelos;  // Para Estela, Items, Moto, etc.

namespace TronGame
{
    public partial class Form1 : Form
    {
        private Malla malla;
        //Esto inicializa la entrada del Form cuando se le da dotnet run.
        public Form1()
        {
            InitializeComponent();
            malla = new Malla(40,40);
            malla.InicializadadorDeNodos();
            malla.ConectarNodos();
            this.FormBorderStyle = FormBorderStyle.FixedSingle; //Para bloquear la opción del mouse de poder "estirar" o "comprimir" la ventana del forms una vez iniciado, ya que si no buguearía la lista enlazada

            this.Paint += new PaintEventHandler(DibujarMalla);
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
            int anchoCelda = this.ClientSize.Width / malla.Columnas;
            int altoCelda = this.ClientSize.Height / malla.Filas;
            

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
        }
    }

}