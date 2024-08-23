using MallaGrid;

namespace itemsDelJuego
{
    public class ItemCombustible : Items
    {
        private int CantidadDeCombustible;
        private static Random random = new Random();
        public ItemCombustible(Nodo posicion)
        {
            CantidadDeCombustible = random.Next(20, 100);
            PosicionEnMalla = posicion;
            // Cargar la imagen usando el nombre del archivo
            CargarImagen("Combustible.png");
        }

        public int AplicarEfecto()
        {   
            return CantidadDeCombustible; // Retornar la cantidad de combustible que se añade
        }
    }
}