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
                MessageBox.Show("Colisión detectada, perdiste");
                estaEnMovimiento = false;
                Environment.Exit(0);
                return true;
            }

            if (Combustible == 0)
            {
                MessageBox.Show("Te quedaste sin combustible, perdiste");
                estaEnMovimiento = false;
                Environment.Exit(0);
                return true;
            }
            return false;
        }
    }
}
