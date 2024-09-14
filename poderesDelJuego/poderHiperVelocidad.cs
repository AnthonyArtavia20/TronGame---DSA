using MallaGrid;
using Modelos;
using Controladores;

namespace poderesDelJuego
{
    public class HiperVelocidad: Poderes
    {
        public int ValorDeVelocidad;
        private static Random random = new Random();

        // Atributo privado para el temporizador
        private System.Timers.Timer? timer;
        private System.Timers.Timer? colorTimer; // Temporizador para cambiar el color

        //Lista con los colores que se aplicarán en la moto que active el poder:
        private List<Color> coloresPowerUp = new List<Color> {Color.LightCoral,Color.Green,Color.Red, Color.Yellow, Color.Violet, Color.Cyan, Color.Magenta};
        private int colorIndex = 0;

        // Atributo para almacenar el color original
        private Color colorOriginal;

        public HiperVelocidad(Nodo posicion)
        {
            ValorDeVelocidad = random.Next(2, 3);
            PosicionEnMalla = posicion;

            //Cargamos la imagen usando el nombre del archivo:
            CargarImagenPoderes("HiperVelocidad.PNG");
        }

        // Método para activar el poder
        public void ActivarHiperVelocidad(Moto moto)
        {
            colorOriginal = moto.ColorEstela;

            moto.Velocidad *= 2; // O cualquier otra lógica para aumentar la velocidad
            int duracionHiperVelocidad = new Random().Next(3, 5) * 1000; // Duración en milisegundos

            if (moto is MotoJugador jugador)
            {
                // Iniciar el temporizador para cambiar el color
                colorTimer = new System.Timers.Timer(100); // Cambia el color cada 100 ms
                colorTimer.Elapsed += (sender, e) => EfetoVisualDeLaMoto(moto);
                colorTimer.Start();

                // Iniciar el temporizador para desactivar el poder
                timer = new System.Timers.Timer(duracionHiperVelocidad);
                timer.Elapsed += (sender, e) => DesactivarHiperVelocidad(moto);
                timer.AutoReset = false; // Para que se ejecute solo una vez
                timer.Start();
            }
            else if (moto is Bots bot)
            {   
                bot.IniciarEfectoVisualHiperVelocidad();
            }

            Task.Delay(duracionHiperVelocidad).ContinueWith(_ => DesactivarHiperVelocidad(moto));
        }
        
        // Método para desactivar el poder
        private void DesactivarHiperVelocidad(Moto moto)
        {
            moto.Velocidad /= 2; // Restablece la velocidad original
            
            if (moto is MotoJugador jugador)
            {
                colorTimer?.Stop(); // Detener el temporizador de color
                colorTimer?.Dispose();

                // Restablecer el color de la estela al original
                moto.ColorEstela = colorOriginal;

                timer?.Stop();
                timer?.Dispose();
            }
            else if (moto is Bots bot)
            {
                bot.DetenerEfectoVisualHiperVelocidad();
            }
        }

        public void EfetoVisualDeLaMoto(Moto moto)
        {
            // Cambiar el color de la estela de la moto
            moto.ColorEstela = coloresPowerUp[colorIndex];
            colorIndex = (colorIndex + 1) % coloresPowerUp.Count;
        }
    }
}