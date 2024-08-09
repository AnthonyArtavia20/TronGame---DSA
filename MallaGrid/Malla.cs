namespace MallaGrid
{
    public class Malla
    {
        public Nodo[,] Nodos {get; set;} //Creamos una lista multidimencional es decir una matriz 
        public int Filas {get; set;}
        public int Columnas {get; set;}
    
    
        public Malla(int filas, int columnas) //Constructor(Inicializadador de los atributos)
        {
            Filas = filas;
            Columnas = columnas;
            Nodos = new Nodo[filas,columnas]; //Creamos un nuevo objeto de la matriz multidimencional y se le da las proporciones de las filas y columnas, es decir del tamaño de la ventana del forms
    
            
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
                        Nodos[i,j].Arriba = Nodos[i - 1,j]; //Esto es para cuando se analicé desde la segunda fila en adelante, que siempre tendrá un elemento por encima del que está analizando.
                    }
                    if (i < Filas - 1)
                    {
                        Nodos[i,j].Abajo = Nodos[i + 1,j]; //Siempre y cuando no hayamos llegado al tope de filas, significa que tiene elementos debajo del que se está analizando, simplemente aumentamos en 1 las filas y conservamos el valor de la columna
                    }
                    if (j > 0) 
                    {
                        Nodos[i,j].Izquierda = Nodos[i, j - 1]; //Mientras j no llegue al tope de las columnas, siempre tendrá a alguien a su izquierda.
                    }
                    if (j < Columnas - 1) Nodos[i,j].Derecha = Nodos[i,j+1]; //Mientras que no lleguemos al tope de columnas, nos encontraremos a alguien siempre a la derecha.
                }
            }
        }
    }
}