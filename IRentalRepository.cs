interface IRentalRepository
{
    Task<List<BoardGame>> GetActiveGamesAsync();
    Task<List<RentalDetails>> GetAllRentalsAsync();
    Task<AddGameResult> AddGameAsync(string name, uint stock);
    RetireGameResult RetireGame(uint id);
}