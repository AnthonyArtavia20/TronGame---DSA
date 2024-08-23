using MallaGrid;

namespace poderesDelJuego
{
    public class HiperVelocidad: Poderes
    {
        private int ValorDeVelocidad;
        private static Random random = new Random();

        public HiperVelocidad(Nodo posicion)
        {
            ValorDeVelocidad = random.Next(5, 8);
            PosicionEnMalla = posicion;

            //Cargamos la imagen usando el nombre del archivo:
            CargarImagenPoderes("HiperVelocidad.PNG");
            Console.WriteLine("He dibujado un a hypervelcidad");
        }

        public int AplicarEfectoPoder()
        {
            return ValorDeVelocidad;
        }
    }
}