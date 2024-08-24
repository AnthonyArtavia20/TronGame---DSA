//Lógica de los bots controlador de forma aleatoria
using MallaGrid;
using Modelos;
using poderesDelJuego;

namespace Controladores
{
    public class Bots : Moto//heredamos de la clase Moto todas sus características
    {
        private Random random = new Random(); //Generador Random
        private int direccionActual; //Almacén del valor de dirección.
        public Color ColorEstela {get; private set;} //Color de estela independiente que va a tener cada bot.
        public int VelocidadBots {get;private set;} //Atributo que se pasa desde el forms 1, es la velocidad alatoria de cada bot.
        private Color colorOriginal;
        public bool InvencibilidadBotActivada { get; private set; } = false; //Permite desactivar o activar métodos de comprobación de colisiones.
        private System.Timers.Timer? colorTimer;
        private List<Color> coloresPowerUp = new List<Color> {Color.LightCoral, Color.Green, Color.Red, Color.Yellow, Color.Violet, Color.Cyan, Color.Magenta};
        private int colorIndex = 0;

        public Bots(Nodo posicionInicial,Malla malla,Color ColorEstela,int velocidadBots,int longitudInicialEstela = 3) : base(posicionInicial,malla,longitudInicialEstela)
        {
            //Nota: Se pasa Malla malla porque se necesita para poder llamar al método de la malla capáz de determinar
            //los nodos de los bordes.
            direccionActual = random.Next(4);
            this.ColorEstela = ColorEstela;
            this.VelocidadBots = velocidadBots;
        }

        public void MoverAleatoriamenteBots(List<Bots> listaDeBotsDesdeForm1, MotoJugador jugadorReal) //Implementamos un método especial que agrega todos los posibles
        {
            UsarPoderAleatorio();
            for (int i  = 0; i < VelocidadBots; i++)
            {
                //movimientos que puede tener el bot en la posición actual para que siempre tome una decisión en base
                //a los nodos que no estén ocupados, logrando que estos no choquen con otras estelas-

                /*Creamos 2 Arrays para almacenar las listas de:
                    -> Posibles Movimientos: La cual almacena todos los nodos a los cuales se ha comprobado que no están ocupados, entonces se puede desplazar.
                    -> Movimeintos malos al propio: Almacena todos los nodos con lo que podría tener colisión para posterior elegir algunas veces unos de estos
                    con el fin de fallar al propio algunas veces, permitiendo que actuen más natural, pareciendo jugadores reales equivocandose.
                */
                List<Nodo> posiblesMovimientos = new List<Nodo>();
                List<Nodo> moviminetosMalosAlPropio = new List<Nodo>();


                //Estos métodos comprueban si los nodos contiguos a la posición actual permmiten o no desplazarse, dentro de ellos se hacen
                //las comprobaciones anteriormente explicadas.
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

                Nodo? movimientoElegido = null; //Variable tipo Nodo que se utiliza para poder almacenar el movimiento random escogido.
    
                if (posiblesMovimientos.Count > 0) //Cuando la cantidad de movimientos almacenados sea mayor a 0
                {
                    movimientoElegido = posiblesMovimientos[random.Next(posiblesMovimientos.Count)];
                }
                else if (moviminetosMalosAlPropio.Count > 0 && random.NextDouble() < 1.0) //20% de probabilidad de escoger un movimiento que mate al bot(ej: chocar con una estela)
                {
                    movimientoElegido = moviminetosMalosAlPropio[random.Next(moviminetosMalosAlPropio.Count)];
                }
    
                if (movimientoElegido != null) //Luego de haber seleccionado el movimiento a realizar, aquí se comprueba que no sea nulo,
                {//esto con el fin de luego verificar si hay colisión con el nodo escogidp, si no la hay, entonces se mueve hacía el nodo escogido.
                    if (!InvencibilidadBotActivada && !jugadorReal.PoderInvensivilidadActivado && jugadorReal.VerificarColision(movimientoElegido))
                    {
                        DetenerMoto();
                    }
                    else
                    {
                        
                        MoverEstelaMoto(movimientoElegido);
                    }
                }
            }
        }

        private bool EsNodoOcupadoPorBots(Nodo nodo, List<Bots> listaDeBotsDesdeForm1) //Utilizado para verificar si un nodo está ocupado por
        {//el jugador o un bot.
            if (PoderInvensivilidadActivado || InvencibilidadBotActivada)
            {
                return false;
            }
            else
            {
                return VerificarColision(nodo) || VerificarColisionConOtrosBot(nodo, listaDeBotsDesdeForm1);
            }
        }

        private bool VerificarColisionConOtrosBot(Nodo nodo, List<Bots> listaDeBots)//Método utilizado para verificar la colisión con los bots
        {//se logra mediante la revisión en ciclo de la lista de bots, es decir, se itera constantemente con el fin de checkear si un bot cualquiera
        //de la lista, choca con la misma posición del actual.
        
            if (PoderInvensivilidadActivado || InvencibilidadBotActivada)
            {
                return false;
            }
            foreach (var otroBot in listaDeBots)
            {
                if (otroBot != this && otroBot.VerificarColision(nodo))
                {
                    return true;
                }
            }
            return false;
        }

        public void UsarPoderAleatorio()
        {
            if (poderesPila.Tope != null && random.NextDouble() < 0.9)  //el 0.5, significa 50% de probabilidad de usar un poder.
            {
                var poder = poderesPila.Tope.PoderAlmacenado;
                AplicarEfectoDelPoder(poder);
                poderesPila.Desapilar();
            }
        }

        public override void AplicarEfectoDelPoder(Poderes poder)
        {
            base.AplicarEfectoDelPoder(poder);

            if (poder is HiperVelocidad hiperVelocidad)
            {
                IniciarEfectoVisualHiperVelocidad();

                // Programar la desactivación del efecto visual
                int duracionHiperVelocidad = new Random().Next(3, 5) * 1000;
                Task.Delay(duracionHiperVelocidad).ContinueWith(_ => DetenerEfectoVisualHiperVelocidad());
            }

            if (poder is Invensibilidad invensibilidad)
            {
                InvencibilidadBotActivada = true;
                int duracionInvencibilidad = new Random().Next(3, 5) * 1000;
                Task.Delay(duracionInvencibilidad).ContinueWith(_ => { InvencibilidadBotActivada = false; });
            }
        }

        public void IniciarEfectoVisualHiperVelocidad()
        {
            colorOriginal = ColorEstela;
            colorTimer = new System.Timers.Timer(100); // Cambia el color cada 100 ms
            colorTimer.Elapsed += (sender, e) => CambiarColorEstela();
            colorTimer.Start();
        }
        private void CambiarColorEstela()
        {
            ColorEstela = coloresPowerUp[colorIndex];
            colorIndex = (colorIndex + 1) % coloresPowerUp.Count;
        }

        public void DetenerEfectoVisualHiperVelocidad()
        {
            colorTimer?.Stop();
            colorTimer?.Dispose();
            ColorEstela = colorOriginal;
        }

        public override bool VerificarColision(Nodo nuevaPosicion)
        {
            if (InvencibilidadBotActivada)
            {
                return false; // Si es invencible, no hay colisión
            }
            return base.VerificarColision(nuevaPosicion);
        }
    }
}