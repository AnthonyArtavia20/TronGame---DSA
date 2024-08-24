using Controladores;
using MallaGrid;

namespace Modelos
{
    public class MotoJugador : Moto //Clase que hereda todas las características de la clase padre, Moto
    {//Se decidió hacer así para pdoer crear tanto los bots como la moto del jugador con las mismas caracteristicas de la moto, pero con 
    //ligeros cambios individuales en ellos.
        public MotoJugador(Nodo posicionInicial, Malla malla,int longitudInicialEstela = 3) : base(posicionInicial, malla,longitudInicialEstela) //Con :base, estamos llamando al constructor de la clase padre.
        {
            this.ColorEstela = Color.Blue;
        }

        public bool VerificarColisionConBots(List<Bots> bots) //Verificación de colisiones con bots.
        {
            foreach (var bot in bots) //Recibe la lista de bots en juego y la itera, en el momento que detecta
            {// una colisión con el jugador, entonces procede a deterner las motos y enseña un mensaje.
                if (bot.VerificarColision(PosicionActual) && !PoderInvensivilidadActivado)
                {
                    DetenerMoto();
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

        public bool VerificarDentroDeLimites()  //Este método se crea aquí, a pesar de que sea similar al de la clase padre, hace lo mism, pero
        { //permite ser llamado específicamente para l amoto del jugador.
            if (malla.NodosDeLosBordes(PosicionActual))
            {
                return true;
            }
            return false;
        }
    }
}