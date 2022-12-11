using System.Text;
using AdventOfCode._Shared;

namespace AdventOfCode.Day7;

public class Day7 : BaseDayWithPuzzleInput
{
    public override int Day => 7;
    public override string DayName => "Day 7: No Space Left On Device";

    private const string RootDirectory = "/";

    private const long TotalAvailableSpace = 70000000;
    private const long SpaceRequired = 30000000;

    public override async Task SolveChallenge1()
    {
        var puzzleInput = await GetPuzzleInput();

        var commands = ParseCommands(puzzleInput);

        var rootDirectory = ReplayCommandsAndGenerateFileSystem(commands);

        Console.WriteLine(rootDirectory);
        Console.WriteLine();

        var directories = GetAllDirectories(rootDirectory);

        var size = directories.Where(x => x.Size <= 100000).Sum(x => x.Size);

        Console.WriteLine($"Challenge 1: Total size of all directories which are at most 100.000: {size}");
    }

    public override async Task SolveChallenge2()
    {
        var puzzleInput = await GetPuzzleInput();

        var commands = ParseCommands(puzzleInput);

        var rootDirectory = ReplayCommandsAndGenerateFileSystem(commands);

        Console.WriteLine(rootDirectory);

        var directories = GetAllDirectories(rootDirectory);

        var usedSize = rootDirectory.Size;

        var freeSpace = TotalAvailableSpace - usedSize; 

        var size = directories.OrderBy(d => d.Size).First(d => freeSpace + d.Size >= SpaceRequired).Size;

        Console.WriteLine($"Challenge 2: The size of the smallest directory to delete to achieve a size of {SpaceRequired} is: {size}");
    }

    private IList<Directory> GetAllDirectories(Directory directory)
    {
        var directories = GetAllDirectoriesRecursive(directory);
        directories.Add(directory);
        return directories;
    }

    private IList<Directory> GetAllDirectoriesRecursive(Directory root)
    {
        var list = root.Directories.ToList();
        var additionalList = new List<Directory>();

        foreach (var dir in list)
        {
            additionalList.AddRange(GetAllDirectoriesRecursive(dir));
        }

        list.AddRange(additionalList);

        return list;
    }

    private Directory ReplayCommandsAndGenerateFileSystem(ICollection<ConsoleCommand> commands)
    {
        var rootDirectory = new Directory(RootDirectory);
        var currentDirectory = rootDirectory;

        foreach (var command in commands)
        {
            if (command.Command == ConsoleCommand.Commands.cd)
            {
                var directoryArgument = command.Arguments![0];
                if (directoryArgument == RootDirectory)
                {
                    currentDirectory = rootDirectory;
                }
                else if (directoryArgument == "..")
                {
                    currentDirectory = currentDirectory.Parent ?? throw new Exception("Can't go to parent directory because it doesn't exist.");
                }
                else
                {
                    var dir = currentDirectory.Directories.FirstOrDefault(x => x.Name == directoryArgument);

                    if (dir is null)
                    {
                        dir = new Directory(directoryArgument)
                        {
                            Parent = currentDirectory
                        };
                        currentDirectory.Directories.Add(dir);

                    }

                    currentDirectory = dir;
                }
            }
            else if (command.Command == ConsoleCommand.Commands.ls)
            {
                var contents = command.Output.Select(s => s.Split()).ToList();

                foreach (var content in contents)
                {
                    if (content[0] == "dir")
                    {
                        if (currentDirectory.Directories.All(x => x.Name != content[1]))
                        {
                            var dir = new Directory(content[1])
                            {
                                Parent = currentDirectory
                            };

                            currentDirectory.Directories.Add(dir);
                        };
                    }
                    else
                    {
                        var file = new File(content[1], long.Parse(content[0]));
                        currentDirectory.Files.Add(file);
                    }
                }
            }
        }

        return rootDirectory;
    }

    private ICollection<ConsoleCommand> ParseCommands(ICollection<string> input)
    {
        return input.Aggregate(new List<ConsoleCommand>(), (aggregate, line) =>
        {
            if (line.StartsWith("$"))
            {
                var split = line.Split();

                var command = new ConsoleCommand(split[1])
                {
                    Arguments = split.Length > 2 ? split[2..] : null
                };

                aggregate.Add(command);
            }
            else // Output line
            {
                var command = aggregate.LastOrDefault();
                if (command is not null) command.Output.Add(line);
            }

            return aggregate;
        });
    }

    private record ConsoleCommand
    {
        public enum Commands
        {
            cd,
            ls
        }

        public ConsoleCommand(string command)
        {
            Command = Enum.TryParse(command, true, out Commands commandEnum)
                ? commandEnum
                : throw new ArgumentException(command, nameof(command));

            Output = new List<string>();
        }

        public Commands Command { get; init; }

        public string[]? Arguments { get; init; }

        public IList<string> Output { get; init; }

        public override string ToString()
        {
            return $"$ {Command.ToString()} {string.Join(" ", Arguments ?? Array.Empty<string>())}";
        }
    }

    private record Directory
    {
        public Directory(string name)
        {
            Name = name;
            Directories = new HashSet<Directory>(new DirectoryComparer());
            Files = new HashSet<File>(new FileComparer()); ;
        }

        public string Name { get; init; }

        public Directory? Parent { get; init; }

        public HashSet<Directory> Directories { get; init; }

        public HashSet<File> Files { get; init; }

        public long Size => Files.Select(f => f.Size).Sum() + Directories.Select(d => d.Size).Sum();

        public override string ToString() => ToString(null, 0);

        private string ToString(StringBuilder? sb = null, int indentCount = 0)
        {
            sb ??= new StringBuilder();

            var indent = string.Empty;

            for (var i = 0; i < indentCount; i++)
            {
                indent += " ";
            }

            ;
            sb.AppendLine($"{indent}- {Name} (dir)");

            foreach (var directory in Directories)
            {
                directory.ToString(sb, indentCount + 2);
            }

            foreach (var file in Files)
            {
                sb.AppendLine($"{indent}- {file}");
            }

            return sb.ToString();
        }

        private class FileComparer : IEqualityComparer<File>
        {
            public bool Equals(File? x, File? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Name == y.Name;
            }

            public int GetHashCode(File obj)
            {
                return HashCode.Combine(obj.Name, obj.Size);
            }
        }

        private class DirectoryComparer : IEqualityComparer<Directory>
        {
            public bool Equals(Directory? x, Directory? y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Name == y.Name;
            }

            public int GetHashCode(Directory obj)
            {
                return HashCode.Combine(obj.Name);
            }
        }
    }

    private record File
    {
        public File(string name, long size)
        {
            Name = name;
            Size = size;
        }

        public string Name { get; init; }

        public long Size { get; init; }

        public override string ToString()
        {
            return $"{Name} (file, size={Size})";
        }
    }
}