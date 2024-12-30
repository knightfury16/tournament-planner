import {
  Component,
  computed,
  EventEmitter,
  inject,
  Input,
  input,
  OnInit,
  Output,
  signal,
} from '@angular/core';
import { DrawTabViewType } from '../tournament-details-homepage/tournament-details-homepage.component';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatTabGroup, MatTabsModule } from '@angular/material/tabs';
import { CommonModule } from '@angular/common';
import { GameTypeDto, MatchDto, MatchTypeDto, RoundDto } from '../tp-model/TpModel';
import { TournamentPlannerService } from '../tournament-planner.service';
import { MatTableModule } from '@angular/material/table';
import { MatListModule } from '@angular/material/list';
import { RoundRobinTableComponent } from "../round-robin-table/round-robin-table.component";
import { GroupStandingProviderComponent } from "../group-standing-provider/group-standing-provider.component";
import { MatGridListModule } from '@angular/material/grid-list';
import { TournamentMatchComponent } from '../tournament-match/tournament-match.component';
import { MatchModel } from '../tournament-matches-list/tournament-matches-list.component';

@Component({
  selector: 'app-tournament-draw-details',
  standalone: true,
  imports: [
    MatIconModule,
    MatTabsModule,
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatListModule,
    RoundRobinTableComponent,
    GroupStandingProviderComponent,
    MatGridListModule,
    TournamentMatchComponent
  ],
  templateUrl: './tournament-draw-details.component.html',
  styleUrl: './tournament-draw-details.component.scss',
})
export class TournamentDrawDetailsComponent implements OnInit {
  @Input({ required: true }) public matchTypeId?: number;
  @Input({ required: true }) public gameType?: GameTypeDto | null;
  @Output() drawTabChangeEvent = new EventEmitter<DrawTabViewType>();
  private _tpService = inject(TournamentPlannerService);

  public matchType = signal<MatchTypeDto | undefined>(undefined);
  public players = computed(() => this.matchType()?.players ?? undefined);
  public rounds = computed(() => this.matchType()?.rounds ?? undefined);
  public matches = computed<MatchDto[]>(() => {
    var matches: MatchDto[] = [];
    if (this.rounds() == undefined) return matches;

    this.rounds()!.forEach((round) => {
      if (round.matches == undefined || round.matches.length == 0) return;
      matches = [...matches, ...round.matches];
    });
    return matches;
  });

  async ngOnInit() {
    try {
      var response = await this._tpService.getMatchType(
        this.matchTypeId!.toString()
      );
      console.log('MATCHTYPE', response);
      this.matchType.set(response);
    } catch (error) {
      console.log(error);
      console.log(
        (error as any).error ??
        (error as any).error?.Error ??
        'An unknown error occurred.'
      );
    }
  }

  public emitDrawTabChangeEvent() {
    this.drawTabChangeEvent.emit(DrawTabViewType.ListView);
  }
  getMatchModel(match: MatchDto, round: RoundDto): MatchModel {
    return {
      firstPlayer: match.firstPlayer,
      secondPlayer: match.secondPlayer,
      matchId: match.id,
      court: match.courtName,
      matchPlayed: match.gamePlayed,
      matchScheduled: match.gameScheduled,
      scoreJson: match.scoreJson,
      matchTypeName: this.matchType()?.name,
      roundName: round.roundName,
      winner: match.winner
    }
  }
}
