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

            public bool EstaOcupado { get; set; }/*Esto se implementó como una forma de checkear si un espacio de los nodos está 
            o no ocupado antes de generar un objeto en el campo de juego, ya que existía el caso en el que
            sin desearlo, cuando se ejecutaba el juego, este se cerraba por una colisión con una bomba que enrealidad
            nunca estuvo ahí, se generaba en un lugar donde ya había otro*/

            public Nodo(int x, int y) //Constructor(Inicializadador de los atributos)
            {
                X = x;
                Y = y;
                EstaOcupado = false; //Cada que se crea un Nodo en [X,Y] coordenadas, ese punto se marca en false, para
                //no volver a genrerar nada ahí.
            }
        
        }
}
