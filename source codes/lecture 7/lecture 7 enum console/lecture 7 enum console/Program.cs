using System;

namespace lecture_7_enum_console
{
    class Program
    {

        enum cities : long
        {
            Ankara = 6,
            Mersin = 33,
            İstanbul = 34,
            Adana = 1
        }
        static void Main(string[] args)
        {

            Console.WriteLine("plate nummber of " + cities.Adana + " is " + (int)cities.Adana);

            while (true)
            {
                Console.WriteLine("type one of the below cities name");
                foreach (cities city in (cities[])Enum.GetValues(typeof(cities)))
                {
                    Console.WriteLine(city);
                }
                var vrCity = Console.ReadLine();

                cities vrSelectedCity;

                bool blResult = Enum.TryParse(vrCity, out vrSelectedCity);

                if (!blResult)
                {
                    Console.WriteLine("you have made an invalid selection. please type the city name again");
                }
                else
                {
                    Console.WriteLine("the plate number of your selected city is : " + (int)vrSelectedCity);
                    break;
                }
            }
        }
    }
}
