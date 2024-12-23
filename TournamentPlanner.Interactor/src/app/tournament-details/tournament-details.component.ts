import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-tournament-details',
  standalone: true,
  imports: [],
  templateUrl: './tournament-details.component.html',
  styleUrl: './tournament-details.component.scss'
})
export class TournamentDetailsComponent {
  @Input() public tournamentId?: string;

}
