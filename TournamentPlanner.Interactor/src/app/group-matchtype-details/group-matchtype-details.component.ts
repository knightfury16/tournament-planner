import { Component, Input } from '@angular/core';
import { GameTypeDto } from '../tp-model/TpModel';
import { MatchDto } from '../tp-model/TpModel';
import { PlayerDto } from '../tp-model/TpModel';
import { RoundRobinTableComponent } from '../round-robin-table/round-robin-table.component';
import { GroupStandingProviderComponent } from '../group-standing-provider/group-standing-provider.component';

@Component({
  selector: 'app-group-matchtype-details',
  standalone: true,
  imports: [RoundRobinTableComponent,GroupStandingProviderComponent],
  templateUrl: './group-matchtype-details.component.html',
  styleUrl: './group-matchtype-details.component.scss'
})
export class GroupMatchtypeDetailsComponent {
  @Input({ required: true }) players?: PlayerDto[];
  @Input({ required: true }) matches?: MatchDto[];

  @Input({ required: true }) gameType?: GameTypeDto | null;
  @Input({ required: true }) matchTypeId?: number;
}
