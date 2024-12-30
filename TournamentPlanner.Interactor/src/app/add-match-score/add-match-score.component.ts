import { Component, EventEmitter, Input, Output } from '@angular/core';
import { GameTypeDto } from '../tp-model/TpModel';
import { MatchTabViewType } from '../tp-model/types';
import {  MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-add-match-score',
  standalone: true,
  imports: [MatCardModule, MatButtonModule, MatIconModule],
  templateUrl: './add-match-score.component.html',
  styleUrl: './add-match-score.component.scss'
})
export class AddMatchScoreComponent {
  @Input({required: true}) public matchId?: number;
  @Input({required: true}) public gameType?: GameTypeDto | null;

  @Output() matchTabViewChangeEvent = new EventEmitter<MatchTabViewType>();

  public emitMatchTabChangeEvent(){
    this.matchTabViewChangeEvent.emit(MatchTabViewType.MatchView);
  }


}
