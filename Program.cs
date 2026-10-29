const string ConnectionVariableName = "BOARDGAME_RENTAL_CONNECTION";

string? connectionString = Environment.GetEnvironmentVariable(ConnectionVariableName);

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine($"{ConnectionVariableName} could not be found or is invalid.");
    return;
}

ConsoleUI ui = new();
RentalDatabase rentalDatabase = new(connectionString);
List<BoardGame> boardGames = rentalDatabase.GetActiveGames();
ui.ShowActiveGameList(boardGames);


List<RentalDetails> rentals = rentalDatabase.GetAllRentals();
ui.ShowRentalList(rentals);

string name = "Ticket to Ride: Europe's Edition";
uint stock = 3;
AddGameResult addResult = rentalDatabase.AddGame(name, stock);
ui.ShowAddGameResult(addResult, name);

uint id = 2;
RetireGameResult retireResult = rentalDatabase.RetireGame(id);
ui.ShowRetireGameResult(retireResult);