import { Component, Input, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTabsModule } from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { TournamentPlayerListComponent } from '../tournament-player-list/tournament-player-list.component';
import { TournamentPlayerDetailsComponent } from '../tournament-player-details/tournament-player-details.component';
import { TournamentDetailsComponent } from '../tournament-details/tournament-details.component';
import { PlayerTabViewType } from '../tournament-details-homepage/tournament-details-homepage.component';
import { PlayerDto, TournamentDto } from '../tp-model/TpModel';
import { ManageTournamentDetailsComponent } from "../manage-tournament-details/manage-tournament-details.component";

@Component({
  selector: 'app-manage-tournament-homepage',
  standalone: true,
  imports: [MatTabsModule, CommonModule, MatCardModule, TournamentPlayerListComponent, TournamentPlayerDetailsComponent, ManageTournamentDetailsComponent],
  templateUrl: './manage-tournament-homepage.component.html',
  styleUrl: './manage-tournament-homepage.component.scss'
})
export class ManageTournamentHomepageComponent {

  @Input() public tournamentId?: string;

  public tournament = signal<TournamentDto | undefined>(undefined);
  public tournamentParticipants = signal<PlayerDto[] | undefined>(undefined);
  public playerTabViewType = PlayerTabViewType;

  public playerTabView = signal<PlayerTabViewType>(PlayerTabViewType.ListView);


  public togglePlayerTabView(view: PlayerTabViewType) {
    this.playerTabView.set(view);
  }

  public tournamentEC(tournament: TournamentDto) {
    this.tournament.set(tournament);
  }

  public tournamentParticipantsEC(participants: PlayerDto[]) {
    this.tournamentParticipants.set(participants);
  }

}
