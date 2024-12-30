import { IScore } from './IScore';

//!! REMEMBER to match the casing of the DTO or else cant parse

export interface SetScore {
  // Define the properties of SetScore based on your requirements
  Player1Points: number;
  Player2Points: number;
}

//!! Experimental
export class TableTennisScore implements IScore {
  public Player1Sets: number = 0;
  public Player2Sets: number = 0;
  public SetsToWin: number = 3; // default 3
  public PointsPerSet: number = 11; // default 11
  public SetScores: SetScore[] = [];
  public IsComplete: boolean = false;
}

export type TableTennisScoreType = {
  Player1Sets: number;
  Player2Sets: number;
  SetsToWin: number; // default 3
  PointsPerSet: number; // default 11
  SetScores: SetScore[];
  IsComplete?: boolean;
};
