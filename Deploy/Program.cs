using System;

namespace QS.ALM.Deploy
{
    class Program
    {
        static int Main(string[] args)
        {
            const string flavor = @"Debug";
            var files = DeployHelper.HarvestFiles(flavor);
            
            try
            {
                

                Console.WriteLine("Please select:");
                Console.WriteLine();
                Console.WriteLine("1. Deploy Server");
                Console.WriteLine("2. Deploy Client");
                Console.WriteLine();
                Console.Write("Choice: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DeployServer.Deploy(files, flavor);
                        break;
                    case "2":
                        DeployClient.Deploy(files);
                        break;
                    default:
                        throw new Exception(string.Format("Invalid choice '{0}'", choice));
                }
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(ex.Message))
                    Console.WriteLine("Error: " + ex.Message + "");

                Console.WriteLine();
                Console.WriteLine("Error: " + ex.Message + "Press enter to exit");

                Console.ReadLine();
                return -1;
            }

            Console.WriteLine();
            Console.WriteLine("Success. Deploy Version: {0}", VersionHelper.GetLastDeployVersion());
            Console.WriteLine();
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
            return 0;
        }
    }
}
