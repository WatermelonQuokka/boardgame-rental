class ConsoleUI
{
    public void ShowActiveGameList(List<BoardGame> boardGames)
    {
        if (boardGames.Count == 0)
        {
            Console.WriteLine("No games found.");
        }
        else
        {
            foreach (BoardGame boardGame in boardGames)
            {
                string copyWord = boardGame.Stock == 1 ? "copy" : "copies";
                Console.WriteLine($"{boardGame.Name}: {boardGame.Stock} {copyWord}");
            }
        }
    }

    public void ShowRentalList(List<RentalDetails> rentals)
    {
        if (rentals.Count == 0)
        {
            Console.WriteLine("No rentals found.");
        }
        else
        {
            foreach (RentalDetails rental in rentals)
            {
                Console.WriteLine($"{rental.MemberName} rented {rental.BoardGameName}, {rental.RentalStart} to {rental.RentalEnd}");
            }
        }
    }

    public void ShowAddGameResult(AddGameResult result, string name)
    {
        string addGameMessage = result == AddGameResult.Success
            ? $"\"{name}\" has been added to the game list."
            : $"\"{name}\" already exists in the game list.";
        Console.WriteLine(addGameMessage);
    }

    public void ShowRetireGameResult(RetireGameResult result)
    {
        string retireGameMessage = result == RetireGameResult.Success
            ? "The chosen game is retired."
            : "The game could not be found.";
        Console.WriteLine(retireGameMessage);
    }
}