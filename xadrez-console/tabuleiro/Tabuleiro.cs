namespace tabuleiro
{
    class Tabuleiro
    {
        //Atributos
        public int linhas { get; set; }
        public int colunas { get; set; }

        private Peca[,] pecas; //Matriz de peças

        //Construtor
        public Tabuleiro(int linhas, int colunas)
        {
            this.linhas = linhas;
            this.colunas = colunas;
            pecas = new Peca[linhas, colunas];
        }

        //retorna uma peça do tabuleiro
        public Peca peca(int linha, int coluna)
        {
            return pecas[linha, coluna];
        }

        //Coloca uma peça no tabuleiro
        public void colocarPeca(Peca p, Posicao pos)
        {
            pecas[pos.linha, pos.coluna] = p;
            p.posicao = pos;
        }
    }
}
