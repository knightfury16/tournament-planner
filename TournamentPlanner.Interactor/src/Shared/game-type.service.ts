import { Injectable } from '@angular/core';
import { GameTypeDto, GameTypeSupported } from '../app/tp-model/TpModel';
import { IScore } from '../GameTypeSuportedScore/IScore';
import { TableTennisScore, TableTennisScoreType } from '../GameTypeSuportedScore/TableTennisScore';
import { trimAllSpace } from './Utility/stringUtility';
import { StringBuilder } from './Utility/StringBuilder';

@Injectable({
  providedIn: 'root'
})
export class GameTypeService {

  constructor() { }

  public getDisplayScore(gameType: GameTypeDto, score: string, flip: boolean = false):string{
    if(gameType.name == trimAllSpace(GameTypeSupported.TableTennis))return this.tableTennisScore(score, flip);
    return "could not map"
  }
  tableTennisScore(score: string, flip = false): string {
    var ttScore: TableTennisScoreType = this.parseScore<TableTennisScoreType>(score);
    if(flip)return `${ttScore.Player2Sets} - ${ttScore.Player1Sets}`
    return `${ttScore.Player1Sets} - ${ttScore.Player2Sets}`
  }

  eightBallPoolScore(score: string, flip: boolean = false): string {
    var eightBallPoolScore: EightBallPoolScoreType = this.parseScore<EightBallPoolScoreType>(score);
    if (flip) return `${eightBallPoolScore.Player2Racks} - ${eightBallPoolScore.Player1Racks}`
    return `${eightBallPoolScore.Player1Racks} - ${eightBallPoolScore.Player2Racks}`
  }

  public getFullDisplayeScore(gameType: GameTypeDto, score: string): string {
    if (gameType.name == trimAllSpace(GameTypeSupported.TableTennis)) return this.tableTennisFullScore(score);
    return "could not map";
  }

  tableTennisFullScore(score: string): string {
    var ttScore: TableTennisScoreType = this.parseScore<TableTennisScoreType>(score);
    var scoreStringBuilder = new StringBuilder();
    ttScore.SetScores.forEach(set => {
      scoreStringBuilder.append(set.Player1Points.toString());
      scoreStringBuilder.append("-");
      scoreStringBuilder.append(set.Player2Points.toString());
      scoreStringBuilder.append(" ");
    });

    return scoreStringBuilder.toString();
  }
  parseScore<T>(score: string): T {
    return JSON.parse(score) as T;
  }
}
