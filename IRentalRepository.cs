interface IRentalRepository
{
    Task<List<BoardGame>> GetActiveGamesAsync();
    Task<List<RentalDetails>> GetAllRentalsAsync();
    AddGameResult AddGame(string name, uint stock);
    RetireGameResult RetireGame(uint id);
}