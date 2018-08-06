using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabuleiro;
using xadrez;

namespace xadrez_console
{
    class Program
    {
        static void Main(string[] args)
        {

            //Teste de criacao do tabuleiro
            Tabuleiro tab = new Tabuleiro(8, 8);

            tab.colocarPeca(new Torre(tab, Cor.Preta), new Posicao(0, 0));
            tab.colocarPeca(new Torre(tab, Cor.Preta), new Posicao(1, 3));
            tab.colocarPeca(new Rei(tab, Cor.Preta), new Posicao(2, 4));

            //Teste de impressao do tabuleiro
            Tela.imprimirTabuleiro(tab);

            Console.ReadLine();

        }
    }
}
