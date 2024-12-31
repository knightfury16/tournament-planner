import { Component, EventEmitter, inject, Input, OnInit, Output, signal } from '@angular/core';
import { TournamentDto, TournamentStatus } from '../tp-model/TpModel';
import { TournamentPlannerService } from '../tournament-planner.service';
import { transformTournamentIsoDate } from '../../Shared/Utility/dateTimeUtility';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { mapStringToEnum } from '../../Shared/Utility/stringUtility';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { SnackbarService } from '../../Shared/snackbar.service';
import { AdminTournamentService } from '../../Shared/admin-tournament.service';

@Component({
  selector: 'app-manage-tournament-details',
  standalone: true,
  imports: [
    MatFormFieldModule,
    ReactiveFormsModule,
    CommonModule,
    MatSelectModule,
    MatCardModule,
    MatButtonModule
  ],
  templateUrl: './manage-tournament-details.component.html',
  styleUrl: './manage-tournament-details.component.scss'
})
export class ManageTournamentDetailsComponent implements OnInit {

  @Input({ required: true }) public tournamentId?: string;
  @Output() public tournamentEE = new EventEmitter<TournamentDto>();
  @Output() public statusChangeEE = new EventEmitter();

  public tournamentStatus = TournamentStatus;
  private _tpService = inject(TournamentPlannerService);
  private _adminTpService = inject(AdminTournamentService);
  private _snackBarService = inject(SnackbarService);

  public tournamentDetails = signal<TournamentDto | null>(null);
  public selectedStatus: TournamentStatus | undefined = undefined;
  public statusFormControl = new FormControl();

  async ngOnInit() {
    if (this.tournamentId == undefined) { return; }
    var tourDetail = await this._tpService.getTournamentById(this.tournamentId)
    var transformDateTournament = transformTournamentIsoDate(tourDetail);
    this.tournamentDetails.set(transformDateTournament);
    this.emitTournament(tourDetail);// name will be here
    this.setSelectedStatus();
  }

  setSelectedStatus() {
    this.selectedStatus = this.getStatusValue();
  }
  emitTournament(tournament: TournamentDto) {
    this.tournamentEE.emit(tournament);
  }

  getStatusValue(): TournamentStatus {
    var status = this.tournamentDetails()?.status ?? TournamentStatus.Draft.toString();
    return mapStringToEnum(TournamentStatus, status);
  }

  getTournamentStatusString(): string[] {
    // I have to do this hack because if there is space in enum values all hell break loose
    //ts can not map it properly
    var status: string[] = [];
    Object.keys(TournamentStatus).forEach(
      key => {
        if (!isNaN(Number(key))) return;
        status.push(TournamentStatus[key as keyof typeof TournamentStatus].toString());
      }
    )
    return status;
  }
  public async changeStatus() {
    try {
      var changedStatus = this.statusFormControl.value;
      var statusChangeResponse = await this._adminTpService.changeTournamentStatus(this.tournamentId!, changedStatus);
      this.statusChangeEE.emit();
      console.log(statusChangeResponse);
      this._snackBarService.showMessage(statusChangeResponse.message);
    } catch (error : any) {
      console.log(error!.error);
      this._snackBarService.showError((error as any).error ?? (error as any).error?.Error ?? "An unknown error occurred.");
    }
  }

}
