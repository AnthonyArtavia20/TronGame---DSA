using MallaGrid;

namespace itemsDelJuego
{
    public class ItemAumentarEstela : Items
    {
        public int incrementoEstela;
        private static Random random = new Random();

        public ItemAumentarEstela(Nodo posicion)
        {
            incrementoEstela = random.Next(1, 6);
            PosicionEnMalla = posicion;

            // Cargar la imagen usando el nombre del archivo
            CargarImagen("AumentarEstelaImagen.png");
        }
    }
}
