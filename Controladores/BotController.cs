//Lógica de los bots controlador de forma aleatoria
using MallaGrid;
using Modelos;

namespace Controladores
{
    public class Bots : Moto
    {
        private Random random = new Random();
        private int direccionActual;

        public Bots(Nodo posicionInicial,int longitudInicialEstela = 3) : base(posicionInicial,longitudInicialEstela)
        {
            direccionActual = random.Next(4);
        }

        public void MoverAleatoriamenteBots(List<Bots> listaDeBotsDesdeForm1, MotoJugador jugadorReal) //Implementamos un método especial que agrega todos los posibles
        {//movimientos que puede tener el bot en la posición actual para que siempre tome una decisión en base
        //a los nodos que no estén ocupados, logrando que estos no choquen con otras esteas
            List<Nodo> posiblesMovimientos = new List<Nodo>();
            List<Nodo> moviminetosMalosAlPropio = new List<Nodo>();

            if (PosicionActual.Arriba != null)
            {
                if (!EsNodoOcupadoPorBots(PosicionActual.Arriba, listaDeBotsDesdeForm1))
                {
                    if (jugadorReal.VerificarColision(PosicionActual.Arriba))
                        moviminetosMalosAlPropio.Add(PosicionActual.Arriba);
                    else
                        posiblesMovimientos.Add(PosicionActual.Arriba);
                }
            }

            if (PosicionActual.Abajo != null)
            {
                if (!EsNodoOcupadoPorBots(PosicionActual.Abajo, listaDeBotsDesdeForm1))
                {
                    if (jugadorReal.VerificarColision(PosicionActual.Abajo))
                        moviminetosMalosAlPropio.Add(PosicionActual.Abajo);
                    else
                        posiblesMovimientos.Add(PosicionActual.Abajo);
                }
            }

            if (PosicionActual.Derecha != null)
            {
                if (!EsNodoOcupadoPorBots(PosicionActual.Derecha, listaDeBotsDesdeForm1))
                {
                    if (jugadorReal.VerificarColision(PosicionActual.Derecha))
                        moviminetosMalosAlPropio.Add(PosicionActual.Derecha);
                    else
                        posiblesMovimientos.Add(PosicionActual.Derecha);
                }
            }

            if (PosicionActual.Izquierda != null)
            {
                if (!EsNodoOcupadoPorBots(PosicionActual.Izquierda, listaDeBotsDesdeForm1))
                {
                    if (jugadorReal.VerificarColision(PosicionActual.Izquierda))
                        moviminetosMalosAlPropio.Add(PosicionActual.Izquierda);
                    else
                        posiblesMovimientos.Add(PosicionActual.Izquierda);
                }
            }

            Nodo movimientoElegido = null;

            if (posiblesMovimientos.Count > 0)
            {
                movimientoElegido = posiblesMovimientos[random.Next(posiblesMovimientos.Count)];
            }
            else if (moviminetosMalosAlPropio.Count > 0 && random.NextDouble() < 1.0) //20% de probabilidad de escoger un movimiento que mate al bot(ej: chocar con una estela)
            {
                movimientoElegido = moviminetosMalosAlPropio[random.Next(moviminetosMalosAlPropio.Count)];
            }

            if (movimientoElegido != null)
            {
                if (jugadorReal.VerificarColision(movimientoElegido))
                {
                    DetenerMoto();
                }
                else
                {
                    MoverEstelaMoto(movimientoElegido);
                }
            }
        }

        private bool EsNodoOcupadoPorBots(Nodo nodo, List<Bots> listaDeBotsDesdeForm1)
        {
            return VerificarColision(nodo) || VerificarColisionConOtrosBot(nodo, listaDeBotsDesdeForm1);
        }

        private bool VerificarColisionConOtrosBot(Nodo nodo, List<Bots> listaDeBots)
        {
            foreach (var otroBot in listaDeBots)
            {
                if (otroBot != this && otroBot.VerificarColision(nodo))
                {
                    return true;
                }
            }
            return false;
        }
    }
}