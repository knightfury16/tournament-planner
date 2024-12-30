import { Component, computed, effect, EventEmitter, inject, Input, OnInit, Output, signal } from '@angular/core';
import { DrawDto, GameTypeDto, PlayerDto, TournamentDto } from '../tp-model/TpModel';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { TournamentPlannerService } from '../tournament-planner.service';
import { LoadingService } from '../../Shared/loading.service';
import { MatGridListModule } from '@angular/material/grid-list';
import { TournamentMatchComponent } from "../tournament-match/tournament-match.component";
import { CommonModule } from '@angular/common';
import { MatchTabViewType } from '../manage-tournament-homepage/manage-tournament-homepage.component';


export type MatchModel = {
  matchId: number;
  roundName: string | undefined | null;
  matchTypeName: string | undefined;
  matchPlayed: Date | string | undefined | null;
  matchScheduled: Date | string | undefined | null;
  firstPlayer: PlayerDto | undefined;
  secondPlayer: PlayerDto | undefined;
  winner: PlayerDto | undefined | null;
  scoreJson: string | undefined | null;
  court: string | undefined | null;
}

// export type MatchCardModel = {
//   gameScheduled: string | undefined | null;
//   matches: MatchModel[];
// }



@Component({
  selector: 'app-tournament-matches-list',
  standalone: true,
  imports: [MatCardModule, MatTabsModule, MatGridListModule, TournamentMatchComponent, CommonModule],
  templateUrl: './tournament-matches-list.component.html',
  styleUrl: './tournament-matches-list.component.scss'
})
export class TournamentMatchesListComponent implements OnInit {
  @Input({required: true}) public tournamentId?: string;
  @Input({required: true}) public gameType?: GameTypeDto | null;

  @Input() public manage: boolean = false;

  @Output() public matchTabChangeEventWithSelectedMatchId = new EventEmitter<{viewType: MatchTabViewType, matchId: number}>();

  private _tpService = inject(TournamentPlannerService);
  private _loadingService = inject(LoadingService);

  public draws = signal< DrawDto[] | undefined>(undefined);
  public matchSelectedId = signal<number | undefined>(undefined);

  public matchCardModels = computed<{[roundName: string]: MatchModel[]}>(() => {
    if(this.draws() == undefined) return {};
    return this.getMatchCardModels().reduce((acc: {[roundName: string]: MatchModel[]}, match) => {
      const roundName = match.roundName || 'Default';
      if (!acc[roundName]) acc[roundName] = [];
      acc[roundName].push(match);
      return acc;
    }, {});
  });

  constructor(){
    effect(() =>{
      console.log("MATCHCARDMODLESSSSSS:::", this.matchCardModels());
    })
  }


  async ngOnInit(){
    try {
      this._loadingService.show()
      var response = await this._tpService.getTournamentMatches(this.tournamentId!.toString());
      this.draws.set(response);
      this._loadingService.hide();
      
    } catch (error: any) {
      this._loadingService.hide();
      console.log(error);
    }
  }

  public matchSelectedEC(matchId: number){
    this.matchSelectedId.set(matchId);
    //if from manage then change match tab view to addscore
    if(this.manage){
      this.emitMatchTabChangeEvent();
    }
  }

  public emitMatchTabChangeEvent()
  {
    // this.matchTabChangeEventWithSelectedMatchId.emit({ viewType:MatchTabViewType.AddScoreView, matchId: this.matchSelectedId()! })
  }

  getMatchCardModels(): MatchModel[] {
    return this.draws()!.flatMap(draw => 
      draw.matchType.rounds.flatMap(round => 
        round.matches.map(match  => {
          const matchCardModel: MatchModel = {
            matchId: match.id,
            firstPlayer: match.firstPlayer,
            secondPlayer: match.secondPlayer,
            winner: match.winner,
            matchPlayed: match.gamePlayed,
            matchScheduled: match.gameScheduled,
            roundName: round.roundName,
            matchTypeName: draw.matchType.name,
            scoreJson: match.scoreJson,
            court: match.courtName
          };
          return matchCardModel;
        })
      ))
  }
}
