import { Component, EventEmitter, Input, input, Output } from '@angular/core';
import { DrawTabViewType } from '../tournament-details-homepage/tournament-details-homepage.component';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatTabGroup, MatTabsModule } from '@angular/material/tabs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-tournament-draw-details',
  standalone: true,
  imports: [MatIconModule,MatTabsModule,CommonModule, MatCardModule, MatButtonModule],
  templateUrl: './tournament-draw-details.component.html',
  styleUrl: './tournament-draw-details.component.scss',
})
export class TournamentDrawDetailsComponent implements OnInit {
  @Input({ required: true }) public matchTypeId?: number;
  @Output() drawTabChangeEvent = new EventEmitter<DrawTabViewType>();

  public emitDrawTabChangeEvent() {
    this.drawTabChangeEvent.emit(DrawTabViewType.ListView);
  }
}
