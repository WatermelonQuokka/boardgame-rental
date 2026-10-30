using System.Xml;

class ConsoleUI
{
    private string? _lastMessage;
    private const int GameNameLimit = 64;

    public void ShowMissingConnectionVariable(string variableName)
    {
        Console.WriteLine($"{variableName} could not be found or is invalid.");
    }

    public void ShowActiveGameList(List<BoardGame> boardGames)
    {
        if (boardGames.Count == 0)
        {
            Console.WriteLine("No games found.");
        }
        else
        {
            Console.WriteLine("=== Active Games ===");
            foreach (BoardGame boardGame in boardGames)
            {
                string copyWord = boardGame.Stock == 1 ? "copy" : "copies";
                Console.WriteLine($"{boardGame.Name}: {boardGame.Stock} {copyWord}");
            }
        }
        PauseBeforeMenu();
    }

    public void ShowRentalList(List<RentalDetails> rentals)
    {
        if (rentals.Count == 0)
        {
            Console.WriteLine("No rentals found.");
        }
        else
        {
            Console.WriteLine("=== Rental List ===");
            foreach (RentalDetails rental in rentals)
            {
                Console.WriteLine($"{rental.MemberName} rented {rental.BoardGameName}, {rental.RentalStart} to {rental.RentalEnd}");
            }
        }
        PauseBeforeMenu();
    }

    public void ShowAddGameResult(AddGameResult result, string name)
    {
        string addGameMessage = result == AddGameResult.Success
            ? $"\"{name}\" has been added to the game list."
            : $"\"{name}\" already exists in the game list.";
        _lastMessage = addGameMessage;
    }

    public void ShowRetireGameResult(RetireGameResult result)
    {
        string retireGameMessage = result == RetireGameResult.Success
            ? "The chosen game is retired."
            : "The game could not be found.";
        _lastMessage = retireGameMessage;
    }

    public MenuChoice AskForMenuChoice()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(_lastMessage);
            _lastMessage = null;
            ShowMenu();
            Console.Write("Choose an option: ");
            string input = ReadInput();
            if (int.TryParse(input, out int number)
                && Enum.IsDefined(typeof(MenuChoice), number))
            {
                return (MenuChoice)number;
            }
            else
            {
                _lastMessage = "Invalid option choice.";
            }
        }
    }

    private void ShowMenu()
    {
        Console.WriteLine(
            """

            === Board Game Rental ===
            1. Show games
            2. Show rentals
            3. Add a game
            4. Retire a game
            0. Quit
            """);
    }

    private void PauseBeforeMenu()
    {
        Console.WriteLine("Press any key to return to the menu.");
        Console.ReadKey(true);
    }

    public string AskForGameName()
    {
        while (true)
        {
            Console.Write("Game name: ");
            string input = ReadInput();
            if (input.Length == 0)
            {
                Console.WriteLine("Please enter a name.");
            }
            else if (input.Length > GameNameLimit)
            {
                Console.WriteLine($"The name is {input.Length} characters long. The maximum is {GameNameLimit}.");
            }
            else
            {
                return input;
            }
        }
    }

    private string ReadInput()
    {
        string? input = Console.ReadLine();
        if (input == null)
        {
            throw new EndOfStreamException("The input could not be read.");
        }
        return input.Trim();
    }
}