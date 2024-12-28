import { Component, Input, input } from '@angular/core';
import { PlayerStandingDto } from '../../tp-model/TpModel';

@Component({
  selector: 'app-base-group-standing',
  standalone: true,
  imports: [],
  templateUrl: './base-group-standing.component.html',
  styleUrl: './base-group-standing.component.scss'
})
export class BaseGroupStandingComponent {

  @Input({required: true}) public playerStandings?: PlayerStandingDto[];

}
