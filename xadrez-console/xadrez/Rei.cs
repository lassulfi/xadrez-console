using tabuleiro;

namespace xadrez
{
    class Rei : Peca
    {
        //Construtor
        public Rei(Tabuleiro tab, Cor cor): base(tab, cor)
        {

        }

        public override string ToString()
        {
            return "R";
        }
    }
}
