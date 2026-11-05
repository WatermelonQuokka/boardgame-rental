using MySqlConnector;

class MySqlRentalRepository : IRentalRepository
{
    private readonly string _connectionString;

    public MySqlRentalRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    private async Task<MySqlConnection> OpenConnectionAsync()
    {
        MySqlConnection connection = new(_connectionString);
        await connection.OpenAsync();
        return connection;
    }

    public async Task<List<BoardGame>> GetActiveGamesAsync()
    {
        List<BoardGame> boardGames = new();

        await using MySqlConnection connection = await OpenConnectionAsync();

        await using MySqlCommand gameListCommand = new("SELECT id, name, stock FROM boardgames WHERE is_retired = false;", connection);
        await using (MySqlDataReader gameListReader = await gameListCommand.ExecuteReaderAsync())
        {
            while (await gameListReader.ReadAsync())
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

    public async Task<List<RentalDetails>> GetAllRentalsAsync()
    {
        List<RentalDetails> rentals = new();

        await using MySqlConnection connection = await OpenConnectionAsync();

        await using MySqlCommand rentalListCommand = new(
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

        await using (MySqlDataReader rentalListReader = await rentalListCommand.ExecuteReaderAsync())
        {
            while (await rentalListReader.ReadAsync())
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

    public async Task<AddGameResult> AddGameAsync(string name, uint stock)
    {
        await using MySqlConnection connection = await OpenConnectionAsync();

        await using MySqlCommand addGameCommand = new("INSERT INTO boardgames (name, stock) VALUES (@name, @stock);", connection);
        addGameCommand.Parameters.AddWithValue("@name", name);
        addGameCommand.Parameters.AddWithValue("@stock", stock);

        try
        {
            await addGameCommand.ExecuteNonQueryAsync();
            return AddGameResult.Success;
        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
        {
            return AddGameResult.AlreadyExists;
        }
    }

    public async Task<RetireGameResult> RetireGameAsync(uint id)
    {
        await using MySqlConnection connection = await OpenConnectionAsync();

        await using MySqlCommand retireGameCommand = new("UPDATE boardgames SET is_retired = true WHERE id = @id;", connection);
        retireGameCommand.Parameters.AddWithValue("@id", id);
        int rowsAffected = await retireGameCommand.ExecuteNonQueryAsync();
        return rowsAffected == 1
            ? RetireGameResult.Success
            : RetireGameResult.NotFound;
    }
}