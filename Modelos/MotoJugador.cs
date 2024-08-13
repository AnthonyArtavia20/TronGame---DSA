using MallaGrid;

namespace Modelos
{
    public class MotoJugador : Moto
    {
        public MotoJugador(Nodo posicionInicial, int longitudInicialEstela = 3) : base(posicionInicial, longitudInicialEstela)
        {
        }

        public override bool VerificarColision(Nodo nodo)
        {
            if (base.VerificarColision(nodo))
            {
                DetenerMoto("Colisión detectada, perdiste");
                return true;
            }

            if (Combustible == 0)
            {
                DetenerMoto("Te quedaste sin combustible, perdiste");
                return true;
            }
            return false;
        }
    }
}
