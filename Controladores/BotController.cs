//Lógica de los bots controlador de forma aleatoria
using MallaGrid;
using Modelos;
using poderesDelJuego;

namespace Controladores
{
    public class Bots : Moto//heredamos de la clase Moto todas sus características
    {
        private int direccionActual; //Almacén del valor de dirección.
        public int VelocidadBots {get;private set;} //Atributo que se pasa desde el forms 1, es la velocidad alatoria de cada bot.
        private Color colorOriginal; //Color original de la moto en cuestión.
        public bool InvencibilidadBotActivada { get; private set; } = false; //Permite desactivar o activar métodos de comprobación de colisiones.
        private System.Timers.Timer? colorTimer; //Timer para poder iterar sobre los colores de las estelas.
        private List<Color> coloresPowerUp = new List<Color> {Color.LightCoral, Color.Green, Color.Red, Color.Yellow, Color.Violet, Color.Cyan, Color.Magenta};
        private int colorIndex = 0; //Se utiliza para poder recorrer los colores en una lista y posteriormente aplicarlos.
        private Color colorInvencibilidad = Color.Gold; // Color que indica invencibilidad
        private bool estaInvencible = true; //Esto se usa para comprobar la invencibilidad.
        private System.Timers.Timer? timerParpadeoInvencibilidad; //Timer para poder fijar un temporizador en la invensibilidad.

        public Bots(Nodo posicionInicial,Malla malla,Color ColorEstela,int velocidadBots,int longitudInicialEstela = 3) : base(posicionInicial,malla,longitudInicialEstela)
        {
            //Nota: Se pasa Malla malla porque se necesita para poder llamar al método de la malla capáz de determinar
            //los nodos de los bordes.
            direccionActual = random.Next(4); //Inicializamos la velocidad como unn random para idnicar que dirección será la inicial en los métodos de movimiento.
            this.ColorEstela = ColorEstela; //Inicializamos el color de le estela.
            this.VelocidadBots = velocidadBots; //Inicializamos la velocidad delos bots.
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
                return false; // Si algún poder está activiado, entonces no se comprueban colisiones.
            }
            else
            {
                return VerificarColision(nodo) || VerificarColisionConOtrosBot(nodo, listaDeBotsDesdeForm1);
            }
        }

        public override bool VerificarColision(Nodo nuevaPosicion)
        {
            if (InvencibilidadBotActivada)
            {
                return false; // Si es invencible, no hay colisión
            }
            return base.VerificarColision(nuevaPosicion);
        }
        private bool VerificarColisionConOtrosBot(Nodo nodo, List<Bots> listaDeBots)//Método utilizado para verificar la colisión con los bots
        {//se logra mediante la revisión en ciclo de la lista de bots, es decir, se itera constantemente con el fin de checkear si un bot cualquiera
        //de la lista, choca con la misma posición del actual.
        
            if (PoderInvensivilidadActivado || InvencibilidadBotActivada)
            {
                return false;//De igual forma acá se verifica si algún poder está activado, si lo está, no se compreban colisiones.
            }
            foreach (var otroBot in listaDeBots) //Si no está activado ninguno, se itera la lista delos bots y se comprueba los nodos.
            {
                if (otroBot != this && otroBot.VerificarColision(nodo))
                {
                    return true;
                }
            }
            return false;
        }

        public void UsarPoderAleatorio() //Métodp para permitirles a los bots usar poderes a base de probabilidad.
        {
            if (poderesPila.Tope != null && random.NextDouble() < 0.9)  //el 0.9, significa 90% de probabilidad de usar un poder.
            {
                var poder = poderesPila.Tope.PoderAlmacenado;
                AplicarEfectoDelPoder(poder);
                poderesPila.Desapilar();
            }
        }

        public override void AplicarEfectoDelPoder(Poderes poder) //Aquí se llaman a los diferentes métodos que activan otros métodos encargados de activar los efectos.
        {
            base.AplicarEfectoDelPoder(poder); //Se manda el poder al método original.

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
                IniciarEfectoVisualInvencibilidad();
                int duracionInvencibilidad = new Random().Next(3, 5) * 1000;
                Task.Delay(duracionInvencibilidad).ContinueWith(_ => 
                {
                    InvencibilidadBotActivada = false;
                    DetenerEfectoVisualInvencibilidad();
                });
            }
        }

        /*----------------------------------HIPERVELOCIDAD(Start)-----------------------------------------------*/
        public void IniciarEfectoVisualHiperVelocidad() //Encargado del efecto visual de la hipervelocidad
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

        /*----------------------------------HIPERVELOCIDAD(End)-----------------------------------------------*/

        /*----------------------------------INVENCIBILIDAD(Start)-----------------------------------------------*/
        private void IniciarEfectoVisualInvencibilidad()
        {
            colorOriginal = ColorEstela;
            timerParpadeoInvencibilidad = new System.Timers.Timer(200); // Parpadeo cada 200ms
            timerParpadeoInvencibilidad.Elapsed += (sender, e) => EfectoParpadeoInvencibilidad();
            timerParpadeoInvencibilidad.Start();
        }

        private void EfectoParpadeoInvencibilidad()
        {
            if (estaInvencible)
            {
                ColorEstela = colorInvencibilidad;
            }
            else
            {
                ColorEstela = colorOriginal;
            }
            estaInvencible = !estaInvencible;
        }

        private void DetenerEfectoVisualInvencibilidad()
        {
            timerParpadeoInvencibilidad?.Stop();
            timerParpadeoInvencibilidad?.Dispose();
            ColorEstela = colorOriginal;
        }
        /*----------------------------------INVENCIBILIDAD(End)-----------------------------------------------*/
    }
}