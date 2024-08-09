//Se encarga de manejar las entradas del usuario, como las teclas para mover la moto.
using Modelos;

namespace Controladores
{
    public class TeclasPresionadas
    {
        //Se le otorgó readonly a los atributos para que solo sepuedan asignar en el momento de la declaración o dentro del constructor de la clase.
        //es decir no se puede reasignar despues esto es porque la moto del jugador no debería ser reasignada luego, esto permite evitar errores difíciles de rastrear y generados sin querer.
        private readonly Moto moto;
        private readonly Form form;

        public TeclasPresionadas(Moto moto, Form form)
        {
            this.moto = moto;
            this.form = form;
        }

        public void MoverMoto(KeyEventArgs eventoARegistrar)
        {
            if (eventoARegistrar.KeyCode == Keys.W) //Entonces si se registra un evento de tipo tecla precionada y se categoriza como tecla "W" entonces significa que es subir
            {                                       //por lo tanto llamamos al método MoverArriba de la clase Moto.
                moto.MoverArriba();
            } else if (eventoARegistrar.KeyCode == Keys.S)
            {
                moto.MoverAbajo();
            } else if (eventoARegistrar.KeyCode == Keys.D) 
            {
                moto.MoverDerecha();
            } else if (eventoARegistrar.KeyCode == Keys.A)
            {
                moto.MoverIzquierda();
            }

            form.Invalidate(); //Fuerza un redibujado del forms para poder reflejar el cambio en la LinkedList
        }
    }
}