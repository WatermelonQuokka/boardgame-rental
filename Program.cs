const string ConnectionVariableName = "BOARDGAME_RENTAL_CONNECTION";

string? connectionString = Environment.GetEnvironmentVariable(ConnectionVariableName);

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine($"{ConnectionVariableName} could not be found or is invalid.");
    return;
}

RentalDatabase rentalDatabase = new(connectionString);
List<BoardGame> boardGames = rentalDatabase.GetActiveGames();


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

List<RentalDetails> rentals = rentalDatabase.GetAllRentals();
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

string name = "Ticket to Ride: Europe's Edition";
uint stock = 3;
AddGameResult addResult = rentalDatabase.AddGame(name, stock);
string addGameMessage = addResult == AddGameResult.Success
    ? $"\"{name}\" has been added to the game list."
    : $"\"{name}\" already exists in the game list.";
Console.WriteLine(addGameMessage);

uint id = 2;
RetireGameResult retireResult = rentalDatabase.RetireGame(id);
string retireGameMessage = retireResult == RetireGameResult.Success
    ? "The chosen game is retired."
    : "The game could not be found.";
Console.WriteLine(retireGameMessage);