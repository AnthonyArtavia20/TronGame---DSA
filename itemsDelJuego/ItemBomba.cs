using MallaGrid;

namespace itemsDelJuego
{
    public class ItemBomba : Items
    {
        public int incrementoEstela;
        private static Random random = new Random();

        public ItemBomba(Nodo posicion)
        {
            incrementoEstela = random.Next(1, 6);
            PosicionEnMalla = posicion;

            // Cargar la imagen usando el nombre del archivo
            CargarImagen("BombaFinal.jpg");
        }
    }
}
