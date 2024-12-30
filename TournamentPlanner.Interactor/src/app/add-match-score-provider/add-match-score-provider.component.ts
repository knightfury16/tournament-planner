import { Component, Input } from '@angular/core';
import { GameTypeDto, MatchDto } from '../tp-model/TpModel';

@Component({
  selector: 'app-add-match-score-provider',
  standalone: true,
  imports: [],
  templateUrl: './add-match-score-provider.component.html',
  styleUrl: './add-match-score-provider.component.scss'
})
export class AddMatchScoreProviderComponent {
  @Input({ required: true }) public match?: MatchDto;
  @Input({ required: true }) public gameType?: GameTypeDto | null;

}
