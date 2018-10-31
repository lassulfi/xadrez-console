namespace tabuleiro
{
    abstract class Peca
    {
        //Atributos
        public Posicao posicao { get; set; }
        public Cor cor { get; protected set; }
        public int qtdeMovimentos { get; protected set; }
        public Tabuleiro tab { get; protected set; }

        //Construtor
        public Peca(Tabuleiro tab, Cor cor)
        {
            this.posicao = null;
            this.tab = tab;
            this.cor = cor;
            this.qtdeMovimentos = 0;
        }

        public void incrementarQtdeMovimentos()
        {
            qtdeMovimentos++;
        }

        //Verifica se existe movimentos possíveis nas pecas
        public bool existeMovimentosPossiveis()
        {
            bool[,] mat = movimentosPossiveis();
            for(int i = 0; i < tab.linhas; i++)
            {
                for(int j = 0; j < tab.colunas; j++)
                {
                    if (mat[i, j]) return true;
                }
            }

            return false;
        }

        public abstract bool[,] movimentosPossiveis();

        //Verifica se a peca pode se mover para uma data posicao
        public bool movimentoPossivel(Posicao pos)
        {
            bool[,] mat = movimentosPossiveis();
            return mat[pos.linha, pos.coluna];
        }

        public void decremetarQtdeMovimentos()
        {
            qtdeMovimentos--;
        }
    }
}
