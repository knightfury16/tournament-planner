import {
  Component,
  effect,
  EventEmitter,
  inject,
  input,
  Input,
  OnInit,
  Output,
  signal,
} from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { DrawDto, MatchTypeDto } from '../tp-model/TpModel';
import { TournamentPlannerService } from '../tournament-planner.service';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog } from '@angular/material/dialog';
import { MatCommonModule } from '@angular/material/core';
import { CommonModule } from '@angular/common';
import { getDateStringInFormat } from '../../Shared/Utility/dateTimeUtility';
import { RouterModule } from '@angular/router';
import { DrawTabViewType } from '../tournament-details-homepage/tournament-details-homepage.component';
import { LoadingService } from '../../Shared/loading.service';
import { AdminTournamentService } from '../../Shared/admin-tournament.service';
import { MatIconModule } from '@angular/material/icon';
import { SnackbarService } from '../../Shared/snackbar.service';
import { GroupSeedingDialogComponent } from '../group-seeding-dialog/group-seeding-dialog.component';

@Component({
  selector: 'app-tournament-draw-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatChipsModule,
    MatCommonModule,
    MatChipsModule,
    MatIconModule
  ],
  templateUrl: './tournament-draw-list.component.html',
  styleUrl: './tournament-draw-list.component.scss',
})
export class TournamentDrawListComponent implements OnInit {
  @Input({ required: true }) public tournamentId?: string;
  @Input() public manage: boolean = false;
  @Input() public tournamentStatusChange = false;
  @Output() drawTabChangeEvent = new EventEmitter<DrawTabViewType>();
  @Output() matchTypeId = new EventEmitter<number>();
  private _tpService = inject(TournamentPlannerService);
  private _adminService = inject(AdminTournamentService);
  private _loadingService = inject(LoadingService);
  private _snackBarService = inject(SnackbarService);
  public canIMakeDraw = signal(false);
  public draws = signal<DrawDto[] | undefined>(undefined);
  public readonly dialog = inject(MatDialog);

  constructor() {
    if (this.manage) {
      effect(async () => {
        console.log("CALING CAN I DRAW REQ")
        await this.checkIfICanMakeDraw();
      });
    }
  }

  async ngOnInit() {
    try {
      this._loadingService.show();
      var reqResponse = await this._tpService.getTournamentDraws(
        this.tournamentId!
      );
      this.draws.set(reqResponse);
      if (this.manage) await this.checkIfICanMakeDraw();
      console.log("CALING CAN I DRAW REQ from ONINIT")
      this._loadingService.hide();
    } catch (error) {
      this._loadingService.hide();
      console.log(error);
      console.log(
        (error as any).error ??
        (error as any).error?.Error ??
        'An unknown error occurred.'
      );
    }
  }
  async checkIfICanMakeDraw() {
    try {
      var canIDrawDto = await this._adminService.canIDraw(this.tournamentId!.toString());
      this.canIMakeDraw.set(canIDrawDto.success);
    } catch (err: any) {
      throw new Error(err);
    }
  }

  getClassName(matchType: MatchTypeDto): string {
    if (matchType.isCompleted) return 'chip-completed';

    return 'chip-incompleted';
  }

  getCreatedAtDate(draw: DrawDto): string {
    return getDateStringInFormat(new Date(draw.createdAt!));
  }

  emitDrawTabViewChangeEvent(matchTypeId: number) {
    this.drawTabChangeEvent.emit(DrawTabViewType.DetailView);
    this.matchTypeId.emit(matchTypeId);
  }

  public async makeDraw() {
    try {
      // check tournament start date
      // show the seeding dialog
      this.seedingDilaog();


      // make the draw
    } catch (error: any) {
      this._snackBarService.showError(error?.error?.Error ?? "An unknown error occurred.");
      this._loadingService.hide();
      console.log(error);
    }
  }


  public seedingDilaog() {
    var dialogRef = this.dialog.open(GroupSeedingDialogComponent, { height: '400px', width: '600px' });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The Dialog was closed')

    })
  }

}
