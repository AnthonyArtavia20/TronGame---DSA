//Se encarga de manejar las entradas del usuario, como las teclas para mover la moto.
using Modelos;

namespace Controladores
{
    public class TeclasPresionadas
    {
        //Se le otorgó readonly a los atributos para que solo se puedan asignar en el momento de la declaración o dentro del constructor de la clase.
        //es decir no se puede reasignar después esto es porque la moto del jugador no debería ser reasignada luego, esto permite evitar errores difíciles de rastrear y generados sin querer.
        private readonly Moto moto;
        private readonly Form form;
        public Keys ultimaTeclaPresionada = Keys.D;//Por defecto inicializada en D, almacena la tecla presioanda para poder seguir desplazandose en esa dirección.

        public TeclasPresionadas(Moto moto, Form form)//Constructor
        {
            this.moto = moto;
            this.form = form;
        }

        public void MoverMoto(KeyEventArgs eventoARegistrar)//Método que recibe la tecla presionada de modo que
        {//se pueda alterar la dirección de desplazamiento.
            if (eventoARegistrar != null)
            {
                switch (eventoARegistrar.KeyCode)
            {
                case Keys.W:
                    moto.Movimiento.MoverArriba();
                    ultimaTeclaPresionada = Keys.W;
                    break;
                case Keys.S:
                    moto.Movimiento.MoverAbajo();
                    ultimaTeclaPresionada = Keys.S;
                    break;
                case Keys.D:
                    moto.Movimiento.MoverDerecha();
                    ultimaTeclaPresionada = Keys.D;
                    break;
                case Keys.A:
                    moto.Movimiento.MoverIzquierda();
                    ultimaTeclaPresionada = Keys.A;
                    break;
                case Keys.M: // Tecla para mover el tope de la pila al fondo
                    moto.poderesPila.MoverTopeAlFondo();
                    break;
                case Keys.P: // Tecla para aplicar el poder del tope
                    AplicarPoderDelTope();
                    break;
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
                moto.Movimiento.MoverArriba();
            }
            else if (ultimaTeclaPresionada == Keys.S)
            {
                moto.Movimiento.MoverAbajo();
            }
            else if (ultimaTeclaPresionada == Keys.A)
            {
                moto.Movimiento.MoverIzquierda();
            }
            else if (ultimaTeclaPresionada == Keys.D)
            {
                moto.Movimiento.MoverDerecha();
            }
        }
        private void AplicarPoderDelTope()
        {
            if (moto.poderesPila.Tope != null)
            {
                var poderAplicar = moto.poderesPila.Tope.PoderAlmacenado;
                moto.AplicarEfectoDelPoder(poderAplicar);
                moto.poderesPila.Desapilar();
            }
        }
    }
}