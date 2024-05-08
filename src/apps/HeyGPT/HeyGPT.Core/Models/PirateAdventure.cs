namespace HeyGPT.Core.Models;

public class PirateAdventure
{
    public static void GenerateTreasureMap(int seed)
    {
        var random = new Random(seed);
        var rows = 10; // Define the number of rows in the map
        var cols = 10; // Define the number of columns in the map
        var map = new char[rows, cols];

        // Fill the map with water and land
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                // Randomly place land or water
                map[i, j] = random.Next(100) < 20 ? '.' : '~'; // 20% chance of land
            }
        }

        // Randomly place a treasure
        var treasureX = random.Next(rows);
        var treasureY = random.Next(cols);
        map[treasureX, treasureY] = 'X';

        // Print the map
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                Console.Write(map[i, j] + " ");
            }
            Console.WriteLine();
        }
    }
}
