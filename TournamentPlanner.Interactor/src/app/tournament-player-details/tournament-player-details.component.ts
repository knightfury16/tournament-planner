import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PlayerTabViewType } from '../tournament-details-homepage/tournament-details-homepage.component';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-tournament-player-details',
  standalone: true,
  imports: [MatButtonModule],
  templateUrl: './tournament-player-details.component.html',
  styleUrl: './tournament-player-details.component.scss'
})
export class TournamentPlayerDetailsComponent {
  @Input({ required: true }) tournamentId?: string;
  @Output() playerTabChangeEvent = new EventEmitter<PlayerTabViewType>();

  public emitTabViewChangeEvent()
  {
    this.playerTabChangeEvent.emit(PlayerTabViewType.ListView)
  }

}
