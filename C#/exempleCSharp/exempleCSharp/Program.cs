using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using Microsoft.VisualBasic;

calcule();

static void calcule()
{
   int? nbr1;
    int? nbr2;
    string? operation = null;

    nbr1 = checkInt("Entrez un nombre :");
    nbr2 = checkInt("Entrez un autre nombre :");
    Console.WriteLine("Choixissez une opération : +, -, *, /");
    operation = Console.ReadLine();
    switch (operation)
    {
        case "+":
            Console.WriteLine($"Résultat : {nbr1 + nbr2}");
            break;
        case "-":
            Console.WriteLine($"Résultat : {nbr1 - nbr2}");
            break;
        case "*":
            Console.WriteLine($"Résultat : {nbr1 * nbr2}");
            break;
        case "/":
            Console.WriteLine($"Résultat : {nbr1 / nbr2}");
            break;
        default:
            Console.WriteLine("Opération non valide");
            break;
    }

}

static int checkInt(string message)
{
    string? nbr;
    int res;
    bool condition = false;
    do
    {
        Console.WriteLine(message);
        nbr = Console.ReadLine();
        condition = int.TryParse(nbr, out int result);
        res = result;
    }
    while (!condition);

    return res;
}


