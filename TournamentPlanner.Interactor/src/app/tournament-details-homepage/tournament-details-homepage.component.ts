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
    TournamentPlayerDetailsComponent,TournamentDrawDetailsComponent, TournamentDetailsComponent, TournamentDrawListComponent],
  templateUrl: './tournament-details-homepage.component.html',
  styleUrl: './tournament-details-homepage.component.scss'
})
export class TournamentDetailsHomepageComponent {
  @Input() public tournamentId?: string;

  public tournamentName = signal('');
  public tournamentParticipants = signal<PlayerDto[] | undefined>(undefined);
  public playerTabViewType = PlayerTabViewType;
  public drawTabViewType = DrawTabViewType;

  public playerTabView = signal<PlayerTabViewType>(PlayerTabViewType.ListView);
  public drawTabView = signal<DrawTabViewType>(DrawTabViewType.ListView);
  public drawId = signal<number | undefined> (undefined);


  public togglePlayerTabView(view: PlayerTabViewType) {
    this.playerTabView.set(view);
  }
  public toggleDrawTabView(view: DrawTabViewType){
    this.drawTabView.set(view);
  }

  public tournamentNameEC(tournamentName: string) {
    this.tournamentName.set(tournamentName);
  }

  public tournamentParticipantsEC(participants: PlayerDto[]) {
    this.tournamentParticipants.set(participants);
  }

  public catchDrawIdEC(drawId: number){
    this.drawId.set(drawId);
  }


}
