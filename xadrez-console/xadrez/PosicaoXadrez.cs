using tabuleiro;

namespace xadrez
{
    //Classe auxiliar para traduzir um tabuleiro de xadrez com letras e números
    //em apenas números
    class PosicaoXadrez
    {
        //Atributos
        public char coluna { get; set; }
        public int linha { get; set; }

        //Construtor
        public PosicaoXadrez(char coluna, int linha)
        {
            this.coluna = coluna;
            this.linha = linha;
        }

        //Método de conversão
        public Posicao ToPosicao()
        {
            return new Posicao(8 - linha, coluna - 'a');
        }

        public override string ToString()
        {
            return "" + coluna + linha;
        }
    }
}
