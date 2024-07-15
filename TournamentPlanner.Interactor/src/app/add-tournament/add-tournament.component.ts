import { Component } from '@angular/core';
import { TournamentDto } from '../tp-model/TpModel';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-add-tournament',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-tournament.component.html',
  styleUrl: './add-tournament.component.scss',
})
export class AddTournamentComponent {

  public tournamentDto: TournamentDto;

  constructor(){
    this.tournamentDto = {
      name: ""
    }
  }

  onClickCreate() {
    throw new Error('Method not implemented.');
  }
}
