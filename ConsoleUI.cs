class ConsoleUI
{
    public void ShowActiveGameList(List<BoardGame> boardGames)
    {
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
    }
}