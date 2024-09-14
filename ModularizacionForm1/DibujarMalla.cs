using poderesDelJuego;
using MallaGrid;  // Para acceder a la clase Malla.
using Modelos;   // Para acceder a la clase Moto, Bots, etc.
using itemsDelJuego;
using Controladores;

namespace ModularizacionForm1
{
    public class DibujadorMalla
    {
        private Malla malla;
        private List<Bots> bots;
        private Moto moto;
        private MotoJugador motoJugador;

        public DibujadorMalla(Malla malla, List<Bots> bots, Moto moto, MotoJugador motoJugador)
        {
            this.malla = malla;
            this.bots = bots;
            this.moto = moto;
            this.motoJugador = motoJugador;
        }

        public void Dibujar(Graphics g, Size clientSize)
        {

            Pen LineasSeparadoras = new Pen(Color.Black);
            float anchoCelda = (float)clientSize.Width / malla.Columnas;
            float altoCelda = (float)clientSize.Height / malla.Filas;

            // Dibujar líneas horizontales
            for (int i = 0; i < malla.Filas; i++)
            {
                g.DrawLine(LineasSeparadoras, 0, i * altoCelda, clientSize.Width, i * altoCelda);
            }

            // Dibujar líneas verticales
            for (int j = 0; j <= malla.Columnas; j++)
            {
                g.DrawLine(LineasSeparadoras, j * anchoCelda, 0, j * anchoCelda, clientSize.Height);
            }

            // Dibujar la Moto del jugador y su estela
            if (moto != null)
            {
                SolidBrush motoBrush = new SolidBrush(Color.Red);
                SolidBrush estelaBrush = new SolidBrush(moto.ColorEstela);
                
                var NodoEstelaMotoADibujar = moto.headEstela;
                while (NodoEstelaMotoADibujar != null)
                {
                    if (NodoEstelaMotoADibujar.Posicion == null)
                        {
                            return; //Se agregó esto para evitar el aviso de que podía haber una referencia nula.
                        }
                    g.FillRectangle(estelaBrush, NodoEstelaMotoADibujar.Posicion.Y * anchoCelda, NodoEstelaMotoADibujar.Posicion.X * altoCelda, anchoCelda, altoCelda);
                    NodoEstelaMotoADibujar = NodoEstelaMotoADibujar.Siguiente;
                }

                g.FillRectangle(motoBrush, moto.PosicionActual.Y * anchoCelda, moto.PosicionActual.X * altoCelda, anchoCelda, altoCelda);
            }
            DibujarPilaDePoderes(g, motoJugador);

            // Dibujar bots
            foreach (var bot in bots)
            {
                if (bot != null)
                {
                    SolidBrush botHeadBrush = new SolidBrush(Color.DarkGreen);
                    SolidBrush botEstelaBrush = new SolidBrush(bot.ColorEstela);

                    // Dibujar la estela del bot
                    var NodoEstelaBotADibujar = bot.headEstela;
                    while (NodoEstelaBotADibujar != null)
                    {
                        g.FillRectangle(botEstelaBrush, NodoEstelaBotADibujar.Posicion.Y * anchoCelda, NodoEstelaBotADibujar.Posicion.X * altoCelda, anchoCelda, altoCelda);
                        NodoEstelaBotADibujar = NodoEstelaBotADibujar.Siguiente;
                    }

                    g.FillRectangle(botHeadBrush, bot.PosicionActual.Y * anchoCelda, bot.PosicionActual.X * altoCelda, anchoCelda, altoCelda);
                }
            }

            // Dibujar ítems
            if (malla.ItemsEnMalla != null)
            {
                foreach (var item in malla.ItemsEnMalla)
                {
                    if (item is ItemAumentarEstela aumentarEstela)
                    {
                        if (aumentarEstela.PosicionEnMalla == null)
                        {
                            return;
                        }
                        if (aumentarEstela.Imagen != null)
                        {
                            g.DrawImage(aumentarEstela.Imagen, 
                                aumentarEstela.PosicionEnMalla.Y * anchoCelda, 
                                aumentarEstela.PosicionEnMalla.X * altoCelda, 
                                anchoCelda, altoCelda);
                        }
                    }

                    if (item is ItemCombustible combustible)
                    {
                        if (combustible.PosicionEnMalla == null)
                        {
                            return; //Se agregó esto para evitar que diera u aviso de que podía ser null
                        }
                        if (combustible.Imagen != null)
                        {
                            g.DrawImage(combustible.Imagen, 
                                combustible.PosicionEnMalla.Y * anchoCelda, 
                                combustible.PosicionEnMalla.X * altoCelda, 
                                anchoCelda, altoCelda);
                        }
                    }

                    if (item is ItemBomba bomba)
                    {
                        if (bomba.PosicionEnMalla == null)
                        {
                            return; //Se agregó esto para evitar que diera u aviso de que podía ser null
                        }
                        if (bomba.Imagen != null)
                        {
                            g.DrawImage(bomba.Imagen, 
                                bomba.PosicionEnMalla.Y * anchoCelda, 
                                bomba.PosicionEnMalla.X * altoCelda, 
                                anchoCelda, altoCelda);
                        }
                    }
                }
            }

            if (malla.PoderesEnMalla != null)
            {
                foreach (var poder in malla.PoderesEnMalla)
                {
                    if (poder is HiperVelocidad velocidadAumentada)
                    {
                        if (velocidadAumentada.PosicionEnMalla == null)
                        {
                            return; //Se agregó esto para evitar que diera u aviso de que podía ser null
                        }
                        if (velocidadAumentada.Imagen != null)
                        {
                            g.DrawImage(velocidadAumentada.Imagen, 
                                velocidadAumentada.PosicionEnMalla.Y * anchoCelda, 
                                velocidadAumentada.PosicionEnMalla.X * altoCelda, 
                                anchoCelda, altoCelda);
                        }
                    }
                    if (poder is Invensibilidad Invensibilidad)
                    {
                        if (Invensibilidad.PosicionEnMalla == null)
                        {
                            return; //Se agregó esto para evitar que diera u aviso de que podía ser null
                        }
                        if (Invensibilidad.Imagen != null)
                        {
                            g.DrawImage(Invensibilidad.Imagen, 
                                Invensibilidad.PosicionEnMalla.Y * anchoCelda, 
                                Invensibilidad.PosicionEnMalla.X * altoCelda, 
                                anchoCelda, altoCelda);
                        }
                    }
                }
            }
        }

        private void DibujarPilaDePoderes(Graphics g, MotoJugador motoJugador)
        {
            if (motoJugador.poderesPila.Tope == null) return; // No dibujar si la pila está vacía

            int y = 10;
            int x = 1000;
            int width = 150;
            int height = 20;

            // Dibujar un fondo para la lista de poderes
            g.FillRectangle(new SolidBrush(Color.FromArgb(128, 0, 0, 0)), x, y, width, height * 5);

            var nodoActual = motoJugador.poderesPila.Tope;
            while (nodoActual != null)
            {
                if (nodoActual.PoderAlmacenado == null)
                {
                    return; //Se agregó esto para evitar el aviso de que podía haber una referencia nula.
                }
                string poderNombre = nodoActual.PoderAlmacenado.GetType().Name;
                
                using (Font font = new Font("Arial", 10)) // Puedes ajustar el nombre y tamaño de la fuente
                {
                    g.DrawString(poderNombre, font, Brushes.Yellow, new Point(x + 5, y + 5));
                }

                y += height;
                nodoActual = nodoActual.Siguiente;
            }
        }
    }
}
