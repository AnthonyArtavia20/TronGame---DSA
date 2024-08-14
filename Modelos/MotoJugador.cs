using Controladores;
using MallaGrid;

namespace Modelos
{
    public class MotoJugador : Moto
    {
        public MotoJugador(Nodo posicionInicial, int longitudInicialEstela = 20) : base(posicionInicial, longitudInicialEstela)
        {
        }

        public bool VerificarColisionConBots(List<Bots> bots)
        {
            foreach (var bot in bots)
            {
                if (bot.VerificarColision(PosicionActual))
                {
                    MessageBox.Show("Colisión con un bot");
                    DetenerMoto();
                    Environment.Exit(0);
                    return true;
                }
            }
            return false;
        }

    }
}
