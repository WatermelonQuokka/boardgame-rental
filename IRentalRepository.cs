interface IRentalRepository
{
    Task<List<BoardGame>> GetActiveGamesAsync();
    List<RentalDetails> GetAllRentals();
    AddGameResult AddGame(string name, uint stock);
    RetireGameResult RetireGame(uint id);
}