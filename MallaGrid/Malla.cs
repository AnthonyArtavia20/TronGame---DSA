using EstructurasDeDatos;
using itemsDelJuego;
using poderesDelJuego;

namespace MallaGrid
{
    public class Malla //Clase principal encarga de crear la matríz del campo de juego.
    {
        public Nodo[,] Nodos {get; set;} //Creamos una lista multidimencional es decir una matriz 
        public int Filas {get; set;}
        public int Columnas {get; set;}
        public List<Items> ItemsEnMalla { get; private set; }
        public List<Poderes> PoderesEnMalla { get; private set; }
        private static Random random = new Random();

        public Malla(int filas, int columnas) //Constructor(Inicializadador de los atributos)
        {
            Filas = filas;
            Columnas = columnas;
            Nodos = new Nodo[filas,columnas]; //Creamos un nuevo objeto de la matriz multidimencional y se le da las proporciones de las filas y columnas, es decir del tamaño de la ventana del forms
            ItemsEnMalla = new List<Items>();
            PoderesEnMalla = new List<Poderes>();
            
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

        public void GenerarObjetoAleatorio()
        {
            int totalObjetosEnMalla = ItemsEnMalla.Count + PoderesEnMalla.Count;
        
            if (totalObjetosEnMalla >= 30) // Limitar el número máximo total de ítems y poderes en la malla
            {
                return;
            }
        
            int x, y;
            do
            {
                x = random.Next(0, Filas);
                y = random.Next(0, Columnas);
            } while (Nodos[x,y].EstaOcupado); // Mientras los nodos estén desocupados.
        
            if (totalObjetosEnMalla % 2 == 0)
            {
                GenerarItem(x, y);
            }
            else
            {
                GenerarPoder(x, y);
            }
        }
        
        public void GenerarItem(int x, int y) //Se encarga de generar un item de forma random por medio de un
        { // switch case.
            Items nuevoItem;
            int tipoItem = random.Next(3);
        
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
        
            ItemsEnMalla.Add(nuevoItem);
            Nodos[x,y].EstaOcupado = true;
        }
        
        private void GenerarPoder(int x, int y) //Se encarga de crear objetos del tipo específico de manera 
        { // random.
            Poderes nuevoPoder;
            int tipoPoder = random.Next(2);
        
            switch (tipoPoder)
            {
                case 0:
                    nuevoPoder = new HiperVelocidad(Nodos[x,y]);
                    break;
                default:
                    nuevoPoder = new Invensibilidad(Nodos[x,y]);
                    break;
            }
        
            PoderesEnMalla.Add(nuevoPoder);
            Nodos[x,y].EstaOcupado = true;
        }

        // En la clase Malla (Malla.cs)
        public void DevolverItemsYPoderes(ItemsCola itemsCola, PilaDePoderes poderesPila)
        {
            // Devolver ítems
            while (itemsCola.Inicio != null)
            {
                var nodoItem = itemsCola.Desencolar();
                if (nodoItem != null && nodoItem.ItemAlamcenado != null)
                {
                    var item = nodoItem.ItemAlamcenado;
                    // Asignar una nueva posición aleatoria al ítem
                    int x, y;
                    do
                    {
                        x = random.Next(0, Filas);
                        y = random.Next(0, Columnas);
                    } while (Nodos[x,y].EstaOcupado);
        
                    item.PosicionEnMalla = Nodos[x,y];
                    Nodos[x,y].EstaOcupado = true;
                    ItemsEnMalla.Add(item);
                }
            }
        
            // Devolver poderes (este código parece estar bien, lo dejamos como está)
            while (poderesPila.Tope != null)
            {
                var poder = poderesPila.Desapilar();
                if (poder != null)
                {
                    // Asignar una nueva posición aleatoria al poder
                    int x, y;
                    do
                    {
                        x = random.Next(0, Filas);
                        y = random.Next(0, Columnas);
                    } while (Nodos[x,y].EstaOcupado);
        
                    poder.PosicionEnMalla = Nodos[x,y];
                    Nodos[x,y].EstaOcupado = true;
                    PoderesEnMalla.Add(poder);
                }
            }
        }
    }
}