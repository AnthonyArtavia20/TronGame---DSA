//cONTIENE ATRIBUTOS: Posición, velocidad, combustible... y métodos para el movimiento}

using MallaGrid;

namespace Modelos
{
    public class Moto
    {
        //Creamos un atributo para la posición inicial de la moto
        public Nodo PosicionActual {get; private set;}

        //Creamos el constructor de la clase para poder otorgarle el valor x,y donde va a aparecer la moto, es decir el valor incial que se le 
        //va a pasar a esta clase para que inicialice la ubicación inicial ahí:

        public Moto(Nodo posicionInicial)
        {
            PosicionActual = posicionInicial;
        }

        /*Ahora creamos los diferentes métodos que comprobarán si se puede realizar el movimiento que se desea, esto se logra verificando las
        condiciones incialmente establecidas en la clase MallaGrid, donde se verifica si, por ejemplo, arriba del primer nodo hay algo, en ese caso
        como no hay nada ya que el primer nodo es [0,0] encima de él hay Null, es decir nada, en este caso no se hace la condición de:
        [i-1,j] que permite establecer la posición una fila atrás. Es bajo este mismo principio que se cumpleas las 4 condiciones.*/

        public void MoverArriba()
        {
            if (PosicionActual.Arriba != null) 
            {
                PosicionActual = PosicionActual.Arriba; //Como se mencionó anteriormente, esto utiliza el atributo de 
                //Arriba, ya que PosicionActual es un nodo de la clase Nodo que puede utilizar .Arriba, pudiendo actualizar 
                //de [i,j] a [i -1 ,j] y así con todas
            }
        }

        public void MoverAbajo()
        {
            if (PosicionActual.Abajo != null)
            {
                PosicionActual = PosicionActual.Abajo; //Pasa de [i,j] -> [i + 1,j]
            }
        }

        public void MoverDerecha()
        {
            if (PosicionActual.Derecha != null)
            {
                PosicionActual = PosicionActual.Derecha; //Pasa de [i,j] -> [i,j + 1]
            }
        }

        public void MoverIzquierda()
        {
            if (PosicionActual.Izquierda != null)
            {
                PosicionActual = PosicionActual.Izquierda; //Pasa de [i,j] -> [i, j - 1]
            }
        }
    }
}