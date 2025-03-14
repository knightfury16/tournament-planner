import { Injectable } from '@angular/core';
import { GameTypeColor, GameTypeSupported, TournamentStatus, TournamentStatusColor } from '../app/tp-model/TpModel';
import { trimAllSpace } from './Utility/stringUtility';

@Injectable({
  providedIn: 'root'
})
export class TournamentColorService {
  constructor() { }

  public getTournamentStatusColor(status: string | undefined | null): string {
    if (status == null || status == undefined || status == "") return TournamentStatusColor.Default;
    switch (status) {
      case TournamentStatus.Draft:
        return TournamentStatusColor.Draft;
      case trimAllSpace(TournamentStatus.RegistrationOpen):
        return TournamentStatusColor.RegistrationOpen
      case trimAllSpace(TournamentStatus.RegistrationClosed):
        return TournamentStatusColor.RegistrationClosed
      case TournamentStatus.Ongoing:
        return TournamentStatusColor.Ongoing
      case TournamentStatus.Completed:
        return TournamentStatusColor.Completed;
      default:
        return TournamentStatusColor.Draft;
    }
  }

  public getGameTypeColor(gameType: GameTypeSupported | undefined | null | string): string {
    if (gameType == null || gameType == undefined) return GameTypeColor.Default;
    switch (gameType) {
      case trimAllSpace(GameTypeSupported.TableTennis):
        return GameTypeColor.TableTennis
      case trimAllSpace(GameTypeSupported.EightBallPool):
        return GameTypeColor.EightBallPool
      default:
        return GameTypeColor.Default
    }
  }
}
