using Controladores;
using MallaGrid;

namespace Modelos
{
    public class MotoJugador : Moto
    {
        public MotoJugador(Nodo posicionInicial, int longitudInicialEstela = 20) : base(posicionInicial, longitudInicialEstela)
        {
        }

        public bool VerificarColisionConBots(List<Bots> bots) //Verifiación de colisiones con bots
        {
            foreach (var bot in bots) //Recibe la lista de bots en juego y la itera, en el momento que detecta
            {// una colisión con el jugador, entonces procede a deterner las motos y enseña un mensaje.
                if (bot.VerificarColision(PosicionActual))
                {
                    DetenerMoto();
                    bot.DetenerMoto(); //Medio innceserario pues uan vez se da la colisón, el juego se para.
                    return true;
                }
            }
            return false;
        }

        public bool VerificarCombustible() //Método para poder cerrar el jeugo si el Combustible es 0
        {
            if (Combustible == 0)
            {
                DetenerMoto();
                return true; // Indica que el combustible se agotó
            }
            return false;
        }

    }
}
