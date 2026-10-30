interface IRentalRepository
{
    List<BoardGame> GetActiveGames();
    List<RentalDetails> GetAllRentals();
    AddGameResult AddGame(string name, uint stock);
    RetireGameResult RetireGame(uint id);
}