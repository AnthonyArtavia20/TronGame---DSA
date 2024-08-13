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

        public void MoverAleatoriamenteBots(List<Bots> listaDeBotsDesdeForm1) //Implementamos un método especial que agrega todos los posibles
        {//movimientos que puede tener el bot en la posición actual para que siempre tome una decisión en base
        //a los nodos que no estén ocupados, logrando que estos no choquen con otras esteas
            List<Nodo> posiblesMovimientos = new List<Nodo>();

            if (PosicionActual.Arriba != null && !EsNodoOcupado(PosicionActual.Arriba,listaDeBotsDesdeForm1))
                posiblesMovimientos.Add(PosicionActual.Arriba);

            if (PosicionActual.Abajo != null && !EsNodoOcupado(PosicionActual.Abajo,listaDeBotsDesdeForm1))
                posiblesMovimientos.Add(PosicionActual.Abajo);

            if (PosicionActual.Derecha != null && !EsNodoOcupado(PosicionActual.Derecha,listaDeBotsDesdeForm1))
                posiblesMovimientos.Add(PosicionActual.Derecha);

            if (PosicionActual.Izquierda != null && !EsNodoOcupado(PosicionActual.Izquierda,listaDeBotsDesdeForm1))
                posiblesMovimientos.Add(PosicionActual.Izquierda);

            // Aquí puedes aplicar una lógica para escoger el mejor movimiento
            // Por ejemplo, puedes elegir aleatoriamente una dirección válida
            if (posiblesMovimientos.Count > 0)
            {
                Nodo movimientoElegido = posiblesMovimientos[random.Next(posiblesMovimientos.Count)];
                MoverEstelaMoto(movimientoElegido);
            }
        }

        private bool EsNodoOcupado(Nodo nodo, List<Bots> listaDeBotsDesdeForm1)
        {
            // Comprobar si el nodo está ocupado por la estela del bot
            if (VerificarColision(nodo))
                return true;

            return VerificarColisionConOtrosBot(nodo, listaDeBotsDesdeForm1);
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