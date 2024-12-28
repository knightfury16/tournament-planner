import { Injectable, Type } from '@angular/core';
import { GameTypeDto, GameTypeSupported } from '../app/tp-model/TpModel';
import { trimAllSpace } from './Utility/stringUtility';
import { TableTennisGroupStandingComponent } from '../app/GroupStandingComponents/table-tennis-group-standing/table-tennis-group-standing.component';
import { EightBallPoolGroupStandingComponent } from '../app/GroupStandingComponents/eight-ball-pool-group-standing/eight-ball-pool-group-standing.component';
import { BaseGroupStandingComponent } from '../app/GroupStandingComponents/base-group-standing/base-group-standing.component';

@Injectable({
  providedIn: 'root',
})
export class GroupStandingProviderService {
  constructor() {}

  public getGroupStaindingComponent(gameType: GameTypeDto): Type<BaseGroupStandingComponent> {
    if (gameType.name == trimAllSpace(GameTypeSupported.TableTennis))
      return TableTennisGroupStandingComponent;
    if(gameType.name == trimAllSpace(GameTypeSupported.EightBallPool)){
      return EightBallPoolGroupStandingComponent;
    }
    throw new Error('Unsupported game type');
  }
}
