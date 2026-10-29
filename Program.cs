using MySqlConnector;

const string ConnectionVariableName = "BOARDGAME_RENTAL_CONNECTION";

string? connectionString = Environment.GetEnvironmentVariable(ConnectionVariableName);

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine($"{ConnectionVariableName} could not be found or is invalid.");
    return;
}

using MySqlConnection connection = new(connectionString);
connection.Open();

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

using MySqlCommand rentalListCommand = new(
    """
    SELECT
        members.name AS member_name,
        boardgames.name AS boardgame_name,
        rentals.rental_start,
        rentals.rental_end
    FROM rentals
        JOIN boardgames ON rentals.game_id = boardgames.id
        JOIN members ON rentals.member_id = members.id
    ORDER BY rentals.rental_end
    """,
    connection
);

using (MySqlDataReader rentalListReader = rentalListCommand.ExecuteReader())
{
    while (rentalListReader.Read())
    {
        string memberName = rentalListReader.GetString("member_name");
        string boardGameName = rentalListReader.GetString("boardgame_name");
        DateOnly rentStart = rentalListReader.GetDateOnly("rental_start");
        DateOnly rentEnd = rentalListReader.GetDateOnly("rental_end");
        Console.WriteLine($"{memberName} rented {boardGameName}, {rentStart} to {rentEnd}");
    }
}

using MySqlCommand addGameCommand = new("INSERT INTO boardgames (name, stock) VALUES (@name, @stock);", connection);
string name = "Ticket to Ride: Europe's Edition";
uint stock = 3;
addGameCommand.Parameters.AddWithValue("@name", name);
addGameCommand.Parameters.AddWithValue("@stock", stock);
try
{
    addGameCommand.ExecuteNonQuery();
    Console.WriteLine($"{name} has been added in the list!");
}
catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
{
    Console.WriteLine($"{name} is already in the list.");
}

using MySqlCommand retireGameCommand = new("UPDATE boardgames SET is_retired = true WHERE id = @id;", connection);
uint id = 2;
retireGameCommand.Parameters.AddWithValue("@id", id);
int rowsAffected = retireGameCommand.ExecuteNonQuery();
string successOrFailMessage = rowsAffected == 1 ? "The chosen game is retired." : "The game does not exist.";
Console.WriteLine(successOrFailMessage);