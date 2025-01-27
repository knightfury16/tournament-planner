import { Component, EventEmitter, Input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { TournamentPlayerListComponent } from '../tournament-player-list/tournament-player-list.component';
import { TournamentPlayerDetailsComponent } from '../tournament-player-details/tournament-player-details.component';
import { TournamentDetailsComponent } from '../tournament-details/tournament-details.component';
import { DrawTabViewType, PlayerTabViewType } from '../tournament-details-homepage/tournament-details-homepage.component';
import { PlayerDto, TournamentDto } from '../tp-model/TpModel';
import { ManageTournamentDetailsComponent } from "../manage-tournament-details/manage-tournament-details.component";
import { TournamentMatchesListComponent } from "../tournament-matches-list/tournament-matches-list.component";
import { AddMatchScoreComponent } from '../add-match-score/add-match-score.component';
import { MatchTabViewType } from '../tp-model/types';
import { TournamentDrawListComponent } from '../tournament-draw-list/tournament-draw-list.component';
import { TournamentDrawDetailsComponent } from '../tournament-draw-details/tournament-draw-details.component';
import { MatButtonModule } from '@angular/material/button';
import { MakeMatchScheduleComponent } from "../make-match-schedule/make-match-schedule.component";
import { ManageTournamentViewTabIndex } from '../tp-model/ManageTournamentVIewTabIndex';

@Component({
  selector: 'app-manage-tournament-homepage',
  standalone: true,
  imports: [MatTabsModule, AddMatchScoreComponent, CommonModule, MatCardModule,
    TournamentPlayerListComponent, TournamentPlayerDetailsComponent, ManageTournamentDetailsComponent,
    TournamentMatchesListComponent, TournamentDrawListComponent, MatButtonModule, TournamentDrawDetailsComponent, MakeMatchScheduleComponent],
  templateUrl: './manage-tournament-homepage.component.html',
  styleUrl: './manage-tournament-homepage.component.scss'
})
export class ManageTournamentHomepageComponent {

  @Input() public tournamentId?: string;

  public tournament = signal<TournamentDto | undefined>(undefined);
  public drawTabView = signal<DrawTabViewType>(DrawTabViewType.ListView);
  public tournamentStatusChange = signal(false);
  public matchTypeId = signal<number | undefined> (undefined);
  public tournamentParticipants = signal<PlayerDto[] | undefined>(undefined);
  public playerTabViewType = PlayerTabViewType;
  public drawTabViewType = DrawTabViewType;

  public tabSelectedIndex = signal<number>(ManageTournamentViewTabIndex.DefaultTabIndex);

  public matchTabViewType = MatchTabViewType;

  public playerTabView = signal<PlayerTabViewType>(PlayerTabViewType.ListView);
  public matchTabView = signal<MatchTabViewType>(MatchTabViewType.MatchView);

  public matchId = signal<number | undefined>(undefined);

  public togglePlayerTabView(view: PlayerTabViewType) {
    this.playerTabView.set(view);
  }
  public toggleMatchViewType(view: MatchTabViewType) {
    this.matchTabView.set(view);
  }

  public tournamentEC(tournament: TournamentDto) {
    this.tournament.set(tournament);
  }

  public tournamentParticipantsEC(participants: PlayerDto[]) {
    this.tournamentParticipants.set(participants);
  }

  public tabViewChangeToAddScoreWithMatchId(event: { viewType: MatchTabViewType, matchId: number }) {
    this.matchTabView.set(event.viewType);
    this.matchId.set(event.matchId);
  }

  public tabChangeToListView() {
    this.matchTabView.set(MatchTabViewType.MatchView);
  }
  public toggleDrawTabView(view: DrawTabViewType) {
    this.drawTabView.set(view);
  }

  public catchMatchTypeId(matchType: number){
    this.matchTypeId.set(matchType);
  }

  public tournamentStatusChangeEC()
  {
    this.tournamentStatusChange.update(old => !old); //just triggering it
  }
}
