import { Component } from '@angular/core';
import { BaseGroupStandingComponent } from '../base-group-standing/base-group-standing.component';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-table-tennis-group-standing',
  standalone: true,
  imports: [MatTableModule],
  templateUrl: './table-tennis-group-standing.component.html',
  styleUrl: './table-tennis-group-standing.component.scss',
})
export class TableTennisGroupStandingComponent extends BaseGroupStandingComponent {
  displayedColumns = [
    'ranking',
    'name',
    'points',
    'matchesPlayed',
    'matchRecord',
    'gamesRecord',
    'pointsRecord',
  ];
}
