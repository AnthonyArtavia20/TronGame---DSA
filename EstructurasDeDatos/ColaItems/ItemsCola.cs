//Implementación de la estrucutra cola, para implementar items y que afecten a los bots y las motos.
namespace EstructurasDeDatos
{
    
    public class ItemsCola
    {
        private NodoItemsCola? _inicio;
        public NodoItemsCola? Inicio //Nos permite devolver el valor de _inicio como un apropiedad de "Solo lectura" y que además no sea 
        {//editable desde niguna otra parte.
            get 
            {
                return _inicio;
            }
        }

        public void EnColar(NodoItemsCola item) //Agregar al final, funciona como una lista enlazada
        {
            if (_inicio == null) 
            {
                _inicio = item;
            }
            else
            {
                //Variable auxiliar que permite apuntar al último nodo de la lista.
                NodoItemsCola aux = BuscarUltimo(_inicio);//Es un punturero de referemcia al último nodo, falta traer el nodo.l
                aux.Siguiente = item; //El siguiente al último es el nodo que queremos encolar.
            }
        }

        private NodoItemsCola BuscarUltimo(NodoItemsCola unNodo)
        {//Buscamos recursivamente el último nodo.
            //Este método es algo extra que implementé por experimentar, pero se puede usar en alguna implementación.
            if (unNodo.Siguiente == null)
            {
                return unNodo;
            }
            else
            {
                return BuscarUltimo(unNodo.Siguiente);
            }
        }

        public NodoItemsCola? Desencolar()
        {
            if (_inicio == null)
            {
                // La cola está vacía, no hay nada que desencolar
                return null;
            }
            _inicio = _inicio.Siguiente; //Sirve para eliminar un elemento de la cola, elimina su referencia igualandolo al siguiente.
            //Ejemplo: A -> B -> C ---> b -> C

            return _inicio;
        }

    }

}
