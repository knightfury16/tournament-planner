import { Injectable } from '@angular/core';
import { GameTypeDto, GameTypeSupported } from '../app/tp-model/TpModel';
import { IScore } from '../GameTypeSuportedScore/IScore';
import { TableTennisScore, TableTennisScoreType } from '../GameTypeSuportedScore/TableTennisScore';
import { trimAllSpace } from './Utility/stringUtility';

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
    var ttScore: TableTennisScoreType = JSON.parse(score);
    if(flip)return `${ttScore.Player2Sets} - ${ttScore.Player1Sets}`
    return `${ttScore.Player1Sets} - ${ttScore.Player2Sets}`
  }

}
