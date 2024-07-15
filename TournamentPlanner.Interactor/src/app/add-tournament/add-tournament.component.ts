import { Component } from '@angular/core';
import { TournamentDto } from '../tp-model/TpModel';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TimeService } from '../time.service';
import { TournamentPlannerService } from '../tournament-planner.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-tournament',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-tournament.component.html',
  styleUrl: './add-tournament.component.scss',
})
export class AddTournamentComponent {
  public tournamentDto: TournamentDto;

  constructor(
    private tp: TournamentPlannerService,
    private timeService: TimeService,
    private router: Router
  ) {
    this.tournamentDto = {
      name: '',
    };
  }

  onClickCreate() {
    this.tp.addTournament(this.tournamentDto).subscribe((res) => {
      console.log(res);
      this.router.navigate(['/tp'])
    });
  }

  onStartDateChange(event: Event) {
    const input = event.target as HTMLInputElement;
    const localDate = input.value;
    this.tournamentDto.startDate = this.timeService.convertToUTC(localDate);
  }
  onEndDateChange(event: Event) {
    const input = event.target as HTMLInputElement;
    const localDate = input.value;
    this.tournamentDto.endDate = this.timeService.convertToUTC(localDate);
  }
}
