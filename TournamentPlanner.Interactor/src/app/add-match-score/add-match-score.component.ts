import { Component, EventEmitter, Input, Output } from '@angular/core';
import { GameTypeDto } from '../tp-model/TpModel';
import { MatchTabViewType } from '../tp-model/types';

@Component({
  selector: 'app-add-match-score',
  standalone: true,
  imports: [],
  templateUrl: './add-match-score.component.html',
  styleUrl: './add-match-score.component.scss'
})
export class AddMatchScoreComponent {
  @Input({required: true}) public matchId?: number;
  @Input({required: true}) public gameType?: GameTypeDto | null;

  @Output() matchTabViewChangeEvent = new EventEmitter<MatchTabViewType>();


}
