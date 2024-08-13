//cONTIENE ATRIBUTOS: Posición, velocidad, combustible... y métodos para el movimiento}
using MallaGrid;


namespace Modelos
{
    public class Moto
    {

        public class NodoEstelaMoto
        {
            public Nodo? Posicion {get;set;}
            public NodoEstelaMoto? Siguiente {get;set;}
        }

        //ATRIBUTOS DE LA MOTO Y SU ESTELA:
        public Nodo PosicionActual {get; private set;}//Creamos un atributo para la posición inicial de la moto
        public NodoEstelaMoto? headEstela;
        private int longitudEstela;
        public int Velocidad {get;private set;}
        public int Combustible {get;private set;}
        private bool estaEnMovimiento;

        //Creamos el constructor de la clase para poder otorgarle el valor x,y donde va a aparecer la moto, es decir el valor incial que se le 
        //va a pasar a esta clase para que inicialice la ubicación inicial ahí:

        public Moto(Nodo posicionInicial,int longitudInicialEstela = 3)
        {
            PosicionActual = posicionInicial; //Posición actual de la moto "Donde aparece"
            longitudEstela = longitudInicialEstela;
            Velocidad = 2; //new Random().Next(1,6); //Velocidad entre 1 y 5
            Combustible = 10;// Tanque de combustible lleno
            estaEnMovimiento = true; // Inicialmente la moto está en movimiento
            InicializarEstaleMoto();
        }

        //SECTOR, MÉTODOS PARA LA ESTELA:
        
        private void InicializarEstaleMoto()
        {
            NodoEstelaMoto headEstela = new NodoEstelaMoto{Posicion = PosicionActual};
            var actual = headEstela;
            for (int i = 1; i < longitudEstela; i++)
            {
                actual.Siguiente = new NodoEstelaMoto {Posicion = PosicionActual};
                actual = actual.Siguiente;
            }
            this.headEstela = headEstela;
        }

        public void MoverEstelaMoto(Nodo nuevaPosicion)
        {
            //En resumidas cuenta la estela va creando nodos al frente constantemente y cuando la cantidad de nodos
            //es mayor a la longitud establecida de la estela, entonces se comienza a eliminar el último.
            if (!estaEnMovimiento) return; // Si la moto no está en movimiento, no hacer nada

            if (!DentroDeLimites(nuevaPosicion))
            {
                DetenerMoto("Has salido del campo de juego, perdiste"); // Detiene la moto y evita que siga moviéndose
                return;
            }

            if (VerificarColision(nuevaPosicion)) //Verifica si se chocó con algún nodo ocupado por una estela
            {
                DetenerMoto("Colisión detectada, perdiste"); // Detiene la moto y evita que siga moviéndose
                return;
            }

            var nuevoNodoDelFrente = new NodoEstelaMoto {Posicion = PosicionActual, Siguiente = headEstela};
            headEstela = nuevoNodoDelFrente;

            //Eliminamos el último nodo si la estela crece más de la cuenta de lo establecido como longitud.
            if (ContadorDeNodosEstela() > longitudEstela)
            {
                EliminarUtimoNodo();
            }

            PosicionActual = nuevaPosicion;//nuevaPosicion no es más que la posición a transladar indicadas por
            //las teclas del treclado, este argumento viene desde MoverArriba, MoverAbbajo, etc...

            //A medida que la estela crece, significa que la moto avanza, por lo tanto su combustible debe
            //disminuir. Actualmente será 5 unidades por cada celda de la malla.

            // Consumir combustible
            Combustible -= Velocidad / 5;
            if (Combustible < 0)
            {
                Combustible = 0;
            }

            if (Combustible == 0)
            {
                DetenerMoto("Te quedaste sin combustible, perdiste.");
                return;
            }
        }

        private int ContadorDeNodosEstela()
        {
            int cantidadDeNodosEnlaEstela = 0;
            var actual = headEstela;

            while (actual != null)
            {
                cantidadDeNodosEnlaEstela++;
                actual = actual.Siguiente;
            }
            return cantidadDeNodosEnlaEstela;
        } 

        private void EliminarUtimoNodo()
        {
            if (headEstela == null || headEstela.Siguiente == null) //Si resulta que la cabeza de la estala
            {//es null, significa que no hay último elemento por eliminar.
                headEstela = null;
                return;
            }

            var nodoActual = headEstela; //Declaramos una variable que utilizo para identificar el elemento
            //actual o inicial para luego moverme por la linkedList hasta el elemento a eliminar.

            while (nodoActual.Siguiente.Siguiente != null) 
            {
                /*Ejemplo de uso:
                [2] -> [4] -> [1] -> [3] Si quiero eliminar el último elemento basta con estar sobre [1] y
                decir ¿Es [1].Siguiente.Siguiente, null?, es decir un elemento más afuera de la cantidad, 
                Pues sí, entonces elimina el anterior a ese, solo quita un Siguiente y será [3].*/
                nodoActual = nodoActual.Siguiente;
            }
            nodoActual.Siguiente = null;
        }

        //El siguiente método analiza si un acelda está ocupada por la estala, esto sirve para detectar coliciones en un futuro y la prevensión
        //de bugs en la malla.
        public virtual bool VerificarColision(Nodo nuevaPosicion)
        {
            var actual = headEstela;
            while (actual != null) //Analiza cada nodo para ver si está opcupado o no.
            {
                if (actual.Posicion.Equals(nuevaPosicion))  {                           
                    return true;
                }
                actual = actual.Siguiente;
            }
            return false;
        }
        
        private bool DentroDeLimites(Nodo posicion)
        {
            return posicion.X >= 1 && posicion.X <= 38 && posicion.Y >= 1 && posicion.Y <= 38;
        }

        public void DetenerMoto(string mensaje)
        {
            estaEnMovimiento = false; // Detener la moto
            //MessageBox.Show(mensaje);
            //Environment.Exit(1);
        }

        /*Ahora creamos los diferentes métodos que comprobarán si se puede realizar el movimiento que se desea, esto se logra verificando las
        condiciones incialmente establecidas en la clase MallaGrid, donde se verifica si, por ejemplo, arriba del primer nodo hay algo, en ese caso
        como no hay nada ya que el primer nodo es [0,0] encima de él hay Null, es decir nada, en este caso no se hace la condición de:
        [i-1,j] que permite establecer la posición una fila atrás. Es bajo este mismo principio que se cumpleas las 4 condiciones.*/
        
        public void MoverArriba()
        {
            if (PosicionActual.Arriba != null) 
            {
                MoverEstelaMoto(PosicionActual.Arriba);//Como se mencionó anteriormente, esto utiliza el atributo de 
                //Arriba, ya que PosicionActual es un nodo de la clase Nodo que puede utilizar .Arriba, pudiendo actualizar 
                //de [i,j] a [i -1 ,j] y así con todas
            }
        }

        public void MoverAbajo()
        {
            if (PosicionActual.Abajo != null)
            {
                MoverEstelaMoto(PosicionActual.Abajo); //Pasa de [i,j] -> [i + 1,j]
            }
        }

        public void MoverDerecha()
        {
            if (PosicionActual.Derecha != null)
            {
                MoverEstelaMoto(PosicionActual.Derecha); //Pasa de [i,j] -> [i,j + 1]
            }
        }

        public void MoverIzquierda()
        {
            if (PosicionActual.Izquierda != null)
            {
                MoverEstelaMoto(PosicionActual.Izquierda); //Pasa de [i,j] -> [i, j - 1]
            }
        }
    }
}