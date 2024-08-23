using MallaGrid;

namespace itemsDelJuego
{
    public class Items
    {
        public string? Nombre { get; protected set; }
        public Image? Imagen { get; protected set; }
        public Nodo? PosicionEnMalla { get; set; }

        protected void CargarImagen(string nombreArchivo)
        {
            string rutaImagen = Path.Combine(
                @"C:\Users\Anthony\OneDrive - Estudiantes ITCR\TEC\2024\Segundo semestre\Algoritmos y Estructuras de Datos 1\Proyectos\Proyecto 1 Tron\Desarrollo del proyecto\TronGame\Asets\Items", 
                nombreArchivo
            );

            if (File.Exists(rutaImagen))
            {
                Imagen = Image.FromFile(rutaImagen);
            }
        }
    }
}
