import { Component, Input, input, signal } from '@angular/core';
import { MatTabsModule } from '@angular/material/tabs';
import { PlayerDto, TournamentDto } from '../tp-model/TpModel';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { TournamentPlayerListComponent } from "../tournament-player-list/tournament-player-list.component";
import { TournamentPlayerDetailsComponent } from '../tournament-player-details/tournament-player-details.component';
import { TournamentDetailsComponent } from "../tournament-details/tournament-details.component";

export enum PlayerTabViewType {
  ListView = "List View",
  DetailView = "Detail View"
}

@Component({
  selector: 'app-tournament-details-homepage',
  standalone: true,
  imports: [MatTabsModule, CommonModule, MatCardModule, TournamentPlayerListComponent, TournamentPlayerDetailsComponent, TournamentDetailsComponent],
  templateUrl: './tournament-details-homepage.component.html',
  styleUrl: './tournament-details-homepage.component.scss'
})
export class TournamentDetailsHomepageComponent {
  @Input() public tournamentId?: string;

  public tournamentName = signal('');
  public tournamentParticipants = signal<PlayerDto[] | undefined>(undefined);
  public playerTabViewType = PlayerTabViewType;

  public playerTabView = signal<PlayerTabViewType>(PlayerTabViewType.ListView);


  public togglePlayerTabView(view: PlayerTabViewType) {
    this.playerTabView.set(view);
  }

  public tournamentNameEC(tournamentName: string) {
    this.tournamentName.set(tournamentName);
  }

  public tournamentParticipantsEC(participants: PlayerDto[]) {
    this.tournamentParticipants.set(participants);
  }


}
