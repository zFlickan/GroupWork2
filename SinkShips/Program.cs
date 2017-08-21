using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SinkShips
{
    class Program
    {
        static int numberOfBoats;

        static void Main(string[] args)
        {
            int lifeCount = 5;
            string[,] playingField = CreatePlayingField(); //skapar spelplan
            int[,] boatField = CreateBoatArray(AskForDifficulty()); // sätter båtar
            WritePlayingField(playingField); // skriver ut spelplanen
            TellMeNumberOfBoats();  // skriver hur många båtar det finns

            while (lifeCount > 0)
            {
                
                int[] userGuess = AskForCoordinates();
                bool isHit = IsHit(boatField, userGuess); //returnerar sant om träff
                playingField = UpdatePlayingField(playingField, userGuess, isHit);
                WritePlayingField(playingField);
                if (isHit == false)
                    lifeCount--; // todo: vore bra att veta hur många missar man har kvar
            }
            //todo: Man kan ju för bövelen inte vinna!
        }

        private static string[,] UpdatePlayingField(string[,] playingField, int[] userGuess, bool isHit)
        {
            int x = userGuess[0];
            int y = userGuess[1];
            playingField[x, y] = (isHit ? " X " : " O ");
            return playingField;
        }

        private static bool IsHit(int[,] boatField, int[] userGuess)
        {
            //Gör om userGuess till två ints "X" och "Y"
            bool hit;
            int x = userGuess[0];
            int y = userGuess[1];
            hit = (boatField[x, y] == 1 ? true : false);
            return hit;
            //    hit = true;
            //else
            //    hit = false;
            //return hit;
        }

        private static int[] AskForCoordinates()
        {
            // good: robust
            bool loop = true;
            int[] coordinates = new int[2];
            do
            {
                Console.Write("Skriv in koordinaterna du vill bombardera(exempelvis 1.1): ");
                string[] answer = Console.ReadLine().Split('.');
                if (IsValidInput(answer))
                {
                    coordinates = new int[] { int.Parse(Convert.ToString(answer[0])), int.Parse(Convert.ToString(answer[1])) };
                    loop = false;
                }
            } while (loop == true);
            return coordinates;

        }
        static bool IsValidInput(string[] input)
        {
            bool validInput = false;
            if ((Regex.IsMatch(input[0], @"^[0-3]+$")) && (Regex.IsMatch(input[1], @"^[0-3]+$"))) // todo: plustecknet skall bort (se kommentar i välja svårighetsgradens metod)
                validInput = true;
            return validInput;
        }
        

        private static int[,] CreateBoatArray(int difficulty)
        {
            // good: väldigt smidigt sätt att slumpa fram båtar på
            Random randomNumberGenerator = new Random();
            //int[,] boatPlaces = new int[4, 4] { { 1, 0, 1, 0 }, { 0, 0, 1, 0 }, { 0, 0, 1, 0 }, { 0, 0, 0, 0 } };
            int[,] boatPlaces = new int[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (randomNumberGenerator.Next(0, difficulty*4) < 3)
                    {
                        boatPlaces[i, j] = 1;
                        numberOfBoats++;
                    }
                    else
                    {
                        boatPlaces[i, j] = 0;
                    }
                }
                Console.WriteLine(); ;
            }
            return boatPlaces;
        }

        private static void WritePlayingField(string[,] playingField)
        {
            //good: Effektiv metod för att skriva ut och lagra båt och träff
            // todo: det vore bra att se rutornas koordinater i listen
            Console.Clear();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Console.Write(playingField[i,j]);
                }
                Console.WriteLine(); ;
            }
            
        }

        private static string[,] CreatePlayingField()
        {
            // todo: man kan välja storlek på spelfält
            string[,] playingField = new string[4, 4] {{" - "," - "," - "," - "},{" - "," - "," - "," - "},{" - "," - "," - "," - "}, { " - ", " - ", " - ", " - " } };
            return playingField;
            
        }
        static void TellMeNumberOfBoats()
        {
            // good: detta är bra info att ha innan spelet börjar
            Console.WriteLine($"Du ska träffa {numberOfBoats} båtar");
        }
        static int AskForDifficulty()
        {
            string input;
            bool loop = true;
            do
            {
                Console.WriteLine("Välj svårighetsgrad: (1)Jättelätt, (2)vanligt, (3)skitsvårt");
                input = Console.ReadLine();
                if (Regex.IsMatch(input, @"^[0-3]+$")) // todo: tag ut plustecknet. Det gör att minst en siffra skall vara 0-3; man kan alltså skriva tex 12
                    loop = false;

                // todo: man kan välja noll och detta är ett trivialt fall
            } while (loop == true);
            
            return int.Parse(input);
        }
        
    }
}
