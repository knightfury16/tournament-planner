import { Component, EventEmitter, inject, Input, OnInit, Output, signal } from '@angular/core';
import { PlayerDto } from '../tp-model/TpModel';
import { TournamentPlannerService } from '../tournament-planner.service';
import { MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';
import { PlayerTabViewType } from '../tournament-details-homepage/tournament-details-homepage.component';
import { MatButtonModule } from '@angular/material/button';
import { MatExpansionModule } from '@angular/material/expansion';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-tournament-player-list',
  standalone: true,
  imports: [MatListModule, MatExpansionModule, MatButtonModule, MatCardModule, CommonModule],
  templateUrl: './tournament-player-list.component.html',
  styleUrl: './tournament-player-list.component.scss'
})
export class TournamentPlayerListComponent {
  @Input({ required: true }) tournamentId?: string;
  @Input() players?: PlayerDto[];
  @Output() playerTabChangeEvent = new EventEmitter<PlayerTabViewType>();

  public tpServive = inject(TournamentPlannerService);

  //Im getting the tournament participants through the torunament details calls
  // public players = signal<PlayerDto[] | undefined>(this.participantsPlayer);
  // async ngOnInit() {
  // var playerResponse = await this.tpServive.getTournamentPlayers(this.tournamentId!);
  // this.players.set(this.participantsPlayer);
  // }

  //From the list view i will always trigger the detail view
  public emitTabViewChangeEvent(): void {
    this.playerTabChangeEvent.emit(PlayerTabViewType.DetailView);
  }
  public getRoundWinRatio(playerWinRation: number | null | undefined): string {
    if (playerWinRation) {
      return `${Math.ceil(playerWinRation * 100)}%`
    }
    return "N/A";
  }


}
