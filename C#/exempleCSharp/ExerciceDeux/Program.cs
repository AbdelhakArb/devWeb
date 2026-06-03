// See https://aka.ms/new-console-template for more information
double temperature;
string? fromUnit;
Console.WriteLine("Entrez la température à convertir :");
temperature = double.Parse(Console.ReadLine());
Console.WriteLine("Entrez l'unité de la température (C pour Celsius, F pour Fahrenheit) :");
fromUnit = Console.ReadLine();
convertTemperature(temperature, fromUnit);

static double convertTemperature(double temperature, string fromUnit)
{
    if (fromUnit == "C")
    {
         return Math.Round(((temperature - 32) * 5 / 9), 2);
       
    }
    else if (fromUnit == "F")
    {
        return Math.Round((temperature * 9 / 5) + 32, 2);
    }
    else
    {
        throw new ArgumentException("Unité de température non valide");
    }
}

