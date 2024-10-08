//cONTIENE ATRIBUTOS: Posición, velocidad, combustible... y métodos para el movimiento}
using MallaGrid;
using EstructurasDeDatos;
using poderesDelJuego;


namespace Modelos
{
    public class Moto //Clase principal, Moto
    {
        public class NodoEstelaMoto //Clase anidada para la estela de la moto
        {
            /*En resumidas cuentas la estela va creando nodos al frente constantemente y cuando la cantidad de nodos
                es mayor a la longitud establecida de la estela, entonces se comienza a eliminar el último.*/
            public Nodo? Posicion {get;set;}
            public NodoEstelaMoto? Siguiente {get;set;}
        }

        //ATRIBUTOS DE LA MOTO:
        public Nodo PosicionActual {get; private set;}//Creamos un atributo para la posición inicial de la moto
        public NodoEstelaMoto? headEstela;
        public int LongitudEstela { get; set; }
        public Color ColorEstela { get; set; }
        public int Velocidad {get; set;} //Se le qutó el private al set, esto con el fin de poder modificar la velocidad desde la clase
                                            //Hipervelocidad.
        public int Combustible {get; set;}
        public MovimientoMoto Movimiento { get; private set; } //Referencia a la clase donde se tiene la lógica del movimiento.
        public MotoColisionAplicarPoderesItems ColisionYEfectos { get; private set; }
        public bool estaEnMovimiento;
        protected Random random;
        public Malla malla; //Se crea una variable de tipo Malla(La clase) para luego hacer verificación de límites

        public ItemsCola itemsCola = new ItemsCola();
        public PilaDePoderes poderesPila = new PilaDePoderes();

        public bool PoderInvensivilidadActivado { get; set; }

        //Creamos el constructor de la clase para poder otorgarle el valor x,y donde va a aparecer la moto, es decir el valor incial que se le 
        //va a pasar a esta clase para que inicialice la ubicación inicial ahí:

        public Moto(Nodo posicionInicial,Malla malla,int longitudInicialEstela = 3) //Constructor para la moto
        {
            random = new Random();
            PosicionActual = posicionInicial; //Posición actual de la moto "Donde aparece"
            this.malla = malla; //Para poder comparar los nodos de los bordes
            LongitudEstela = longitudInicialEstela + 1;
            Velocidad = 1; //Velocidad setteable
            //Velocidad = random.Next(1,3); //Velocidad entre 1 y 3
            Combustible = 100;// Tanque de combustible lleno
            estaEnMovimiento = true; // Inicialmente la moto está en movimiento
            PoderInvensivilidadActivado = false; // Por defecto, no es invulnerable
            Movimiento = new MovimientoMoto(this);
            ColisionYEfectos = new MotoColisionAplicarPoderesItems(this);
            InicializarEstaleMoto();
        }

        //SECTOR, MÉTODOS PARA LA ESTELA:
        
        private void InicializarEstaleMoto() //Se encarga de inicializar la estela y comenzar a crear nodos hasta la longitud actual.
        {
            NodoEstelaMoto headEstela = new NodoEstelaMoto{Posicion = PosicionActual}; //Se crea un nuevo nodo para la cabeza de la moto.

            var actual = headEstela; //variable temporal para no afectar a la referencia de la cabeza
            for (int i = 1; i < LongitudEstela; i++) //Iteramos y en el proceso creamos nuevos nodos hasta la lonitud fijada.
            {
                actual.Siguiente = new NodoEstelaMoto {Posicion = PosicionActual}; //Hacemos la referencia de siguiente del nodo como actual.siguiente.
                actual = actual.Siguiente; //luego definimos actual como el nodo que se acaba de crear para así seguir agregando referencias de "siguiente".
            }
            this.headEstela = headEstela;//Enlazamos la referencia al nodo principal de la estela de la moto, enlazando así, su cabezza.
        }

        public void MoverEstelaMoto(Nodo nuevaPosicion)//Método encargado de mover la estela
        {
            if (nuevaPosicion == null || !estaEnMovimiento) return; // Este es como el freno de mano, cuando se detecte una colisión, entonces
            //estaEnMoviento se pondrá en False, provocando que ya no se puedan procesar más movimientos

            if (VerificarColision(nuevaPosicion)) //Verifica si se chocó con algún nodo ocupado por una estela
            {
                DetenerMoto(); // Detiene la moto y evita que siga moviéndose
                return;
            }

            PosicionActual = nuevaPosicion;//Se almacena en una variable temporal el nodo correspondiente como nuevo, y a su ves se le llama
            //PosicionActuial, esto con el fin de que se cree un nuevo nodo con la posición atual y así no perder la referencia  de la nuevaPosicion

            var nuevoNodoDelFrente = new NodoEstelaMoto {Posicion = PosicionActual, Siguiente = headEstela};//Como mencioné, se crea un nuevo nodo
            //con la posición actual y la referencia siguiente que simboliza la cabeza que siempre va al frente.
            headEstela = nuevoNodoDelFrente; //Posteriormente se estableze la cabeza como el nuevo nodo que se acaba de crear

            while (ContadorDeNodosEstela() >= LongitudEstela+1)//Esta condición sirve solamente para poder lograr mantener la estela de un tamaño
            {///determinado, ya que la estela se tiene que ir difuminando, no tiene una longitud infinita.
                EliminarUtimoNodo();
            }

            PosicionActual = nuevaPosicion;//nuevaPosicion no es más que la posición a transladar indicadas por
            //las teclas del treclado, este argumento viene desde MoverArriba, MoverAbbajo, etc...
            /*
            A medida que la estela crece, significa que la moto avanza, por lo tanto su combustible debe
            disminuir. Actualmente será 5 unidades por cada celda de la malla*/

            ColisionYEfectos.VerificarColisionConItems();
            ColisionYEfectos.ProcesarColaDeItems();

            ColisionYEfectos.VerificarColisionConPoderes();
            //ProcesarPilaDePoderes();

            // ---------Consumir combustible-------------
             Combustible = Math.Max(0, Math.Min(Combustible, 100)); // Asegurar que esté entre 0 y 100

            if (Combustible == 0)
            {
                DetenerMoto();
                return;
            }
            Combustible -= Math.Max(1, Velocidad / 5);
        }

        private int ContadorDeNodosEstela()//Método utilizado para poder contar la cantidad de nodos en la estela y con esto poder compararlos
        {//en la condición de arriba para así poder eliminar nodos, técnicamente por cada referencia a siguiente se agrega +1 al contador.
            int cantidadDeNodosEnlaEstela = 0; //Almacen con la cantidad de nodos.
            var actual = headEstela; //Actual simboliza una varaible temporal para poder interar sobre los nodos de la linkedlist Simple.

            while (actual != null) //Llegar a null significa llegar hasta donde no hayan más nodos, es decir, el final de la lista.
            {
                cantidadDeNodosEnlaEstela++;
                actual = actual.Siguiente;
            }
            return cantidadDeNodosEnlaEstela;
        } 

        private void EliminarUtimoNodo()
        {
            // Verificamos si la cabeza de la estela es null o si su siguiente también es null.
            if (headEstela == null) // Si no hay nodos, simplemente retornamos.
            {
                return;
            }
        
            if (headEstela.Siguiente == null) // Si solo hay un nodo, lo eliminamos.
            {
                headEstela = null;
                return;
            }

            var nodoActual = headEstela; //Declaramos una variable que utilizo para identificar el elemento
            //actual o inicial para luego moverme por la linkedList hasta el elemento a eliminar.
            while (nodoActual.Siguiente != null && nodoActual.Siguiente.Siguiente != null) 
            {
                /*Ejemplo de uso:
                [2] -> [4] -> [1] -> [3] Si quiero eliminar el último elemento basta con estar sobre [1] y
                decir ¿Es [1].Siguiente.Siguiente, null?, es decir un elemento más afuera de la cantidad, 
                Pues sí, entonces elimina el anterior a ese, solo quita un Siguiente y será [3].*/
                nodoActual = nodoActual.Siguiente;
            }
            nodoActual.Siguiente = null;
        }

        //verifica si una nueva posición en la malla del juego está ocupada por la estela de la moto. Esto es importante para detectar
        // colisiones y evitar errores (bugs) en el juego.
        public virtual bool VerificarColision(Nodo nuevaPosicion) //El método es virtual pues se necesita que pueda ser sobreescrito en una clase
        {//Derivada o heredada.
            if (nuevaPosicion == null) return true; //Verifiación sobre nulos

            var actual = headEstela; //Se crea una variable local con el fin de que inicialmente apunte al primer nodo de la estela("headEstela")
            while (actual != null)
            {
                if (!PoderInvensivilidadActivado && actual.Posicion != null && actual.Posicion.Equals(nuevaPosicion))// se usa el método Equals para comparar si la posición del nodo actual 
                {//de la estela coincide con nuevaPosicion, si se encuentra una coincidencia, el método retorna true, indicando que la nueva posición coincide
                //con una parte de la estela, significando que se ha detectado una colisión.
                    return true;
                }
                actual = actual.Siguiente;
            }

            return false;
        }

        public void DetenerMoto()//Creado para facilitar la escritura del movimeinto en false.
        {
            estaEnMovimiento = false; // Detener la moto
            // Devolver ítems y poderes a la malla
            malla.DevolverItemsYPoderes(itemsCola, poderesPila);
            // Limpiar la cola de ítems y la pila de poderes
            while (itemsCola.Inicio != null)
            {
                itemsCola.Desencolar();
            }
            while (poderesPila.Tope != null)
            {
                poderesPila.Desapilar();
            }
        }

        public virtual void AplicarEfectoDelPoder(Poderes poder) //Esto se usa para coenctar los métodos entre 
        {//BotController.cs y MetodosColisionAplicarEfectos.cs
            ColisionYEfectos.AplicarEfectoDelPoder(poder);
        }
    }
}