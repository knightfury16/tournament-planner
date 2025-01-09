import { Component, inject, Input, OnInit, signal } from '@angular/core';
import { AdminTournamentService } from '../../Shared/admin-tournament.service';
import { LoadingService } from '../../Shared/loading.service';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-make-match-schedule',
  standalone: true,
  imports: [MatButtonModule],
  templateUrl: './make-match-schedule.component.html',
  styleUrl: './make-match-schedule.component.scss'
})
export class MakeMatchScheduleComponent implements OnInit {
  @Input({ required: true }) public tournamentId?: string;
  public canISchedule = signal(false);
  private _adminService = inject(AdminTournamentService);
  private _loadingService = inject(LoadingService);


  async ngOnInit() {
    try {
      this._loadingService.show();
      var canIScheduleDto = await this._adminService.canISchedule(this.tournamentId!.toString());
      console.log("CAN I SCHEDULE RESUPONSE:::", canIScheduleDto);
      this.canISchedule.set(canIScheduleDto.success);
    } catch (err: any) {
      throw new Error(err);
    }
  }
}
