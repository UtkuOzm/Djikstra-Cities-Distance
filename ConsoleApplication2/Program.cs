using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using OfficeOpenXml;

class Program
{
    static void Random10City(int[][] array, String[] array2)
    {
        Random random = new Random();
        for (int i = 0; i < 10; i++)
        {
            int first = random.Next(1, 81); //random city selection
            int second = random.Next(1, 81); //random city selection


            Console.WriteLine(array2[first] + " license plate number of the province: " + first); // Prints of license plate numbers
            Console.WriteLine(array2[second] + " license plate number of the province: " + second); //Prints of license plate numbers

            if (first > second)
            {
                Console.WriteLine("Distance between two provinces: " + array[first - 1][second - 1]); // Prints of distances
            }
            else
            {
                Console.WriteLine("Distance between two provinces: " + array[second - 1][first - 1]);//Prints of distances
            }

            Console.WriteLine("");
        }
    }

    static double[][] doubleDjikstra(double[][] array) // Provides implementation of Dijkstra's algorithm
    {
        for (int i = 0; i < 30; i++)
        {
            List<double> visitedCityList = new List<double>(); // Creates a list that holds the indexes of the visited cities
            double[] copy = array[i].Clone() as double[]; // Gets a copy of the distance array to the current starting city

            for (int j = 0; j < copy.Length; j++) // All cities are traveled over.
            {
                double minCityValue = copy.Min(); // Find the smallest distance in the current distance array
                int minCityIndex = Array.IndexOf(copy, minCityValue); // Get the index of the city having this distance.
                for (int k = 0; k < copy.Length; k++) // Update distances from this city to other cities.
                {
                    if (array[i][k] > minCityValue + array[minCityIndex][k]) // If a shorter path is found than the current distance, this distance is updated.
                    {
                        array[i][k] = minCityValue + array[minCityIndex][k];
                    }
                }

                visitedCityList.Add(minCityIndex); // Minimum distance city is added to the visited list.
                copy = array[i].Clone() as double[]; // Distance array is copied again and the distance of visited cities is marked as infinite.
                for (int z = 0; z < visitedCityList.Count; z++) // Distance of visited cities is set to a large value (1000000) which is considered "infinite"
                {
                    copy[(int)visitedCityList[z]] = 1000000;
                }
            }
        }

        return array; // An array of updated distances is returned.
    }

    static int[][] Djikstra(int[][] array) // Provide implementation of Dijkstra's algorithm
    {
        for (int i = 0; i < 81; i++) // The algorithm is run for 81 starting cities.
        {
            List<int> visitedCityList = new List<int>(); // A list is created that keeps the indexes of the visited cities.
            int[] copy = array[i].Clone() as int[];  // A copy of the distance array to the current starting city is made.

            for (int j = 0; j < copy.Length; j++) // All cities are traveled over.
            {
                int minCityValue = copy.Min(); // Find the smallest distance in the current distance array.
                int minCityIndex = Array.IndexOf(copy, minCityValue); // Get the index of the city that has this distance.
                for (int k = 0; k < copy.Length; k++) // Check the distances from the minimum distance city to other cities and update if necessary.
                {
                    if (array[i][k] > minCityValue + array[minCityIndex][k]) // If a shorter path is found than the current distance, this distance is updated.
                    {
                        array[i][k] = minCityValue + array[minCityIndex][k];
                    }
                }

                visitedCityList.Add(minCityIndex); // The city with the minimum distance is added to the visited list.
                copy = array[i].Clone() as int[]; // Distance array is copied again and the distance of visited cities is marked as "infinite".
                for (int z = 0; z < visitedCityList.Count; z++) // The distance of visited cities is set to a large value (1000000), which is considered "infinite".
                {
                    copy[visitedCityList[z]] = 1000000;
                }
            }
        }

        return array;// An array of updated distances is returned.
    }

    static double[][] townInfinityRoadDistances(double[][] array, int[][] neighbourTowns) // A function that marks the distance between non-neighboring towns as "infinite".
    {
        for (int i = 0; i < 30; i++) // All townsare navigated
        {
            for (int j = 0; j < 30; j++) // The inner loop that controls the connection of each town to other towns.
            {
                if (neighbourTowns[i][j] == 0) // If there is no connection between two towns (0), the distance is marked as "infinite" (1000000).
                {
                    array[i][j] = 1000000; // "Infinite" distance is assigned 
                }
            }
        }

        return array; // An array of updated distances is returned.
    }

    static int[][] InfinityRoadDistances(int[][] array, Dictionary<int, List<int>> neighbour) //  Function that marks the distance between non-neighboring cities as "infinite".
    {
        int[][] copy = array.Clone() as int[][]; // Create a copy of the distances matrix
        for (int i = 1; i < 82; i++) // Loop over all cities (starting from cities 1).
        {
            if (((ICollection)copy[0]).Count == 81) // If the first row of the distances array consists of 81 elements.
            {
                for (int j = 1; j < 82; j++) // Controls the connection of cities to other cities.
                {
                    if (!neighbour[i].Contains(j)) // If neighbour dictionary does not contain j for city i.
                    {
                        copy[i - 1][j - 1] = 1000000; // Distance is marked as "infinite".
                    }
                }
            }
            else
            {
                for (int j = 1; j < i + 1; j++) // If there are fewer cities or a different structure.
                {
                    if (!neighbour[i].Contains(j)) // If city j is not a neighbor of city i.
                    {
                        if (j < i) // Daha düşük indeksli şehirlerde çalışır.
                        {
                            copy[i - 1][j - 1] = 1000000; // Distance is marked as "infinite".
                        }
                    }
                }
            }
        }

        return copy; // An array of updated distances is returned.
    }

    static void Main(string[] args)
    {
        using (StreamWriter writer = new StreamWriter("output.txt")) // Create a StreamWriter to write to the "output.txt" file
        {
            Console.SetOut(writer);

            int[][] jaggedArray = new int[81][]; //indexes:plate number-1
            int[][] fullArray = new int[81][];
            String[] cityArray = new String[82]; //indexes of plaka no 
            double[][] townFullArray = new double[30][];
            cityArray[0] = null;
            for (int i = 0; i < 30; i++) // A 30x30 array is created and 30 elements are assigned for each row.
            {
                townFullArray[i] = new double[30];
            }

            for (int i = 0; i < 81; i++) // For each row of the jagged array, the length is assigned according to the row index.
            {
                jaggedArray[i] = new int[i + 1];
            }

            for (int i = 0; i < 81; i++) // A full 81x81 array is created
            {
                fullArray[i] = new int[81];
            }

            string[] townofizmir = // Towns in İzmir
            {
                "Aliağa", "Balçova", "Bayındır", "Bayraklı", "Bergama",
                "Beydağ", "Bornova", "Buca", "Çeşme", "Çiğli",
                "Dikili", "Foça", "Gaziemir", "Güzelbahçe", "Karabağlar",
                "Karaburun", "Karşıyaka", "Kemalpaşa", "Kınık", "Kiraz",
                "Konak", "Menderes", "Menemen", "Narlıdere", "Ödemiş",
                "Seferihisar", "Selçuk", "Tire", "Torbalı", "Urla"
            };
            int[][] neighbourTowns = new int[30][]; //Izmir's neighboring towns matrix
            neighbourTowns[0] = new int[]
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 };
            neighbourTowns[1] = new int[]
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0 };
            neighbourTowns[2] = new int[]
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 0 };
            neighbourTowns[3] = new int[]
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            neighbourTowns[4] = new int[]
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            neighbourTowns[5] = new int[]
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 };
            neighbourTowns[6] = new int[]
                { 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0 };
            neighbourTowns[7] = new int[]
                { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0 };
            neighbourTowns[8] = new int[]
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
            neighbourTowns[9] = new int[]
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 };
            neighbourTowns[10] = new int[]
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            neighbourTowns[11] = new int[]
                { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 };
            neighbourTowns[12] = new int[]
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 };
            neighbourTowns[13] = new int[]
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 1 };
            neighbourTowns[14] = new int[]
                { 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0, 0, 0, 0 };
            neighbourTowns[15] = new int[]
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
            neighbourTowns[16] = new int[]
                { 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 };
            neighbourTowns[17] = new int[]
                { 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 };
            neighbourTowns[18] = new int[]
                { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            neighbourTowns[19] = new int[]
                { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 };
            neighbourTowns[20] = new int[]
                { 0, 1, 0, 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            neighbourTowns[21] = new int[]
                { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 1, 0 };
            neighbourTowns[22] = new int[]
                { 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            neighbourTowns[23] = new int[]
                { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            neighbourTowns[24] = new int[]
                { 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 };
            neighbourTowns[25] = new int[]
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1 };
            neighbourTowns[26] = new int[]
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 0 };
            neighbourTowns[27] = new int[]
                { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0 };
            neighbourTowns[28] = new int[]
                { 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0 };
            neighbourTowns[29] = new int[]
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 };
            Dictionary<int, List<int>> neighbour = new Dictionary<int, List<int>> //List of neighboring cities
            {
                { 1, new List<int> { 33, 31, 80, 51, 46 } },
                { 2, new List<int> { 21, 27, 46, 63, 44 } },
                { 3, new List<int> { 15, 26, 32, 43, 64, 42, 20 } },
                { 4, new List<int> { 36, 76, 65, 25, 49, 13 } },
                { 5, new List<int> { 19, 60, 55, 66 } },
                { 6, new List<int> { 18, 14, 26, 42, 40, 71, 68, 3 } },
                { 7, new List<int> { 70, 32, 33, 15, 48, 42 } },
                { 8, new List<int> { 53, 25, 75 } },
                { 9, new List<int> { 35, 45, 48, 20 } },
                { 10, new List<int> { 16, 35, 45, 54, 17, 43 } },
                { 11, new List<int> { 16, 43, 26, 54, 14 } },
                { 12, new List<int> { 21, 23, 24, 25, 62, 49 } },
                { 13, new List<int> { 65, 4, 49, 56, 72 } },
                { 14, new List<int> { 54, 81, 78, 18, 26, 67, 6, 11 } },
                { 15, new List<int> { 32, 3, 20, 7, 48 } },
                { 16, new List<int> { 41, 11, 43, 10, 54, 77 } },
                { 17, new List<int> { 22, 10, 59 } },
                { 18, new List<int> { 78, 71, 6, 14, 37, 19 } },
                { 19, new List<int> { 71, 55, 5, 66, 57, 37, 18 } },
                { 20, new List<int> { 9, 15, 3, 64, 45, 48 } },
                { 21, new List<int> { 47, 63, 72, 49, 12, 23, 2, 44 } },
                { 22, new List<int> { 17, 59, 39 } },
                { 23, new List<int> { 62, 21, 12, 44 } },
                { 24, new List<int> { 25, 69, 29, 12, 62, 58, 44, 23, 28 } },
                { 25, new List<int> { 75, 24, 12, 69, 8, 4, 49, 36, 53, 61 } },
                { 26, new List<int> { 6, 43, 11, 3, 14, 42 } },
                { 27, new List<int> { 46, 31, 79, 63, 80, 2 } },
                { 28, new List<int> { 52, 58, 29, 24, 61 } },
                { 29, new List<int> { 24, 28, 61, 69 } },
                { 30, new List<int> { 65, 73 } },
                { 31, new List<int> { 1, 27, 80, 79 } },
                { 32, new List<int> { 3, 7, 42, 15 } },
                { 33, new List<int> { 1, 7, 42, 70, 51 } },
                { 34, new List<int> { 41, 59, 39 } },
                { 35, new List<int> { 9, 45, 10 } },
                { 36, new List<int> { 4, 25, 75, 76 } },
                { 37, new List<int> { 18, 78, 74, 19, 57 } },
                { 38, new List<int> { 66, 50, 1, 46, 58, 51 } },
                { 39, new List<int> { 22, 59, 34 } },
                { 40, new List<int> { 71, 50, 68, 66, 6 } },
                { 41, new List<int> { 77, 34, 16, 54 } },
                { 42, new List<int> { 3, 70, 32, 26, 6, 68, 7, 33, 51 } },
                { 43, new List<int> { 10, 16, 3, 11, 26, 45, 64 } },
                { 44, new List<int> { 23, 24, 2, 58, 46 } },
                { 45, new List<int> { 9, 35, 10, 43, 64 } },
                { 46, new List<int> { 58, 44, 1, 80, 2, 38, 27 } },
                { 47, new List<int> { 21, 63, 72, 56, 73 } },
                { 48, new List<int> { 20, 7, 9, 15 } },
                { 49, new List<int> { 12, 13, 65, 21, 25, 4 } },
                { 50, new List<int> { 38, 51, 68, 66, 40 } },
                { 51, new List<int> { 50, 38, 68, 42, 1, 33 } },
                { 52, new List<int> { 28, 58, 55, 60 } },
                { 53, new List<int> { 25, 61, 8, 69 } },
                { 54, new List<int> { 16, 11, 14, 81, 41 } },
                { 55, new List<int> { 52, 19, 57, 66, 5 } },
                { 56, new List<int> { 73, 47, 72, 65, 13 } },
                { 57, new List<int> { 37, 55, 19 } },
                { 58, new List<int> { 52, 28, 24, 44, 46, 60, 38, 66 } },
                { 59, new List<int> { 34, 22, 17, 39 } },
                { 60, new List<int> { 55, 66, 5, 52, 58 } },
                { 61, new List<int> { 53, 28, 29, 69 } },
                { 62, new List<int> { 23, 24, 12 } },
                { 63, new List<int> { 2, 47, 21, 27 } },
                { 64, new List<int> { 3, 43, 45, 20 } },
                { 65, new List<int> { 30, 4, 13, 56, 73 } },
                { 66, new List<int> { 38, 19, 50, 5, 60, 58, 40, 71 } },
                { 67, new List<int> { 81, 14, 74, 78, 18 } },
                { 68, new List<int> { 42, 51, 50, 40, 6 } },
                { 69, new List<int> { 25, 24, 29, 53, 61 } },
                { 70, new List<int> { 42, 33, 7 } },
                { 71, new List<int> { 40, 6, 18, 19, 66 } },
                { 72, new List<int> { 21, 49, 56, 47, 13 } },
                { 73, new List<int> { 30, 56, 47, 65 } },
                { 74, new List<int> { 67, 37, 78 } },
                { 75, new List<int> { 36, 8, 25 } },
                { 76, new List<int> { 36, 4 } },
                { 77, new List<int> { 41, 16 } },
                { 78, new List<int> { 37, 67, 64, 18, 14 } },
                { 79, new List<int> { 27, 31 } },
                { 80, new List<int> { 27, 46, 31, 1 } },
                { 81, new List<int> { 67, 14, 54 } }
            };
            // You may need to accept the license to use ExcelPackage
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            // Path of Excel file to read
            string cityFilePath = "ilmesafe.xlsx";

            //Check if the file exists
            if (File.Exists(cityFilePath)) // If the file exists in the specified path, perform the operation.
            {
                using (var package = new ExcelPackage(new FileInfo(cityFilePath))) // An ExcelPackage object is created to read the specified Excel file.
                {
                    // We get the first page (worksheet)
                    ExcelWorksheet cityWorksheet = package.Workbook.Worksheets[0];

                    // Set row and column boundaries
                    int cityRowCount = cityWorksheet.Dimension.Rows;
                    int cityColCount = cityWorksheet.Dimension.Columns;
                    // Reading data
                    for (int row = 3; row <= cityRowCount; row++)
                    {
                        for (int col = 3; col <= cityColCount; col++)
                        {
                            // Reading the value in a cell
                            var cellValue = cityWorksheet.Cells[row, col].Text;
                            if (cellValue != "0")
                            {
                                jaggedArray[row - 3][col - 3] = int.Parse(cellValue);
                            }
                            else
                            {
                                break; // Break the loop when "0" is found.
                            }
                        }
                    }

                    for (int row = 3; row <= cityRowCount; row++) // Loop for reading data (for full matrix).
                    {
                        for (int col = 3; col <= cityColCount; col++)
                        {
                            var cellValue = cityWorksheet.Cells[row, col].Text; // Read the value from the cell and add it to fullArray.
                            fullArray[row - 3][col - 3] = int.Parse(cellValue);
                        }
                    }

                    for (int row = 3; row <= cityRowCount; row++) //Loop adding city names to cityArray.
                    {
                        var cellValue = cityWorksheet.Cells[row, 2].Text; // Read the city name in the second column.
                        cityArray[row - 2] = cellValue;
                    }
                }
            }
            else
            {
                Console.WriteLine("File Not Found"); // Print file not found error message.
            }

            string townFilePath = "ilcemesafefinal.xlsx"; // Processing district distances file
            if (File.Exists(townFilePath))
            {
                using (var package = new ExcelPackage(new FileInfo(townFilePath))) // An ExcelPackage object is created to read the specified Excel file.
                {
                    ExcelWorksheet townWorksheet = package.Workbook.Worksheets[0]; // The first page (worksheet) is retrieved.


                    for (int i = 0; i < 30; i++) // Loop through city indexes.
                    {
                        for (int townCount = 1; townCount < 30; townCount++) // Loop through district numbers.
                        {
                            var cellValue = townWorksheet.Cells[townCount + (i * 29), 5].Text; // Read the distance value in each cell.
                            if (i <= townCount - 1) // If the index order is OK, add the distance to the matrix.
                            {
                                townFullArray[i][townCount] = double.Parse(cellValue);
                            }
                            else if (i > townCount - 1)
                            {
                                townFullArray[i][townCount - 1] = double.Parse(cellValue);
                            }
                        }
                    }
                }
            }
            else 
            {
                Console.WriteLine("File Not Found"); // Print file not found error message.
            }

            int[][] jaggedCopyArray = new int[81][];
            int[][] fullCopyArray = new int[81][]; // A duplicate array is created in a full matrix structure of size 81x81.
            double[][] townCopyArray = new double[30][]; // A 30x30 array is created
            cityArray[0] = null; // The first element of the cityArray that will hold the city names is assigned null.
            int[][] cityInfinityFullCopyArray = new int[81][];
            for (int i = 0; i < 81; i++) // Each row of the full copy array points to an array of 81 elements.
            {
                cityInfinityFullCopyArray[i] = new int[81];
            }
            for (int i = 0; i < 81; i++) // Each row of the jagged copy array is assigned an appropriate length.
            {
                jaggedCopyArray[i] = new int[i + 1];
            }
            
            for (int i = 0; i < 81; i++) // Each row of the full copy array points to an array of 81 elements.
            {
                fullCopyArray[i] = new int[81];
            }

            for (int i = 0; i < 30; i++) // For each row of the Town copy array, an array of 30 elements is assigned.
            {
                townCopyArray[i] = new double[30];
            }

            for (int i = 0; i < 81; i++) // Copy JaggedArray to jaggedCopyArray.
            {
                for (int j = 0; j < jaggedCopyArray[i].Length; j++)
                {
                    jaggedCopyArray[i][j] = jaggedArray[i][j];
                }
            }

            for (int i = 0; i < 81; i++) // Copy FullArray to fullCopyArray.
            {
                for (int j = 0; j < fullCopyArray[i].Length; j++)
                {
                    fullCopyArray[i][j] = fullArray[i][j]; 
                }
            }

            for (int i = 0; i < 30; i++) // Copy TownFullArray to townCopyArray.
            {
                for (int j = 0; j < townCopyArray[i].Length; j++)
                {
                    townCopyArray[i][j] = townFullArray[i][j]; // Data from the old district distance matrix is ​​copied.
                }
            }

            int[][] copy = fullArray.Clone() as int[][]; // .Clone() is used to get a full copy of fullArray.
            int[][] cityInfinityFullArray = InfinityRoadDistances(copy, neighbour); // When calculating infinite distances for cities, a new matrix consisting of distances between the district and the city is created.
            for (int i = 0; i < 81; i++) // Copy FullArray to fullCopyArray.
            {
                for (int j = 0; j < cityInfinityFullCopyArray[i].Length; j++)
                {
                    cityInfinityFullCopyArray[i][j] = cityInfinityFullArray[i][j]; 
                }
            }
            int[][] cityDjikstraFullArray = Djikstra(cityInfinityFullArray);// A matrix is created that calculates the shortest paths between cities using the Dijkstra algorithm.
            double[][] townInfinityFullArray = townInfinityRoadDistances(townFullArray, neighbourTowns); // Town distances are calculated and townInfinityFullArray is created.
            double[][] townDjikstraFullArray = doubleDjikstra(townInfinityFullArray); // Shortest paths are calculated by applying Dijstra algorithm for district distances.

            Random10City(fullArray, cityArray);
            //the distance value on the highways table, the value we calculated and the difference between the two for cities

            List<List<string>> cityMinGapList = new List<List<string>>(); //
            List<List<string>> cityMaxGapList = new List<List<string>>();
            int cityminGap = 1000000;
            int citymaxGap = 0;
            for (int i = 0; i < 81; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (i != j && !neighbour[i].Contains(j))
                    {
                        Console.WriteLine(
                            $"While the distance between {cityArray[i + 1]}-{cityArray[j + 1]} is {fullCopyArray[i][j]} km according to the distance table, " +
                            $"the distance between{cityArray[i + 1]}-{cityArray[j + 1]} is {cityDjikstraFullArray[i][j]} km according to the dijikstra table.. " +
                            $"Difference: {Math.Abs(fullCopyArray[i][j] - cityDjikstraFullArray[i][j])} km.");
                        Console.WriteLine();


                        if (Math.Abs(fullCopyArray[i][j] - cityDjikstraFullArray[i][j]) == cityminGap)//shortest distance city(s)
                        {
                            cityMinGapList.Add(new List<string> { cityArray[i + 1], cityArray[j + 1] });
                        }
                        else if (Math.Abs(fullCopyArray[i][j] - cityDjikstraFullArray[i][j]) < cityminGap) //Change of the shortest distance city(s)
                        {
                            cityMinGapList = new List<List<string>>();
                            cityminGap = Math.Abs(fullCopyArray[i][j] - cityDjikstraFullArray[i][j]);
                            cityMinGapList.Add(new List<string> { cityArray[i + 1], cityArray[j + 1] });
                        }

                        if (Math.Abs(fullCopyArray[i][j] - cityDjikstraFullArray[i][j]) == citymaxGap) //longest distance city(s)
                        {
                            cityMaxGapList.Add(new List<string> { cityArray[i + 1], cityArray[j + 1] });
                        }
                        else if (Math.Abs(fullCopyArray[i][j] - cityDjikstraFullArray[i][j]) > citymaxGap)//Change of the longest distance city(s)
                        {
                            cityMaxGapList = new List<List<string>>();
                            citymaxGap = Math.Abs(fullCopyArray[i][j] - cityDjikstraFullArray[i][j]);
                            cityMaxGapList.Add(new List<string> { cityArray[i + 1], cityArray[j + 1] });
                        }
                    }
                }
            }
            //the distance value on the highways table, the value we calculated and the difference between the two for towns
            List<List<string>> townMinGapList = new List<List<string>>();
            List<List<string>> townMaxGapList = new List<List<string>>();
            double townminGap = 1000000;
            double townmaxGap = 0;
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (i != j && neighbourTowns[i][j] != 1)
                    {
                        Console.WriteLine(
                            $"While the distance between {townofizmir[i]}-{townofizmir[j]} is {townCopyArray[i][j]} km according to the distance table, " +
                            $"the distance between{townofizmir[i]}-{townofizmir[j]} is {townDjikstraFullArray[i][j]} km according to the dijikstra table.. " +
                            $"Difference: {Math.Abs(townCopyArray[i][j] - townDjikstraFullArray[i][j])} km.");
                        Console.WriteLine();

                        if (Math.Round(Math.Abs(townCopyArray[i][j] - townDjikstraFullArray[i][j]), 4) == townminGap)//shortest distance town(s)
                        {
                            townMinGapList.Add(new List<string> { townofizmir[i], townofizmir[j] });
                        }
                        else if (Math.Round(Math.Abs(townCopyArray[i][j] - townDjikstraFullArray[i][j]), 4) <
                                 townminGap) //Change of the shortest distance town(s)
                        {
                            townMinGapList = new List<List<string>>();
                            townminGap = (Math.Abs(townCopyArray[i][j] - townDjikstraFullArray[i][j]));
                            townMinGapList.Add(new List<string> { townofizmir[i], townofizmir[j] });
                        }

                        if (Math.Round(Math.Abs(townCopyArray[i][j] - townDjikstraFullArray[i][j]), 4) == townmaxGap) //longest distance town(s)
                        {
                            townMaxGapList.Add(new List<string> { townofizmir[i], townofizmir[j] });
                        }
                        else if (Math.Round(Math.Abs(townCopyArray[i][j] - townDjikstraFullArray[i][j]), 4) >
                                 townmaxGap) //Change of the longest distance town(s)
                        {
                            townMaxGapList = new List<List<string>>();
                            townmaxGap = Math.Abs(townCopyArray[i][j] - townDjikstraFullArray[i][j]);
                            townMaxGapList.Add(new List<string> { townofizmir[i], townofizmir[j] });
                        }
                    }
                }
            }
            Console.WriteLine("Minimum differences between two towns normal distance and Djikstra distance");
            for (int i = 0; i < townMinGapList.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Console.Write(townMinGapList[i][j]); //Print of shortest distance town(s)
                }

                Console.WriteLine();
            }

            Console.WriteLine("Maximum differences between two towns normal distance and Djikstra distance");
            for (int i = 0; i < townMaxGapList.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Console.Write(townMaxGapList[i][j]); //Print of longest distance town(s)
                }

                Console.WriteLine();
            }

            Console.WriteLine("Minimum differences between two cities normal distance and Djikstra distance");
            for (int i = 0; i < cityMinGapList.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Console.Write(cityMinGapList[i][j]); //Print of shortest distance city(s)
                }

                Console.WriteLine();
            }

            Console.WriteLine("Maximum difference between two cities normal distance and Djikstra distance");
            for (int i = 0; i < cityMaxGapList.Count; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Console.Write(cityMaxGapList[i][j]); //Print of longest distance city(s)
                }

                Console.WriteLine();
            }
            for (int i = 0; i < cityInfinityFullCopyArray.Length; i++)
            {
                Console.WriteLine(cityArray[i+1]);
                for (int j = 0; j < cityInfinityFullCopyArray.Length; j++)
                {
                    Console.Write(cityInfinityFullCopyArray[i][j]+" ");
                }
                Console.WriteLine();
            }
            for (int i = 0; i < townCopyArray.Length; i++)
            {
                Console.WriteLine(townofizmir[i]);
                for (int j = 0; j < townCopyArray.Length; j++)
                {
                    Console.Write(townCopyArray[i][j]+" ");
                }
                Console.WriteLine();
            }
        }
    }
}