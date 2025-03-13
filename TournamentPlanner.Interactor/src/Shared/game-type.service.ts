import { Injectable } from '@angular/core';
import { GameTypeDto, GameTypeSupported } from '../app/tp-model/TpModel';
import { TableTennisScore, TableTennisScoreType } from '../GameTypeSuportedScore/TableTennisScore';
import { trimAllSpace } from './Utility/stringUtility';
import { StringBuilder } from './Utility/StringBuilder';
import { EightBallPoolScoreType } from '../GameTypeSuportedScore/EightBallPoolScore';

@Injectable({
  providedIn: 'root'
})
export class GameTypeService {

  constructor() { }

  public getDisplayScore(gameType: GameTypeDto, score: string, flip: boolean = false): string {
    if (gameType.name == trimAllSpace(GameTypeSupported.TableTennis)) return this.tableTennisScore(score, flip);
    if (gameType.name == trimAllSpace(GameTypeSupported.EightBallPool)) return this.eightBallPoolScore(score, flip);
    return "could not map score"
  }

  public getFullDisplayeScore(gameType: GameTypeDto, score: string): string {
    if (gameType.name == trimAllSpace(GameTypeSupported.TableTennis)) return this.tableTennisFullScore(score);
    if (gameType.name == trimAllSpace(GameTypeSupported.EightBallPool)) return this.eightBallPoolScore(score);
    return "could not map full score";
  }

  tableTennisScore(score: string, flip = false): string {
    var ttScore: TableTennisScoreType = this.parseScore<TableTennisScoreType>(score);
    if (flip) return `${ttScore.Player2Sets} - ${ttScore.Player1Sets}`
    return `${ttScore.Player1Sets} - ${ttScore.Player2Sets}`
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

  eightBallPoolScore(score: string, flip: boolean = false): string {
    var eightBallPoolScore: EightBallPoolScoreType = this.parseScore<EightBallPoolScoreType>(score);
    if (flip) return `${eightBallPoolScore.Player2Racks} - ${eightBallPoolScore.Player1Racks}`
    return `${eightBallPoolScore.Player1Racks} - ${eightBallPoolScore.Player2Racks}`
  }

  parseScore<T>(score: string): T {
    return JSON.parse(score) as T;
  }
}
