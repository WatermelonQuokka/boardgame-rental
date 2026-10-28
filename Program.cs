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


using MySqlCommand gameListCommand = new("SELECT name, stock FROM boardgames;", connection);

using (MySqlDataReader gameListReader = gameListCommand.ExecuteReader())
{
    bool isThereAnyGame = false;
    while (gameListReader.Read())
    {
        string gameListName = gameListReader.GetString("name");
        uint gameListStock = gameListReader.GetUInt32("stock");
        string copyWord = gameListStock == 1 ? "copy" : "copies";
        Console.WriteLine($"{gameListName}: {gameListStock} {copyWord}");
        isThereAnyGame = true;
    }

    if (!isThereAnyGame)
    {
        Console.WriteLine("No games found.");
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