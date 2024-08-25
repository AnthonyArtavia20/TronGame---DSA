namespace Modelos
{
    public class MovimientoMoto
    {
        private Moto moto;

        public MovimientoMoto(Moto moto)
        {
            this.moto = moto;
        }

        public void MoverArriba()
        {
            for (int i = 0; i < moto.Velocidad; i++)
            {
                //Como se mencionó anteriormente, esto utiliza el atributo de 
                //Arriba, ya que PosicionActual es un nodo de la clase Nodo que puede utilizar .Arriba, pudiendo actualizar 
                //de [i,j] a [i -1 ,j] y así con todas
                if (moto.PosicionActual.Arriba != null)
                {
                    moto.MoverEstelaMoto(moto.PosicionActual.Arriba);
                }
                else
                {
                    break;
                }
            }
        }

        public void MoverAbajo()
        {
            for (int i = 0; i < moto.Velocidad; i++)
            {
                 //Pasa de [i,j] -> [i + 1,j]
                if (moto.PosicionActual.Abajo != null)
                {
                    moto.MoverEstelaMoto(moto.PosicionActual.Abajo);
                }
                else
                {
                    break;
                }
            }
        }

        public void MoverDerecha()
        {
            for (int i = 0; i < moto.Velocidad; i++)
            {
                 //Pasa de [i,j] -> [i,j + 1]
                if (moto.PosicionActual.Derecha != null)
                {
                    moto.MoverEstelaMoto(moto.PosicionActual.Derecha);
                }
                else
                {
                    break;
                }
            }
        }

        public void MoverIzquierda()
        {
            for (int i = 0; i < moto.Velocidad; i++)
            {
                if (moto.PosicionActual.Izquierda != null)
                {
                    moto.MoverEstelaMoto(moto.PosicionActual.Izquierda);
                }
                else
                {
                    break;
                }
            }
        }
    }
}