using MallaGrid;

namespace itemsDelJuego
{
    public class Items
    {
        public string? Nombre { get; protected set; }
        public Image? Imagen { get; protected set; }
        public Nodo PosicionEnMalla { get; set; }

        protected void CargarImagen(string rutaImagen)
        {
            if (File.Exists(rutaImagen))
            {
                Imagen = Image.FromFile(rutaImagen);
            }
            else
            {
                CrearImagenPorDefecto();
            }
        }

        private void CrearImagenPorDefecto()
        {
            // Crear una imagen por defecto (un cuadrado amarillo de 32x32 píxeles)
            Imagen = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(Imagen))
            {
                g.Clear(Color.Yellow);
            }
            Console.WriteLine("Se ha creado una imagen por defecto.");
        }
    }
}
