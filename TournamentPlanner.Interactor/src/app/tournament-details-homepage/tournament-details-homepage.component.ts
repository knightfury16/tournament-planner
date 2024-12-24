import { Component, Input, input, signal } from '@angular/core';
import { MatTabsModule } from '@angular/material/tabs';
import { TournamentDto } from '../tp-model/TpModel';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { TournamentPlayerListComponent } from "../tournament-player-list/tournament-player-list.component";
import { TournamentPlayerDetailsComponent } from '../tournament-player-details/tournament-player-details.component';

export enum PlayerTabViewType {
  ListView = "List View",
  DetailView = "Detail View"
}

@Component({
  selector: 'app-tournament-details-homepage',
  standalone: true,
  imports: [MatTabsModule, CommonModule, MatCardModule, TournamentPlayerListComponent, TournamentPlayerDetailsComponent],
  templateUrl: './tournament-details-homepage.component.html',
  styleUrl: './tournament-details-homepage.component.scss'
})
export class TournamentDetailsHomepageComponent {
  @Input() public tournamentId?: string;
  public playerTabViewType = PlayerTabViewType;

  public playerTabView = signal<PlayerTabViewType>(PlayerTabViewType.ListView);


  public togglePlayerTabView(view: PlayerTabViewType) {
    this.playerTabView.set(view);
  }


}
