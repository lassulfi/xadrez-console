using System;
using tabuleiro;

namespace xadrez
{
    class PartidaDeXadrez
    {
        //Atributos
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }

        //Construtor
        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            colocarPecas();
        }

        //Operacao que executa movimento
        public void executaMovimento(Posicao origem, Posicao destino)
        {
            //Retira a peca a ser movida
            Peca p = tab.retirarPeca(origem);
            //incrementa a quantidade de movimentos da peca
            p.incrementarQtdeMovimentos();
            //verifica se existe uma peça na posicao de destino e retira do tabuleiro
            Peca pecaCapturada = tab.retirarPeca(destino);
            //Coloca a peca na posicao de destino
            tab.colocarPeca(p, destino);
        }

        //Valida a posicao de origem
        public void validarPosicaoDeOrigem(Posicao pos)
        {
            if(tab.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peca na posicao de origem escolhida");
            }
            if(jogadorAtual != tab.peca(pos).cor)
            {
                throw new TabuleiroException("Escolhida peca do adversario");
            }
            if (!tab.peca(pos).existeMovimentosPossiveis())
            {
                throw new TabuleiroException("Não existe movimentos possiveis para a peca escolhida");
            }
        }

        //Valida a posicao de destino da peca
        public void validarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).podeMoverPara(destino)) throw new TabuleiroException("Posicao de destino invalida");
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            //Executa o movimento
            executaMovimento(origem, destino);
            //Passa o turno
            turno++;
            //Muda o jogador
            mudarJogador();
        }

        private void mudarJogador()
        {
            if(jogadorAtual == Cor.Branca)
            {
                jogadorAtual = Cor.Preta;
            }
            else
            {
                jogadorAtual = Cor.Branca;
            }
        }

        private void colocarPecas()
        {
            //Pecas brancas
            tab.colocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('c', 1).ToPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('c', 2).ToPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('d', 2).ToPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('e', 1).ToPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Branca), new PosicaoXadrez('e', 2).ToPosicao());
            tab.colocarPeca(new Rei(tab, Cor.Branca), new PosicaoXadrez('d', 1).ToPosicao());
            //Pecas pretas
            tab.colocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('c', 7).ToPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('c', 8).ToPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('d', 7).ToPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('e', 7).ToPosicao());
            tab.colocarPeca(new Torre(tab, Cor.Preta), new PosicaoXadrez('e', 8).ToPosicao());
            tab.colocarPeca(new Rei(tab, Cor.Preta), new PosicaoXadrez('d', 8).ToPosicao());
        }
    }
}
