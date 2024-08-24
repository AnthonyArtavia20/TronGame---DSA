using Modelos;
using MallaGrid;

namespace poderesDelJuego
{
    public class Invensibilidad : Poderes
    {
        private System.Timers.Timer? timerPoder;

        public Invensibilidad(Nodo posicion)
        {
            PosicionEnMalla = posicion;
            CargarImagenPoderes("Escudo.png");
        }
        public void ActivarInvulnerabilidad(Moto moto)
        {
            moto.PoderInvensivilidadActivado = true;
            int duracionInvensivilidad = new Random().Next(1,4) * 1000;
            
            //Iniciar temporizador para desactivar la invensivilidad
            timerPoder = new System.Timers.Timer(duracionInvensivilidad);
            timerPoder.Elapsed += (sender,e) => DesactivarInvulnerabilidad(moto);
            timerPoder.AutoReset = false; //Para que se ejecute solo una vez
            timerPoder.Start();
        }

        public void DesactivarInvulnerabilidad(Moto moto)
        {
            moto.PoderInvensivilidadActivado = false;

            timerPoder?.Stop();
            timerPoder?.Dispose();
        }
    }
}