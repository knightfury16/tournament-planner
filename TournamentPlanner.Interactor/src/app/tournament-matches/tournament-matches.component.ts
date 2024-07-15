import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-tournament-matches',
  standalone: true,
  imports: [],
  templateUrl: './tournament-matches.component.html',
  styleUrl: './tournament-matches.component.scss'
})
export class TournamentMatchesComponent {

  @Input() public tournamentId?: string;

  constructor(){
    console.log("From matches component", this.tournamentId)
  }

}
