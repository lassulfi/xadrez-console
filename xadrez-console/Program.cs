using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tabuleiro;

namespace xadrez_console
{
    class Program
    {
        static void Main(string[] args)
        {

            //Teste de criacao do tabuleiro
            Tabuleiro tab = new Tabuleiro(8, 8);

            //Teste de impressao do tabuleiro
            Tela.imprimirTabuleiro(tab);

            Console.ReadLine();

        }
    }
}
