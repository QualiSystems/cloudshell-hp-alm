using System;

namespace QS.ALM.Deploy
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Please select:");
            Console.WriteLine();
            Console.WriteLine("1. Deploy Server");
            Console.WriteLine("2. Deploy Client");
            Console.WriteLine();
            Console.Write("Choice: ");

            var choice = Console.ReadLine();

            const string flavor = @"bin\Debug";

            try
            {
                switch (choice)
                {
                    case "1":
                        DeployServer.Deploy(flavor);
                        break;
                    case "2":
                        DeployClient.Deploy(flavor);
                        break;
                    default:
                        throw new Exception(string.Format("Invalid choice '{0}'", choice));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message + "\nPress enter to exit");
                Console.ReadLine();
                return -1;
            }

            Console.WriteLine();
            Console.WriteLine("Success.\nPress enter to exit");
            Console.ReadLine();
            return 0;
        }
    }
}
