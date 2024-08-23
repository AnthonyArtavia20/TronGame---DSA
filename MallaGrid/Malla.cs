using itemsDelJuego;

namespace MallaGrid
{
    public class Malla //Clase principal encarga de crear la matríz del campo de juego.
    {
        public Nodo[,] Nodos {get; set;} //Creamos una lista multidimencional es decir una matriz 
        public int Filas {get; set;}
        public int Columnas {get; set;}
        public List<Items> ItemsEnMalla { get; private set; }
        private static Random random = new Random();

        private bool posicionLibre; //Esto se implementó como una forma de checkear si un espacio de los nodos está 
        //o no ocupado antes de generar un objeto en el campo de juego, ya que existía el caso en el que
        // sin desearlo, cuando se ejecutaba el juego, este se cerraba por una colisión con una bomba que enrealidad
        //nunca estuvo ahí, se generaba en un lugar donde ya había otro

    
    
        public Malla(int filas, int columnas) //Constructor(Inicializadador de los atributos)
        {
            Filas = filas;
            Columnas = columnas;
            Nodos = new Nodo[filas,columnas]; //Creamos un nuevo objeto de la matriz multidimencional y se le da las proporciones de las filas y columnas, es decir del tamaño de la ventana del forms
            ItemsEnMalla = new List<Items>();
            
        }
    
        public void InicializadadorDeNodos()//Tanto para este método como el de conectar su tope va a ser ya sea Filas y Columnas, cuando llegan al valor de las proporciones de la malla, entonces paran los ciclos.
        {
            for (int i = 0; i < Filas; i++)
            {
                for (int j = 0; j < Columnas; j++)
                {
                    Nodos[i,j] = new Nodo(i,j); /* En cada coordenada dada por los índices de los ciclos vamos a crear un 
                    nodo, es decir en cada celda de la matriz multidimencional agregamos un nodo que posteriormente 
                    conectaremos a los que tenga a los alrededores */
                }
            }
        }
    
        public void ConectarNodos()
        {
            for (int i = 0; i < Filas; i++)
            {
                for (int j = 0; j < Columnas; j++)
                {
                    if (i > 0) 
                    {
                        Nodos[i,j].Arriba = Nodos[i - 1,j]; //Esto es para cuando se analice desde la segunda fila 
                        //en adelante, que siempre tendrá un elemento por encima del que está analizando.
                    }
                    if (i < Filas - 1)
                    {
                        Nodos[i,j].Abajo = Nodos[i + 1,j]; //Siempre y cuando no hayamos llegado al tope de filas, 
                        //significa que tiene elementos debajo del que se está analizando, simplemente aumentamos 
                        //en 1 las filas y conservamos el valor de la columna
                    }
                    if (j > 0) 
                    {
                        Nodos[i,j].Izquierda = Nodos[i, j - 1]; //Mientras j no llegue al tope de las columnas, 
                        //siempre tendrá a alguien a su izquierda.
                    }
                    if (j < Columnas - 1) 
                    {
                        Nodos[i,j].Derecha = Nodos[i,j+1]; //Mientras que no lleguemos al tope de columnas, nos 
                        //encontraremos a alguien siempre a la derecha.
                    }
                    
                }
            }
        }

        public bool NodosDeLosBordes(Nodo nodo)
        {
            /*Método encargado de hacer una verificación de los bordes de la lista enlazada.
            Esto se utiiza para poder definir los límites del campo de juego.
            Nota: Se decidió hacer esto así ya que verificando con cordenadas y cálculos ya que resultaba muy difícil
            detectar errores generados por esto*/
            return nodo.X == 0 || nodo.X == Filas-1 || nodo.Y == 0 ||nodo.Y == Columnas -1;
        }

        public void GenerarItemAleatorio() //Se simplificó este método  con el fin de mejorar el rendimiento
        {//anteriormente se hacía un bucle for por cada item a dibujar y esto generaba muchísimo lag.

            if (ItemsEnMalla.Count >= 30)
            {
                // Limitar el número máximo de ítems en la malla
                return; //Ahora se controla la cantidad de elementos desde aquí.
            } 

            int x, y; //Creadas para poder otorgar un lugar en la malla.
            do
            {
                x = random.Next(0, Filas);
                y = random.Next(0, Columnas);
            } while (Nodos[x,y].EstaOcupado); //Mientras los nodos 

            Items nuevoItem; //Variable Tipo Items para almacenar los items
            int tipoItem = random.Next(3); //Variable de tipo random

            switch (tipoItem)
            {
                case 0:
                    nuevoItem = new ItemAumentarEstela(Nodos[x,y]);
                    break;
                case 1:
                    nuevoItem = new ItemCombustible(Nodos[x,y]);
                    break;
                default:
                    nuevoItem = new ItemBomba(Nodos[x,y]);
                    break;
            }

            ItemsEnMalla.Add(nuevoItem); //Agregamos el item a la malla.
            Nodos[x,y].EstaOcupado = true; //Ponemos ese nodo como ocupado.
        }
    }
}