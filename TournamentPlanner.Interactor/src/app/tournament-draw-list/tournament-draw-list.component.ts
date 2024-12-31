import {
  Component,
  EventEmitter,
  inject,
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
import { MatCommonModule } from '@angular/material/core';
import { CommonModule } from '@angular/common';
import { getDateStringInFormat } from '../../Shared/Utility/dateTimeUtility';
import { RouterModule } from '@angular/router';
import { DrawTabViewType } from '../tournament-details-homepage/tournament-details-homepage.component';
import { LoadingService } from '../../Shared/loading.service';

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
  ],
  templateUrl: './tournament-draw-list.component.html',
  styleUrl: './tournament-draw-list.component.scss',
})
export class TournamentDrawListComponent implements OnInit {
  @Input({ required: true }) public tournamentId?: string;
  @Input() public manage: boolean = false;
  @Output() drawTabChangeEvent = new EventEmitter<DrawTabViewType>();
  @Output() matchTypeId = new EventEmitter<number>();
  private _tpService = inject(TournamentPlannerService);
  private _loadingService = inject(LoadingService);
  public canIMakeDraw = signal(false);
  public draws = signal<DrawDto[] | undefined>(undefined);

  async ngOnInit() {
    try {
      this._loadingService.show();
      var reqResponse = await this._tpService.getTournamentDraws(
        this.tournamentId!
      );
      this.draws.set(reqResponse);
      await this.checkIfICanMakeDraw();
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
}
