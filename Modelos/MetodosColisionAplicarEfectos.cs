
using EstructurasDeDatos;
using itemsDelJuego;
using poderesDelJuego;
using Controladores;

namespace Modelos
{
    public class MotoColisionAplicarPoderesItems
    {
        private Moto moto;

        public MotoColisionAplicarPoderesItems(Moto moto)
        {
            this.moto = moto;
        }

        /*Ahora creamos los diferentes métodos que comprobarán si se puede realizar el movimiento que se desea, esto se logra verificando las
        condiciones incialmente establecidas en la clase MallaGrid, donde se verifica si, por ejemplo, arriba del primer nodo hay algo, en ese caso
        como no hay nada ya que el primer nodo es [0,0] encima de él hay Null, es decir nada, en este caso no se hace la condición de:
        [i-1,j] que permite establecer la posición una fila atrás. Es bajo este mismo principio que se cumpleas las 4 condiciones.*/
        
        //Método para poder detectar colisiones con los items en la malla:
        public void VerificarColisionConItems()
        {
            foreach (var item in moto.malla.ItemsEnMalla)
            {
                if (item.PosicionEnMalla == null)
                {
                    return;
                }
                if (moto.PosicionActual.X == item.PosicionEnMalla.X && moto.PosicionActual.Y == item.PosicionEnMalla.Y)
                {
                    // Se ha detectado una colisión con un ítem
                    moto.itemsCola.EnColar(new NodoItemsCola { ItemAlmacenado = item });
                    // Remover el ítem de la malla

                    moto.malla.ItemsEnMalla.Remove(item);
                    break; // Salir del bucle después de aplicar el efecto
                }
            }
        }

        public async void ProcesarColaDeItems()
        {
            while (moto.itemsCola.Inicio != null && moto.estaEnMovimiento)
            {
                var nodoItem = moto.itemsCola.Inicio;
                if (nodoItem?.ItemAlmacenado is Items item)
                {
                    if (item is ItemBomba && this.moto is Bots)
                    {
                        moto.DetenerMoto();
                        moto.itemsCola.Desencolar();
                        break;
                    }
                    else if (item is ItemCombustible && moto.Combustible >= 100)
                    {
                        // Si es un ítem de combustible y el tanque está lleno, lo volvemos a encolar
                        var itemDesencolar = moto.itemsCola.Desencolar();
                        if (itemDesencolar != null)
                        {
                            moto.itemsCola.EnColar(itemDesencolar);
                        }
                    }
                    else
                    {
                        AplicarEfectoDelItem(item);
                        moto.itemsCola.Desencolar();
                    }
                }
                else
                {
                    moto.itemsCola.Desencolar();
                }
        
                await Task.Delay(1000);
            }
        }

        public void AplicarEfectoDelItem(Items item)//Se vuelve virtual para poder aplicar polimorfismo
        {
            switch (item)
            {
                case ItemAumentarEstela aumentoEstela:
                    moto.LongitudEstela += aumentoEstela.incrementoEstela; // Aumentar la longitud de la estela
                    break; 
                case ItemCombustible combustible:
                    moto.Combustible += combustible.AplicarEfecto(); // Aumentar el combustible
                    break;
                case ItemBomba bomba:
                    if (this.moto is MotoJugador && !moto.PoderInvensivilidadActivado) // Verifica si es el jugador porque anteriormente esto dió un reguero de bugs
                    {
                        moto.DetenerMoto();
                        MessageBox.Show("¡Perdiste por una bomba!"); // Muestra el mensaje de que perdió
                        Environment.Exit(0);
                    }
                    else if(this.moto is Bots && !moto.PoderInvensivilidadActivado)
                    {
                        moto.DetenerMoto(); // Detiene el movimiento de los bots
                    }
                    break;
            }
        }

        public void VerificarColisionConPoderes()
        {
            foreach (var poder in moto.malla.PoderesEnMalla)
            {
                if (poder.PosicionEnMalla == null)
                    {
                        return; //Se agregó esto para evitar una referencia nula.
                    }
                if (moto.PosicionActual.X == poder.PosicionEnMalla.X && moto.PosicionActual.Y == poder.PosicionEnMalla.Y)
                {
                    //Se ha detectado una colisión con un poder
                    moto.poderesPila.Apilar(new NodosPilaDePoderes {PoderAlmacenado = poder});

                    // Remover el ítem de la malla
                    moto.malla.PoderesEnMalla.Remove(poder);
                    break;
                }
            }
        }

        public virtual void AplicarEfectoDelPoder(Poderes poder)
        {
            switch(poder)
            {
                case HiperVelocidad velocidadAumentada:
                    velocidadAumentada.ActivarHiperVelocidad(this.moto);
                    break;
                case Invensibilidad invensibilidad:
                    invensibilidad.ActivarInvulnerabilidad(this.moto);
                    break;
                default:
                    break;
                
            }
        }
    }
}