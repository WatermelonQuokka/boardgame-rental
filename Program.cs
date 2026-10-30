const string ConnectionVariableName = "BOARDGAME_RENTAL_CONNECTION";

string? connectionString = Environment.GetEnvironmentVariable(ConnectionVariableName);

ConsoleUI ui = new();

if (string.IsNullOrWhiteSpace(connectionString))
{
    ui.ShowMissingConnectionVariable(ConnectionVariableName);
    return;
}

IRentalRepository repository = new MySqlRentalRepository(connectionString);
List<BoardGame> boardGames = repository.GetActiveGames();
ui.ShowActiveGameList(boardGames);

List<RentalDetails> rentals = repository.GetAllRentals();
ui.ShowRentalList(rentals);

string name = "Ticket to Ride: Europe's Edition";
uint stock = 3;
AddGameResult addResult = repository.AddGame(name, stock);
ui.ShowAddGameResult(addResult, name);

uint id = 2;
RetireGameResult retireResult = repository.RetireGame(id);
ui.ShowRetireGameResult(retireResult);