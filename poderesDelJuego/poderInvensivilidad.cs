using Modelos;
using MallaGrid;

namespace poderesDelJuego
{
    public class Invensibilidad : Poderes
    {
        private System.Timers.Timer? timerPoder;
        private System.Timers.Timer? timerParpadeo;
        private Color colorInvencibilidad = Color.Gold; // Color que indica invencibilidad
        private bool estaVisible = true;
        private Color colorOriginal;

        public Invensibilidad(Nodo posicion)
        {
            PosicionEnMalla = posicion;
            CargarImagenPoderes("Escudo.png");
        }
        public void ActivarInvulnerabilidad(Moto moto)
        {
            moto.PoderInvensivilidadActivado = true;
            colorOriginal = moto.ColorEstela; //Guardamos el colo que tenía originalmente.
            int duracionInvensivilidad = new Random().Next(1,4) * 1000;

            // Iniciar temporizador para el parpadeo
            timerParpadeo = new System.Timers.Timer(200); // Parpadeo cada 200ms
            timerParpadeo.Elapsed += (sender, e) => EfectoParpadeo(moto);
            timerParpadeo.Start();
            
            //Iniciar temporizador para desactivar la invensivilidad
            timerPoder = new System.Timers.Timer(duracionInvensivilidad);
            timerPoder.Elapsed += (sender,e) => DesactivarInvulnerabilidad(moto);
            timerPoder.AutoReset = false; //Para que se ejecute solo una vez
            timerPoder.Start();
        }

        private void EfectoParpadeo(Moto moto)
        {
            if (estaVisible)
            {
                moto.ColorEstela = colorInvencibilidad;
            }
            else
            {
                moto.ColorEstela = colorOriginal;
            }
            estaVisible = !estaVisible;
        }

        public void DesactivarInvulnerabilidad(Moto moto)
        {
            moto.PoderInvensivilidadActivado = false;

            timerPoder?.Stop();
            timerPoder?.Dispose();

            moto.ColorEstela = colorOriginal;
            timerParpadeo?.Stop();
            timerParpadeo?.Dispose();
        }
    }
}