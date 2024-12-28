export interface TournamentDto {
  id: number;
  name: string | undefined;
  startDate?: string | null;
  endDate?: string | null;
  registrationLastDate?: string | null;
  participants?: PlayerDto[];
  venue?: string | null;
  registrationFee: number;
  minimumAgeOfRegistration: number;
  maxParticipants: number;
  status?: string | null;
  gameTypeDto?: GameTypeDto | null;
  createdAt?: string | null;
  updateAt?: string | null;

}


export enum TournamentStatus {
  Draft = "Draft",
  RegistrationOpen = "Registration Open",
  RegistrationClosed = "Registration Closed",
  Ongoing = "Ongoing",
  Completed = "Completed"
}
export enum TournamentType {
  GroupStage = "Group Stage",
  Knockout = "Knockout"
}

export enum ResolutionStrategy {
  StatBased,
  Random,
  KnockoutQualifier
}

export enum GameTypeSupported {
  TableTennis = 'Table Tennis',
  EightBallPool = 'Eight Ball Pool'
}


export interface GameTypeDto {
  name: string | null;
}

export interface PlayerDto {
  id: number;
  name: string;
  email: string;
  age?: number | null;
  gamePlayed?: number | null;
  gameWon?: number | null;
  winRatio?: number | null;
}

export interface AdminDto {
  id: number;
  name: string;
  email: string;
  phoneNumber: string;
  createdTournament?: TournamentDto[];
}

export interface RoundDto {
  id: number;
  roundName: string | null;
  roundNumber: number;
  startTime?: string | null;
  matches: MatchDto[];
  isCompleted: boolean;
  createdAt: string;
  updatedAt: string;
}

export interface MatchDto {
  id: number;
  firstPlayer: PlayerDto;
  secondPlayer: PlayerDto;
  winner?: PlayerDto | null;
  roundId: number;
  duration: string;
  gameScheduled?: string | null;
  gamePlayed?: string | null;
  scoreJson?: string | null;
  isRescheduled: boolean;
  rescheduleReason?: string | null;
  courtName?: string | null;
}

export interface AddTournamentDto {
  name: string;
  startDate: string | null; //iso string
  endDate: string | null; //iso string
  gameType: GameTypeDto | null;
  status?: TournamentStatus;
  registrationLastDate?: string; //iso string
  maxParticipant?: number;
  venue?: string;
  registrationFee?: number;
  minimumAgeOfRegistration?: number;
  winnerPerGroup?: number; // will lock this field and set it to 2 by force
  knockOutStartNumber?: number;
  participantResolutionStrategy?: ResolutionStrategy;
  tournamentType?: TournamentType;
}

export type AddPlayerDto =
  {
    name: string,
    email: string,
    password: string,
    age?: number,
    weight?: number
  }

export type AddAdminDto =
  {
    name: string,
    email: string,
    password: string,
    phoneNumber: string
  }

export type LoginDto =
  {
    email: string,
    password: string
  }

export enum DomainRole {
  Admin = "Admin",
  Player = "Player"
}

export type TournamentStatusChangeDto =
  {
    tournamentStatus: string;
  }
export interface MatchTypeDto {
  id: number;
  name: string | undefined;
  rounds: RoundDto[];
  players: PlayerDto[];
  isCompleted: boolean;
}

export type DrawDto = {
  createdAt: string | undefined;
  updatedAt: string | undefined;
  tournament: TournamentDto;
  matchType: MatchTypeDto;
}


export type PlayerStandingDto = {
  player: PlayerDto;
  matchPoints: number;
  wins: number;
  losses: number;
  gamesWon: number;
  gamesLost: number;
  gameDifference: number;
  pointsWon: number;
  pointsLost: number;
  pointsDifference: number;
  ranking: number;
}