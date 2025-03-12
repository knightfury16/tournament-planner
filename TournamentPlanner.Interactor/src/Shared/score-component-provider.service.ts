import { Injectable, Type } from '@angular/core';
import { GameTypeDto, GameTypeSupported } from '../app/tp-model/TpModel';
import { BaseAddScoreComponent } from '../app/AddScoreComponents/base-add-score/base-add-score.component';
import { TableTennisAddScoreComponent } from '../app/AddScoreComponents/table-tennis-add-score/table-tennis-add-score.component';
import { trimAllSpace } from './Utility/stringUtility';
import { EightBallPoolAddScoreComponent } from '../app/AddScoreComponents/eight-ball-pool-add-score/eight-ball-pool-add-score.component';

@Injectable({
  providedIn: 'root'
})
export class ScoreComponentProviderService {

  constructor() { }

  public porvideGameScoreComponentBasedOnGameType(gameType: GameTypeDto): Type<BaseAddScoreComponent> {
    if (gameType.name == trimAllSpace(GameTypeSupported.TableTennis)) return TableTennisAddScoreComponent;
    if (gameType.name == trimAllSpace(GameTypeSupported.EightBallPool)) return EightBallPoolAddScoreComponent;
    return BaseAddScoreComponent;
  }
}
