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

    public List<RentalDetails> GetAllRentals()
    {
        List<RentalDetails> rentals = new();

        using MySqlConnection connection = new(_connectionString);
        connection.Open();

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
            connection);

        using (MySqlDataReader rentalListReader = rentalListCommand.ExecuteReader())
        {
            while (rentalListReader.Read())
            {
                RentalDetails rental = new(
                    rentalListReader.GetString("member_name"),
                    rentalListReader.GetString("boardgame_name"),
                    rentalListReader.GetDateOnly("rental_start"),
                    rentalListReader.GetDateOnly("rental_end"));
                rentals.Add(rental);
            }
        }
        return rentals;
    }

    public AddGameResult AddGame(string name, uint stock)
    {
        using MySqlConnection connection = new(_connectionString);
        connection.Open();

        using MySqlCommand addGameCommand = new("INSERT INTO boardgames (name, stock) VALUES (@name, @stock);", connection);
        addGameCommand.Parameters.AddWithValue("@name", name);
        addGameCommand.Parameters.AddWithValue("@stock", stock);
        
        try
        {
            addGameCommand.ExecuteNonQuery();
            return AddGameResult.Success;
        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
        {
            return AddGameResult.AlreadyExists;
        }
    }
}