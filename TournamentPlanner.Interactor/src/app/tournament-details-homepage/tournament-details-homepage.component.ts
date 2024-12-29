import { Component, Input, input, signal } from '@angular/core';
import { MatTabsModule } from '@angular/material/tabs';
import { PlayerDto, TournamentDto } from '../tp-model/TpModel';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { TournamentPlayerListComponent } from "../tournament-player-list/tournament-player-list.component";
import { TournamentPlayerDetailsComponent } from '../tournament-player-details/tournament-player-details.component';
import { TournamentDetailsComponent } from "../tournament-details/tournament-details.component";
import { TournamentDrawListComponent } from "../tournament-draw-list/tournament-draw-list.component";
import { TournamentDrawDetailsComponent } from '../tournament-draw-details/tournament-draw-details.component';
import { TournamentMatchesListComponent } from "../tournament-matches-list/tournament-matches-list.component";

export enum PlayerTabViewType {
  ListView = "List View",
  DetailView = "Detail View"
}

export enum DrawTabViewType{
  ListView = "List View",
  DetailView = "Detail View"
}

@Component({
  selector: 'app-tournament-details-homepage',
  standalone: true,
  imports: [MatTabsModule, CommonModule, MatCardModule, TournamentPlayerListComponent,
    TournamentPlayerDetailsComponent, TournamentDrawDetailsComponent, TournamentDetailsComponent, TournamentDrawListComponent, TournamentMatchesListComponent],
  templateUrl: './tournament-details-homepage.component.html',
  styleUrl: './tournament-details-homepage.component.scss'
})
export class TournamentDetailsHomepageComponent {
  @Input() public tournamentId?: string;

  public tournament = signal<TournamentDto | undefined>(undefined);
  public tournamentParticipants = signal<PlayerDto[] | undefined>(undefined);
  public playerTabViewType = PlayerTabViewType;
  public drawTabViewType = DrawTabViewType;

  public playerTabView = signal<PlayerTabViewType>(PlayerTabViewType.ListView);
  public drawTabView = signal<DrawTabViewType>(DrawTabViewType.ListView);
  public matchTypeId = signal<number | undefined> (undefined);


  public togglePlayerTabView(view: PlayerTabViewType) {
    this.playerTabView.set(view);
  }
  public toggleDrawTabView(view: DrawTabViewType){
    this.drawTabView.set(view);
  }

  public tournamentEC(tournament: TournamentDto) {
    this.tournament.set(tournament);
  }

  public tournamentParticipantsEC(participants: PlayerDto[]) {
    this.tournamentParticipants.set(participants);
  }

  public catchMatchTypeId(matchType: number){
    this.matchTypeId.set(matchType);
  }


}
