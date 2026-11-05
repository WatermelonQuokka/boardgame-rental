const string ConnectionVariableName = "BOARDGAME_RENTAL_CONNECTION";

string? connectionString = Environment.GetEnvironmentVariable(ConnectionVariableName);

ConsoleUI ui = new();

if (string.IsNullOrWhiteSpace(connectionString))
{
    ui.ShowMissingConnectionVariable(ConnectionVariableName);
    return;
}

IRentalRepository repository = new MySqlRentalRepository(connectionString);
bool isRunning = true;
while (isRunning)
{
    MenuChoice choice = ui.AskForMenuChoice();
    switch (choice)
    {
        case MenuChoice.ShowGames:
            List<BoardGame> boardGames = await repository.GetActiveGamesAsync();
            ui.ShowActiveGameList(boardGames);
            break;
        case MenuChoice.ShowRentals:
            List<RentalDetails> rentals = await repository.GetAllRentalsAsync();
            ui.ShowRentalList(rentals);
            break;
        case MenuChoice.AddGame:
            string name = "Ticket to Ride: Europe's Edition";
            uint stock = 3;
            AddGameResult addResult = await repository.AddGameAsync(name, stock);
            ui.ShowAddGameResult(addResult, name);
            break;
        case MenuChoice.RetireGame:
            uint id = 2;
            RetireGameResult retireResult = await repository.RetireGameAsync(id);
            ui.ShowRetireGameResult(retireResult);
            break;
        case MenuChoice.Quit:
            isRunning = false;
            break;
        default:
            throw new ArgumentOutOfRangeException(
                nameof(choice), choice, "Unknown choice.");
    }
}