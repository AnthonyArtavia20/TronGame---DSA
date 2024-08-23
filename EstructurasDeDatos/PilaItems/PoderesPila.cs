
//Esta clase se encarga de implementar la estructura de datos Pila.
namespace EstructurasDeDatos
{
    public class PilaDePoderes
    {
        private NodosPilaDePoderes? _tope;//_tope para indicar que es privado

        public NodosPilaDePoderes? Tope()
        {
            return _tope;
        }

        public void Apilar(NodosPilaDePoderes poder)
        {
            if (_tope == null) //Preguntar si la lista enlazada tipo pila está vacía, = _tope == null.
            {
                _tope = poder; // Se hace una referencia a la posición de memoria del nodo que se está apilando.
            }
            else
            {
                /*Variable auxiliar que nos permita guardar el elemento que está actualmente en el tope
                para poder reemplazarlo por el nuevo y luego, al nuevo agregarle como siguiente, lo que estaba
                /nteriormente.*/
                NodosPilaDePoderes auxiliar = _tope;
                _tope = poder;
                _tope.siguiente = auxiliar;
            }
        }

        public  void Desapilar()//No le pasamos parámetro, ya que siempre se desapila por el mismo lugar,
        {// no nos importa lo que hay, solo desapilamos.
            /*Quita en elemento que está de primero en la pila*/

            if (_tope != null)
            {
                _tope = _tope.siguiente; //En tope va a quedar lo que hay en tope.siguiente.
                //Haciendo que la referencia al elemento actual, se elimine.
            }
        }
    }
}