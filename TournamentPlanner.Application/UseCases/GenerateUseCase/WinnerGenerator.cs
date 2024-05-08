using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TournamentPlanner.Domain.Entities;

namespace TournamentPlanner.Application.UseCases.GenerateUseCase
{
    public class WinnerGenerator
    {
        private static Random random = new Random();

        public static List<Match> MakeAllMatchRadomWinner(List<Match> matches)
        {
            //is Player fiels populated
            if(matches[0].FirstPlayer == null){
                throw new Exception("Players of the Matches can not be empty.");
            }

            for (var i = 0; i < matches.Count; i++)
            {
                if (matches[i].IsComplete == false)
                {
                    var winnerPlayer = random.Next(0, 2) == 0 ? matches[i].FirstPlayer : matches[i].SecondPlayer;
                    matches[i].IsComplete = true;
                    matches[i].Winner = winnerPlayer;
                }
            }
            return matches;
        }

        public static List<Match> MakeSomeMatchRandomWinner(List<Match> matches, int? matchIndex)
        {
            //is Player fiels populated
            if(matches[0].FirstPlayer == null){
                throw new Exception("Players of the Matches can not be empty.");
            }

            var matchNumber = matches.Count();
            var randomMatchWinnerNumber = matchIndex.HasValue ? matchIndex : random.Next(1, matchNumber);

            for (var i = 0; i < randomMatchWinnerNumber; i++)
            {
                var randomMatchIndex = random.Next(0, matchNumber);
                if (matches[randomMatchIndex].IsComplete == false)
                {
                    var winnerPlayer = random.Next(0, 2) == 0 ? matches[randomMatchIndex].FirstPlayer : matches[randomMatchIndex].SecondPlayer;
                    matches[randomMatchIndex].IsComplete = true;
                    matches[randomMatchIndex].Winner = winnerPlayer;
                }
            }
            return matches;
        }
    }
}