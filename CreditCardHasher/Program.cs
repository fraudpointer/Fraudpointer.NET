using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreditCardHasher
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.Error.WriteLine("Usave: cchash <cc> <cc> <cc> <cc> ...");    
                Console.WriteLine("You can use this program to hash a list of credit cards using Fraudpointer hash algorithm.");
                return 1;
            }

            var client = Fraudpointer.API.ClientFactory.Construct("", "");
            for (var i = 0; i < args.Length; i++)
            {
                var credit_card = args[i];
                var cc_hash = client.CreditCardHash(credit_card);
                Console.WriteLine(credit_card + "\t" + cc_hash);
            }
            return 0;
        }
    }
}
