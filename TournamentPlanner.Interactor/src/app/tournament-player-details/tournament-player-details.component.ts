import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PlayerTabViewType } from '../tournament-details-homepage/tournament-details-homepage.component';
import { MatButtonModule } from '@angular/material/button';
import { PlayerDto } from '../tp-model/TpModel';

@Component({
  selector: 'app-tournament-player-details',
  standalone: true,
  imports: [MatButtonModule],
  templateUrl: './tournament-player-details.component.html',
  styleUrl: './tournament-player-details.component.scss'
})
export class TournamentPlayerDetailsComponent {
  // @Input({ required: true }) tournamentId?: string;
  @Input({ required: false }) player: PlayerDto | undefined;
  @Output() playerTabChangeEvent = new EventEmitter<PlayerTabViewType>();

  public emitTabViewChangeEvent() {
    this.playerTabChangeEvent.emit(PlayerTabViewType.ListView)
  }

}
