using MySqlConnector;
class RentalDatabase
{
    private readonly string _connectionString;

    public RentalDatabase(string connectionString)
    {
        _connectionString = connectionString;
    }

    public List<BoardGame> GetActiveGames()
    {
        List<BoardGame> boardGames = new();

        using MySqlConnection connection = new(_connectionString);
        connection.Open();

        using MySqlCommand gameListCommand = new("SELECT id, name, stock FROM boardgames WHERE is_retired = false;", connection);
        using (MySqlDataReader gameListReader = gameListCommand.ExecuteReader())
        {
            while (gameListReader.Read())
            {
                BoardGame boardGame = new(
                    gameListReader.GetUInt32("id"),
                    gameListReader.GetString("name"),
                    gameListReader.GetUInt32("stock"));
                boardGames.Add(boardGame);
            }
        }
        return boardGames;
    }
}