using Controladores;
using MallaGrid;
using itemsDelJuego;
using System.Diagnostics;

namespace Modelos
{
    public class MotoJugador : Moto //Clase que hereda todas las características de la clase padre, Moto
    {//Se decidió hacer así para pdoer crear tanto los bots como la moto del jugador con las mismas caracteristicas de la moto, pero con 
    //ligeros cambios individuales en ellos.
        public MotoJugador(Nodo posicionInicial, Malla malla,int longitudInicialEstela = 3) : base(posicionInicial, malla,longitudInicialEstela) //Con :base, estamos llamando al constructor de la clase padre.
        {
            //No se inicializan atributos, pues la clase padre ya lo hace desde que se crea una instancia de MotoJugador
        }

        public bool VerificarColisionConBots(List<Bots> bots) //Verificación de colisiones con bots.
        {
            foreach (var bot in bots) //Recibe la lista de bots en juego y la itera, en el momento que detecta
            {// una colisión con el jugador, entonces procede a deterner las motos y enseña un mensaje.
                if (bot.VerificarColision(PosicionActual))
                {
                    DetenerMoto();
                    bot.DetenerMoto(); //Medio innceserario pues una vez se da la colisón, el juego se para(Esto se hace desde Form1.cs).
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

        public override void AplicarEfectoDelItem(Items item)//Aquí se le hace override.
        {
            switch (item)
            {
                case ItemBomba bomba://Caso específico para el jugador.
                    DetenerMoto();
                    bomba.Explotar();
                    Environment.Exit(0);
                    break;

            }
        }
    }
}
