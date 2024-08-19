using MallaGrid;

namespace itemsDelJuego
{
    public class ItemAumentarEstela : Items
    {
        private int incrementoEstela;
        public Nodo PosicionEnMalla { get; set; }
        private static Random random = new Random();
        private static Image? imagenAumentarEstela;

        public ItemAumentarEstela(Nodo posicion)
        {
            Nombre = "Aumentar Estela";
            incrementoEstela = random.Next(1, 6);
            PosicionEnMalla = posicion;

            if (imagenAumentarEstela == null)
            {
                string rutaImagen = @"C:\Users\Anthony\OneDrive - Estudiantes ITCR\TEC\2024\Segundo semestre\Algoritmos y Estructuras de Datos 1\Proyectos\Proyecto 1 Tron\Desarrollo del proyecto\TronGame\Asets\AumentarEstelaImagen.png";
                CargarImagen(rutaImagen);
                imagenAumentarEstela = Imagen; // Almacenar la imagen cargada
            }
            else
            {
                Imagen = imagenAumentarEstela; // Usar la imagen ya cargada
            }
        }

        public int AplicarEfecto()
        {
            // Lógica para aplicar el aumento de la estela
            return incrementoEstela;
        }
    }
}
