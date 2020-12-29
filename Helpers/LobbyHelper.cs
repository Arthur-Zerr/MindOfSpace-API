using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Concurrent;
using System.Linq;
using MindOfSpace_Api.Dtos;
using MindOfSpace_Api.Models;
using System.Collections.Generic;
using MindOfSpace_Api.Enums;

namespace MindOfSpace_Api.Helpers
{
    public class LobbyHelper
    {
        private static ConcurrentDictionary<string, GameDto> _gameLobbys = new ConcurrentDictionary<string, GameDto>();
        private static Random _random = new Random();

        public ConcurrentDictionary<string, GameDto> GameLobbys => _gameLobbys;

        public Random Random => _random;

        public string CreateGame(Player hostPlayer)
        {
            var gameDto = new GameDto();
            gameDto.PlayerHost = hostPlayer.ToPlayerForGameDto();
            gameDto.Id = GenerateGameId();
            gameDto.PlayerList = new List<PlayerForGameDto>();
            gameDto.PlayerList.Add(hostPlayer.ToPlayerForGameDto());

            if(GameLobbys.TryAdd(gameDto.Id, gameDto))
                return gameDto.Id;
            else
                return string.Empty;
        }

        public Tuple<bool, GameState> AddPlayerToGame(string gameId, Player joinPlayer)
        {
            GameDto game;
            GameDto originalGame;
            if(!GameLobbys.TryGetValue(gameId, out game))
                return new Tuple<bool, GameState>(false, GameState.Closed);

            originalGame = game;

            if(game.GameState == GameState.Started)
                return new Tuple<bool, GameState>(false, game.GameState);
            var playerInGame = game.PlayerList?.Where(x => x.Username == joinPlayer.UserName).ToList().Count > 0;
            if(playerInGame)
                return new Tuple<bool, GameState>(false, GameState.Closed);
            if(game.PlayerList == null)
                game.PlayerList = new List<PlayerForGameDto>();

            game.PlayerList.Add(joinPlayer.ToPlayerForGameDto());

            return new Tuple<bool, GameState>(GameLobbys.TryUpdate(gameId, game, originalGame), game.GameState);
        }

        public Tuple<bool, GameState> RemovePlayerFromGame(string gameId, Player leavePlayer)
        {
            GameDto game;
            GameDto originalGame;
            if(!GameLobbys.TryGetValue(gameId, out game))
                return new Tuple<bool, GameState>(false, GameState.Closed);    
            originalGame = game;
            var player = leavePlayer.ToPlayerForGameDto();
            if(game.PlayerHost.Username == player.Username)
            {
                game.GameState = GameState.Closed;
                return new Tuple<bool, GameState>(GameLobbys.TryUpdate(gameId, game, originalGame), game.GameState);
            }

            var playerInGame = game.PlayerList?.Where(x => x.Username == leavePlayer.UserName).ToList().Count > 0;
            if(playerInGame)
            {
                var playerToRemove = game.PlayerList.Where(x => x.Username == leavePlayer.UserName).FirstOrDefault();
                game.PlayerList.Remove(playerToRemove);
                return new Tuple<bool, GameState>(GameLobbys.TryUpdate(gameId, game, originalGame), game.GameState);
            }
            
            return new Tuple<bool, GameState>(false, GameState.Closed);    
        }

        public Tuple<bool, GameDto> UpdateGameState(string gameId, GameState gameState)
        {
            GameDto game;
            GameDto originalGame;
            if(!GameLobbys.TryGetValue(gameId, out game))
                return new Tuple<bool, GameDto>(false, null);

            originalGame = game;

            if(game.GameState == gameState)
                return new Tuple<bool, GameDto>(true, game);

            game.GameState = gameState;
            return new Tuple<bool, GameDto>(GameLobbys.TryUpdate(gameId, game, originalGame), game);
        }

        public Tuple<bool, GameDto> UpdatePlayerState(Player player, string gameId, PlayerState playerState)
        {
            GameDto game;
            GameDto originalGame;
            if(!GameLobbys.TryGetValue(gameId, out game))
                return new Tuple<bool, GameDto>(false, null);

            originalGame = game;
            
            if(game.PlayerHost.Username == player.UserName)
            {
                game.PlayerHost.PlayerState = playerState;
                return new Tuple<bool, GameDto>(GameLobbys.TryUpdate(gameId, game, originalGame), game);
            }

            var user = game.PlayerList.Where(x => x.Username == player.UserName).FirstOrDefault();

            if(user != null)
            {
                game.PlayerList.FirstOrDefault(x => x.Username == player.UserName).PlayerState = playerState;
            }
            
            return new Tuple<bool, GameDto>(GameLobbys.TryUpdate(gameId, game, originalGame), game);
        }

        public List<PlayerForGameDto> PlayerListFromGame(string gameId)
        {
            GameDto game;
            if(!GameLobbys.TryGetValue(gameId, out game))
                return new List<PlayerForGameDto>();

            return game.PlayerList;
        }

        public GameDto GetGameDto(string gameId) 
        {
            GameDto game;
            if(!GameLobbys.TryGetValue(gameId, out game))
                return  null;
            
            return game;
        }

        private string GenerateGameId()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var gameId =  new string(Enumerable.Repeat(chars, 5).Select(s => s[Random.Next(s.Length)]).ToArray());
            var existId = GameLobbys.Where(x => x.Key == gameId).ToList();

            if(existId.Count > 0)
                return GenerateGameId();
            else
                return gameId;
        }

    }
}