using System;
using System.Collections.Generic;
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
        public bool xeque { get; private set; }

        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;

        //Construtor
        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branca;
            terminada = false;
            xeque = false;

            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();

            colocarPecas();
        }

        //Operacao que executa movimento
        public Peca executaMovimento(Posicao origem, Posicao destino)
        {
            //Retira a peca a ser movida
            Peca p = tab.retirarPeca(origem);
            //incrementa a quantidade de movimentos da peca
            p.incrementarQtdeMovimentos();
            //verifica se existe uma peça na posicao de destino e retira do tabuleiro
            Peca pecaCapturada = tab.retirarPeca(destino);
            //Coloca a peca na posicao de destino
            tab.colocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }

            // #jogadaespecial roque pequeno
            if(p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna + 1);
                Peca torre = tab.retirarPeca(origemTorre);
                torre.incrementarQtdeMovimentos();
                tab.colocarPeca(torre, destinoTorre);
            }

            //#jogadaespecial roque grande
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna - 1);
                Peca torre = tab.retirarPeca(origemTorre);
                torre.incrementarQtdeMovimentos();
                tab.colocarPeca(torre, destinoTorre);
            }

            return pecaCapturada;
        }

        //Retorna as pecas capturadas para uma dada cor
        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();

            foreach (Peca p in capturadas)
            {
                if (p.cor == cor)
                {
                    aux.Add(p);
                }
            }

            return aux;
        }

        //Retorna as pecas em jogo de uma cor
        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();

            foreach (Peca p in pecas)
            {
                if (p.cor == cor)
                {
                    aux.Add(p);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));

            return aux;
        }

        //Indentifica a cor da peça adversaria
        private Cor adversaria(Cor cor)
        {
            if (cor == Cor.Branca)
            {
                return Cor.Preta;
            }
            else
            {
                return Cor.Branca;
            }
        }

        //Devolve o rei de uma dada cor
        private Peca rei(Cor cor)
        {
            foreach (Peca p in pecasEmJogo(cor))
            {
                if (p is Rei)
                {
                    return p;
                }
            }

            return null;
        }

        //Testa o movimento possivel de todas as pecas
        public bool estaEmXeque(Cor cor)
        {
            //Recupera o rei da cor
            Peca r = rei(cor);
            if (r == null)
            {
                throw new TabuleiroException("Não tem rei da cor " + cor + " no tabuleiro");
            }

            foreach (Peca p in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = p.movimentosPossiveis();
                if (mat[r.posicao.linha, r.posicao.coluna])
                {
                    return true;
                }
            }
            return false;
        }

        //Valida a posicao de origem
        public void validarPosicaoDeOrigem(Posicao pos)
        {
            if (tab.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peca na posicao de origem escolhida");
            }
            if (jogadorAtual != tab.peca(pos).cor)
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
            if (!tab.peca(origem).movimentoPossivel(destino)) throw new TabuleiroException("Posicao de destino invalida");
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            //Executa o movimento
            Peca pecaCapturada = executaMovimento(origem, destino);
            //Verifica se o jogador atual não está em xeque
            if (estaEmXeque(jogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque");
            }

            if (estaEmXeque(adversaria(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }

            if (testeXequeMate(adversaria(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                //Passa o turno
                turno++;
                //Muda o jogador
                mudarJogador();
            }
        }

        public bool testeXequeMate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }
            foreach (Peca p in pecasEmJogo(cor))
            {
                bool[,] mat = p.movimentosPossiveis();
                for (int i = 0; i < tab.linhas; i++)
                {
                    for (int j = 0; j < tab.colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = p.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        //Desfaz um movimento caso a peça do jogador atual esteja em xeque
        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tab.retirarPeca(destino);
            p.decremetarQtdeMovimentos();
            if (pecaCapturada != null)
            {
                tab.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            tab.colocarPeca(p, origem);

            // #jogadaespecial roque pequeno
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna + 1);
                Peca torre = tab.retirarPeca(destinoTorre);
                torre.decremetarQtdeMovimentos();
                tab.colocarPeca(torre, origemTorre);
            }

            //#jogadaespecial roque grande
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna - 1);
                Peca torre = tab.retirarPeca(destinoTorre);
                torre.decremetarQtdeMovimentos();
                tab.colocarPeca(torre, origemTorre);
            }

        }

        private void mudarJogador()
        {
            if (jogadorAtual == Cor.Branca)
            {
                jogadorAtual = Cor.Preta;
            }
            else
            {
                jogadorAtual = Cor.Branca;
            }
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            pecas.Add(peca);
        }

        private void colocarPecas()
        {
            //Pecas brancas
            colocarNovaPeca('a', 1, new Torre(tab, Cor.Branca));
            colocarNovaPeca('b', 1, new Cavalo(tab, Cor.Branca));
            colocarNovaPeca('c', 1, new Bispo(tab, Cor.Branca));
            colocarNovaPeca('d', 1, new Dama(tab, Cor.Branca));
            colocarNovaPeca('e', 1, new Rei(tab, Cor.Branca, this));
            colocarNovaPeca('f', 1, new Bispo(tab, Cor.Branca));
            colocarNovaPeca('g', 1, new Cavalo(tab, Cor.Branca));
            colocarNovaPeca('h', 1, new Torre(tab, Cor.Branca));
            colocarNovaPeca('a', 2, new Peao(tab, Cor.Branca));
            colocarNovaPeca('b', 2, new Peao(tab, Cor.Branca));
            colocarNovaPeca('c', 2, new Peao(tab, Cor.Branca));
            colocarNovaPeca('d', 2, new Peao(tab, Cor.Branca));
            colocarNovaPeca('e', 2, new Peao(tab, Cor.Branca));
            colocarNovaPeca('f', 2, new Peao(tab, Cor.Branca));
            colocarNovaPeca('g', 2, new Peao(tab, Cor.Branca));
            colocarNovaPeca('h', 2, new Peao(tab, Cor.Branca));

            //Pecas pretas
            colocarNovaPeca('a', 8, new Torre(tab, Cor.Preta));
            colocarNovaPeca('b', 8, new Cavalo(tab, Cor.Preta));
            colocarNovaPeca('c', 8, new Bispo(tab, Cor.Preta));
            colocarNovaPeca('d', 8, new Dama(tab, Cor.Preta));
            colocarNovaPeca('e', 8, new Rei(tab, Cor.Preta, this));
            colocarNovaPeca('f', 8, new Bispo(tab, Cor.Preta));
            colocarNovaPeca('g', 8, new Cavalo(tab, Cor.Preta));
            colocarNovaPeca('h', 8, new Torre(tab, Cor.Preta));
            colocarNovaPeca('a', 7, new Peao(tab, Cor.Preta));
            colocarNovaPeca('b', 7, new Peao(tab, Cor.Preta));
            colocarNovaPeca('c', 7, new Peao(tab, Cor.Preta));
            colocarNovaPeca('d', 7, new Peao(tab, Cor.Preta));
            colocarNovaPeca('e', 7, new Peao(tab, Cor.Preta));
            colocarNovaPeca('f', 7, new Peao(tab, Cor.Preta));
            colocarNovaPeca('g', 7, new Peao(tab, Cor.Preta));
            colocarNovaPeca('h', 7, new Peao(tab, Cor.Preta));
        }
    }
}
