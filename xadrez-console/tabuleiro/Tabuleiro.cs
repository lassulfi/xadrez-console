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

        public Peca peca(int linha, int coluna)
        {
            return pecas[linha, coluna];
        }
    }
}
