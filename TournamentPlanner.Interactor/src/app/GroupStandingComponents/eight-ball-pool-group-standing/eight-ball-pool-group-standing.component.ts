import { Component } from '@angular/core';
import { BaseGroupStandingComponent } from '../base-group-standing/base-group-standing.component';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-eight-ball-pool-group-standing',
  standalone: true,
  imports: [MatTableModule],
  templateUrl: './eight-ball-pool-group-standing.component.html',
  styleUrl: './eight-ball-pool-group-standing.component.scss'
})
export class EightBallPoolGroupStandingComponent extends BaseGroupStandingComponent {
  displayedColumns = [
    'ranking',
    'name',
    'points',
    'matchesPlayed',
    'matchRecord',
    'gamesRecord',
  ];

}
