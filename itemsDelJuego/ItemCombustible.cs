using MallaGrid;

namespace itemsDelJuego
{
    public class ItemCombustible : Items
    {
        private int CantidadDeCombustible;
        public Nodo PosicionEnMalla { get; set; }
        private static Random random = new Random();
        private static Image? ImagenCombustible;

        public ItemCombustible(Nodo posicion)
        {
            Nombre = "Combustible";
            CantidadDeCombustible = random.Next(20, 100);
            PosicionEnMalla = posicion;
            
            if (ImagenCombustible == null)
            {
                string rutaImagen = @"C:\Users\Anthony\OneDrive - Estudiantes ITCR\TEC\2024\Segundo semestre\Algoritmos y Estructuras de Datos 1\Proyectos\Proyecto 1 Tron\Desarrollo del proyecto\TronGame\Asets\PROBAR.png";
                CargarImagen(rutaImagen);
                ImagenCombustible = Imagen;
            }
            else
            {
                Imagen = ImagenCombustible; //Usar imagenes para pruebas.
            }
        }

        public int AplicarEfecto()
        {
            //Implementar lógica para aumentar combustible.
            return CantidadDeCombustible;
        }

    }
}