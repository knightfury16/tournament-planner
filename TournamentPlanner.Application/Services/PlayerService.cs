using TournamentPlanner.Application.Common;
using TournamentPlanner.Application.Common.Interfaces;
using TournamentPlanner.Domain.Entities;
using TournamentPlanner.Domain.Enum;
using TournamentPlanner.Domain.Exceptions;

namespace TournamentPlanner.Application.Services;

public interface IPlayerService
{
    public Task<GameStatistic> GetGameStatistic(int playerId, GameTypeSupported gameType);
    public Task<GameStatistic> GetGameStatistic(int playerId, string gameType);
    public Task<UpdateGameStatisticResult> UpdateGameStatistic(
        int playerId,
        GameTypeSupported gameType,
        bool isWon
    );
    public Task<(int GamesPlayed, int GamesWon)> GetOverallGameStatistics(int playerId);
}

public class PlayerService : IPlayerService
{
    private readonly IRepository<GameStatistic> _gameStatRepository;
    private readonly IRepository<Player> _playerRepository;
    private readonly IRepository<GameType> _gameTypeRepository;

    public PlayerService(
        IRepository<GameStatistic> gameStatRepository,
        IRepository<Player> playerRepository,
        IRepository<GameType> gameTypeRepository
    )
    {
        _gameStatRepository = gameStatRepository;
        _playerRepository = playerRepository;
        _gameTypeRepository = gameTypeRepository;
    }

    public async Task<GameStatistic> GetGameStatistic(int playerId, GameTypeSupported gameType)
    {
        var gameStat = await _gameStatRepository.GetAllAsync(gs =>
            gs.PlayerId == playerId && gs.GameType.Name == gameType
        );

        //if game stat not available then create a new game stat
        if (gameStat == null)
        {
            //this player did not play this game type before
            return await CreateNewGameStat(playerId, gameType);
        }

        //if game stat for this specific game type is more than one throw error
        if (gameStat.Count() > 1)
            throw new ValidationException($"More than one Game Stat found of game type {gameType}");

        return gameStat.First();
    }

    private async Task<GameStatistic> CreateNewGameStat(
        int playerId,
        GameTypeSupported gameTypeSupported
    )
    {
        var player = await GetPlayer(playerId);
        var gameType = await GetGameType(gameTypeSupported);

        var gameStat = new GameStatistic { Player = player, GameType = gameType };

        await _gameStatRepository.SaveAsync();

        return gameStat;
    }

    private async Task<GameType> GetGameType(GameTypeSupported gameTypeSupported)
    {
        ArgumentNullException.ThrowIfNull(gameTypeSupported);

        var gameType = await _gameTypeRepository.GetAllAsync(gt => gt.Name == gameTypeSupported);

        if (gameType == null || gameType.Count() == 0)
            throw new NotFoundException(nameof(gameTypeSupported));

        return gameType.First();
    }

    private async Task<Player> GetPlayer(int playerId)
    {
        var player = await _playerRepository.GetByIdAsync(playerId);
        if (player == null)
            throw new NotFoundException(nameof(player));
        return player;
    }

    public async Task<GameStatistic> GetGameStatistic(int playerId, string gameType)
    {
        var gameTypeSupported = GetGameTypeSupported(gameType);
        return await GetGameStatistic(playerId, gameTypeSupported);
    }

    public GameTypeSupported GetGameTypeSupported(string gameType)
    {
        var trimString = GetTrimmedString(gameType);

        if (!Enum.TryParse(trimString, out GameTypeSupported gameTypeSupported))
        {
            throw new ValidationException("Could not parse Game Type");
        }

        return gameTypeSupported;
    }

    private string GetTrimmedString(string stringValue)
    {
        stringValue = stringValue.Replace(" ", "");
        return stringValue.Trim();
    }

    public async Task<UpdateGameStatisticResult> UpdateGameStatistic(
        int playerId,
        GameTypeSupported gameType,
        bool isWon
    )
    {
        var gameStat = await GetGameStatistic(playerId, gameType);

        if (gameStat == null)
            return UpdateGameStatisticResult.Failed("Could not find game stat of the player");

        if (!ValidateGameStat(gameStat.GamePlayed, gameStat.GameWon))
            return UpdateGameStatisticResult.Failed(
                "Game stat validation failed. Game won > Game played"
            );

        await UpdateStat(gameStat, isWon);

        return UpdateGameStatisticResult.Succeeded();
    }

    private async Task UpdateStat(GameStatistic gameStat, bool isWon)
    {
        gameStat.GamePlayed++;
        if (isWon)
            gameStat.GameWon++;
        await _gameStatRepository.SaveAsync();
    }

    private bool ValidateGameStat(int gamePlayed, int gameWon)
    {
        if (gameWon > gamePlayed)
            return false;

        return true;
    }

    public async Task<(int GamesPlayed, int GamesWon)> GetOverallGameStatistics(int playerId)
    {
        var gameStat = await _gameStatRepository.GetAllAsync(gs => gs.PlayerId == playerId);

        if (gameStat == null)
            throw new NotFoundException(nameof(GameStatistic));

        return (gameStat.Sum(gt => gt.GamePlayed), gameStat.Sum(gt => gt.GameWon));
    }
}
