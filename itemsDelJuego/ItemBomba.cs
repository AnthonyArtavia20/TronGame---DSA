using MallaGrid;

namespace itemsDelJuego
{
    public class ItemBomba : Items
    {

        public ItemBomba(Nodo posicion)
        {
            PosicionEnMalla = posicion;
            // Cargar la imagen usando el nombre del archivo
            CargarImagen("BombaFinal.jpg");
        }
    }
}
