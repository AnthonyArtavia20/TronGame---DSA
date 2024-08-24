//Estructura de datos para el manejo de los poderes
//Esta se encarga de crear los nodos con un nombre más la referencia al siguiente.
using poderesDelJuego;

namespace EstructurasDeDatos
{
    public class NodosPilaDePoderes
    {
        public poderesDelJuego.Poderes? PoderAlmacenado {get;set;}
        public NodosPilaDePoderes? siguiente {get; set;}
    }
}
