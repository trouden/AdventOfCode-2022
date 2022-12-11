using System.Text;

const string FileName = "PuzzleInput.txt";
var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);

var puzzleInput = await File.ReadAllLinesAsync(filePath, Encoding.UTF8);

// Puzzle 1

var elfWithHighestCalories = puzzleInput
    .Aggregate((elfNumber: 0, elfNumberCalories: 0, evaluatingElf: 0, calculatingCalories: 0), (highestCalorieElf, next) =>
{
    if (int.TryParse(next, out var number))
    {
        highestCalorieElf.calculatingCalories += number;
    }
    else
    {
        if (highestCalorieElf.calculatingCalories > highestCalorieElf.elfNumberCalories)
        {
            highestCalorieElf.elfNumber = highestCalorieElf.evaluatingElf;
            highestCalorieElf.elfNumberCalories = highestCalorieElf.calculatingCalories;
        }

        highestCalorieElf.evaluatingElf += 1;
        highestCalorieElf.calculatingCalories = 0;
    }

    return highestCalorieElf;
});

Console.WriteLine($"Elf with the highest calories is elf #{elfWithHighestCalories.elfNumber} (0-based) with calories: {elfWithHighestCalories.elfNumberCalories}.");

// Puzzle 2

var elves = new List<(int, int)>();

puzzleInput.Aggregate((elfNumber: 0, elfCalories: 0), (elf, next) =>
{
    if (int.TryParse(next, out var number))
    {
        elf.elfCalories += number;
    }
    else
    {
        elves.Add(elf);
        elf.elfNumber += 1;
        elf.elfCalories = 0;
    }

    return elf;
});

var totalCaloriesByTop3Elves = elves.OrderByDescending(x => x.Item2).Take(3).Sum(x => x.Item2);

Console.WriteLine($"Total Calories for the highest 3 elves: {totalCaloriesByTop3Elves}.");

Console.ReadLine();