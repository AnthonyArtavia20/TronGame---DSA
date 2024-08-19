//Clase utilizada para la creación e inicialización de los nodos de la malla
namespace MallaGrid
{
    public class Nodo
        {
            public int X {get;set;} //Coordenadas "x" Y "y"
            public int Y {get;set;}
            public Nodo? Arriba {get;set;}
            public Nodo? Abajo {get;set;}
            public Nodo? Izquierda {get;set;}
            public Nodo? Derecha {get;set;}

            public bool EstaOcupado { get; set; }

            public Nodo(int x, int y) //Constructor(Inicializadador de los atributos)
            {
                X = x;
                Y = y;
                EstaOcupado = false;
            }
        
        }
}
