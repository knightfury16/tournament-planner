export interface TournamentDto {
  id: number;
  name: string;
  startDate?: string | null;
  endDate?: string | null;
  registrationLastDate?: string | null;
  venue?: string | null;
  registrationFee: number;
  minimumAgeOfRegistration: number;
  status?: string | null;
  gameTypeDto?: GameTypeDto | null;
  createdAt: string;
  updateAt: string;

}


export enum TournamentStatus {
  Draft,
  RegistrationOpen,
  RegistrationClosed,
  Ongoing,
  Completed
}
export enum TournamentType {
  GroupStage,
  Knockout
}

export enum ResolutionStrategy {
  StatBased,
  Random,
  KnockoutQualifier
}

export enum GameTypeSupported {
  TableTennis,
  EightBallPool
}


export interface GameTypeDto {
  name: string | null;
}

export interface PlayerDto {
  id: number;
  name: string;
  age?: number | null;
  gamePlayed?: number | null;
  gameWon?: number | null;
  winRatio?: number | null;
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
  startDate: string;
  endDate: string;
  gameType: GameTypeSupported | null;
  status?: TournamentStatus;
  registrationLastDate?: Date;
  maxParticipant?: number;
  venue?: string;
  registrationFee?: number;
  minimumAgeOfRegistration?: number;
  winnerPerGroup?: number;
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
