using MySqlConnector;

const string ConnectionVariableName = "BOARDGAME_RENTAL_CONNECTION";

string? connectionString = Environment.GetEnvironmentVariable(ConnectionVariableName);

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine($"{ConnectionVariableName} could not be found or is invalid.");
    return;
}

using MySqlConnection connection = new MySqlConnection(connectionString);
connection.Open();

Console.WriteLine($"{connection.State}");

using MySqlCommand command = new MySqlCommand("SELECT name FROM boardgames;", connection);

using MySqlDataReader reader = command.ExecuteReader();
bool isThereAnyGame = false;
while (reader.Read())
{
    string name = reader.GetString("name");
    Console.WriteLine(name);
    isThereAnyGame = true;
}

if (!isThereAnyGame)
{
    Console.WriteLine("No games found.");
}