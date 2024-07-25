export interface TournamentDto {
  name: string;
  startDate?: string | null;
  endDate?: string | null;
}
interface Tournament {
  name: string;
  startDate: string | null;
  endDate: string | null;
  id: number;
  createdAt: string;
  updatedAt: string;
}
export interface Player {
  id: number;
  name: string;
  phoneNumber: string;
  email: string;
  tournament: Tournament;
  tournamentId: number;
  createdAt: string;
  updatedAt: string;
}

export interface Round {
  id: number;
  roundNumber: number;
  startTime: string;
  tournament: Tournament;
  tournamentId: number;
  createdAt: string;
  updatedAt: string;
}

interface Match {
  id: number;
  firstPlayer: Player;
  secondPlayer: Player;
  isComplete: boolean;
  winner: string | null;
  gameScheduled: string;
  gamePlayed: string | null;
  round: Round;
  roundId: number;
  createdAt: string;
  updatedAt: string;
}
