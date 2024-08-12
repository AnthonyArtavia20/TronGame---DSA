//Lógica de los bots controlador de forma aleatoria
using MallaGrid;
using Modelos;

namespace Controladores
{
    public class Bots : Moto
    {
        private Random random = new Random();

        public Bots(Nodo posicionInicial,int longitudInicialEstela = 3) : base(posicionInicial,longitudInicialEstela)
        {
        }

        public void MoverAleatoriamenteBots()
        {
            int direccion = random.Next(4); // entre el 0 y 3
            switch (direccion)
        {
            case 0: MoverArriba(); break;
            case 1: MoverAbajo(); break;
            case 2: MoverIzquierda(); break;
            case 3: MoverDerecha(); break;
        }
        }
    }
}