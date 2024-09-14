using itemsDelJuego;

//Nodo de la cola
namespace EstructurasDeDatos
{
    public class NodoItemsCola
    {
        public itemsDelJuego.Items? ItemAlmacenado {get;set;}
        public NodoItemsCola? Siguiente {get;set;}
    }
}