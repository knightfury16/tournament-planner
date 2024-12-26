import { Component, EventEmitter, Input, input, Output } from '@angular/core';
import { DrawTabViewType } from '../tournament-details-homepage/tournament-details-homepage.component';

@Component({
  selector: 'app-tournament-draw-details',
  standalone: true,
  imports: [],
  templateUrl: './tournament-draw-details.component.html',
  styleUrl: './tournament-draw-details.component.scss',
})
export class TournamentDrawDetailsComponent {
  @Input({ required: true }) public drawId?: number;
  @Output() drawTabChangeEvent = new EventEmitter<DrawTabViewType>();

  public emitDrawTabChangeEvent() {
    this.drawTabChangeEvent.emit(DrawTabViewType.ListView);
  }
}
