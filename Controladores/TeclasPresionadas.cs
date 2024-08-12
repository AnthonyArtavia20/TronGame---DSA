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
        public Keys ultimaTeclaPresionada = Keys.D;//Por defecto inicializada en D

        public TeclasPresionadas(Moto moto, Form form)
        {
            this.moto = moto;
            this.form = form;
        }

        public void MoverMoto(KeyEventArgs eventoARegistrar)
        {
            if (eventoARegistrar != null)
            {
                if (eventoARegistrar.KeyCode == Keys.W) //Entonces si se registra un evento de tipo tecla precionada y se categoriza como tecla "W" entonces significa que es subir
                {                                       //por lo tanto llamamos al método MoverArriba de la clase Moto.
                    moto.MoverArriba();
                    ultimaTeclaPresionada = Keys.W;
                } else if (eventoARegistrar.KeyCode == Keys.S)
                {
                    moto.MoverAbajo();
                    ultimaTeclaPresionada = Keys.S;
                } else if (eventoARegistrar.KeyCode == Keys.D) 
                {
                    moto.MoverDerecha();
                    ultimaTeclaPresionada = Keys.D;
                } else if (eventoARegistrar.KeyCode == Keys.A)
                {
                    moto.MoverIzquierda();
                    ultimaTeclaPresionada = Keys.A;
                }
                
            }else 
                {
                    //Si no se especifica una tecla a precionar, entonces se llama a la variable que almacena la última
                    //tecla precionada.
                    MantenerEnLaUltimaDireccion();
                }
                form.Invalidate(); //Redibujado forzado del forms para mostrar cambios.
        }

        private void MantenerEnLaUltimaDireccion() //Se llamará al movimiento correspondiente según la última
        {//tecla precionada, esto se logra almacenando la tecla en una variable que se compara, haciendo que
            if (ultimaTeclaPresionada == Keys.W)//se comparé aquí y se actualice la coordenadas de la moto.
            {
                moto.MoverArriba();
            }
            else if (ultimaTeclaPresionada == Keys.S)
            {
                moto.MoverAbajo();
            }
            else if (ultimaTeclaPresionada == Keys.A)
            {
                moto.MoverIzquierda();
            }
            else if (ultimaTeclaPresionada == Keys.D)
            {
                moto.MoverDerecha();
            }
        }
    }
}