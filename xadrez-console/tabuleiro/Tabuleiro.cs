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

        public Peca peca(Posicao pos)
        {
            return pecas[pos.linha, pos.coluna];
        }

        //Verifica se existe uma peça em uma dada posicao
        public bool existePeca(Posicao pos)
        {
            validarPosicao(pos);
            return peca(pos) != null;
        }

        //Coloca uma peça no tabuleiro
        public void colocarPeca(Peca p, Posicao pos)
        {
            if (existePeca(pos))
            {
                throw new TabuleiroException("Já existe uma peça nessa posição.");
            }
            pecas[pos.linha, pos.coluna] = p;
            p.posicao = pos;
        }
        
        //Retira uma peca do tabuleiro
        public Peca retirarPeca(Posicao pos)
        {
            if(peca(pos) == null)
            {
                return null;
            }
            
            //Variavel auxiliar para retirar a peca na posicao desejada
            Peca aux = peca(pos);
            aux.posicao = null;

            pecas[pos.linha, pos.coluna] = null;

            return aux;
        }
        //Valida a posicao informada
        public bool posicaoValida(Posicao pos)
        {
            if(pos.linha < 0 || pos.linha >= linhas || pos.coluna < 0 || pos.coluna >= colunas)
            {
                return false;
            }

            return true;
        }

        //Valida a posicao e lanca uma exceção personalizada
        public void validarPosicao(Posicao pos)
        {
            if (!posicaoValida(pos))
            {
                throw new TabuleiroException("Posição inválida.");
            }
        }
    }
}
