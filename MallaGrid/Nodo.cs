//Clase utilizada para la ceración e inicialización de los nodos de la malla

public class Nodo
{
    public int X {get;set;}
    public int Y {get;set;}
    public Nodo Arriba {get;set;}
    public Nodo Abajo {get;set;}
    public Nodo Izquierda {get;set;}

    public Nodo Derecha {get;set;}
    
    public Nodo(int x, int y) //Constructor(Inicializadador de los atributos)
    {
        X = x;
        Y = y;
    }

}