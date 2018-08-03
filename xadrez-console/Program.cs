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
            //Teste da posicao
            Posicao p = new Posicao(3, 4);
            Console.WriteLine("Posição: " + p);

            //Teste de criacao do tabuleiro
            Tabuleiro tab = new Tabuleiro(8, 8);


            Console.ReadLine();

        }
    }
}
