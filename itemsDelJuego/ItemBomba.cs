using MallaGrid;

namespace itemsDelJuego
{
    public class ItemBomba : Items
    {
        
        public ItemBomba(Nodo posicion)
        {
            Nombre = "Item Bomba";
            PosicionEnMalla = posicion;
            {
                string rutaImagen = @"C:\Users\Anthony\OneDrive - Estudiantes ITCR\TEC\2024\Segundo semestre\Algoritmos y Estructuras de Datos 1\Proyectos\Proyecto 1 Tron\Desarrollo del proyecto\TronGame\Asets\Items\BombaFinal.jpg";
                CargarImagen(rutaImagen);
            }
        }

        public void Explotar()
        {   
            MessageBox.Show("¡Oh no! Has muerto por una bomba.");
        }
    }
}