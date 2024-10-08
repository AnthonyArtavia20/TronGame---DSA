using MallaGrid;

namespace poderesDelJuego
{
    public class Poderes
    {
        public Image? Imagen {get; protected set;}
        public Nodo? PosicionEnMalla {get; set;}

        protected void CargarImagenPoderes(string nombreArchivo)
        {
            string rutaImagen = Path.Combine(
                @"C:\Users\Anthony\OneDrive - Estudiantes ITCR\TEC\2024\Segundo semestre\Algoritmos y Estructuras de Datos 1\Proyectos\Proyecto 1 Tron\Desarrollo del proyecto\TronGame\Asets\Poderes", 
                nombreArchivo
            );

            if (File.Exists(rutaImagen))
            {
                Imagen = Image.FromFile(rutaImagen);
            }
        }
    }
}