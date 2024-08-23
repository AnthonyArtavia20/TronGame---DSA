//Estructura de datos para el manejo de los poderes

//Esta se encarga de crear los nodos con un nombre más la referencia al siguiente.
namespace EstructurasDeDatos
{
    public class NodosPilaDePoderes
    {
        public string? nombre {get; set;}
        public NodosPilaDePoderes? siguiente {get; set;}
    }
}
