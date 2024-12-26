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
  @Output() drawTabChangeEvent = new EventEmitter<DrawTabViewType>();
  @Output() drawId = new EventEmitter<number>();
  private _tpService = inject(TournamentPlannerService);
  public draws = signal<DrawDto[] | undefined>(undefined);

  async ngOnInit() {
    try {
      var reqResponse = await this._tpService.getTournamentDraws(
        this.tournamentId!
      );
      console.log(reqResponse);
      this.draws.set(reqResponse);
      console.log(this.draws());
    } catch (error) {
      console.log(error);
      console.log(
        (error as any).error ??
          (error as any).error?.Error ??
          'An unknown error occurred.'
      );
    }
  }

  getClassName(matchType: MatchTypeDto): string {
    if (matchType.isCompleted) return 'chip-completed';

    return 'chip-incompleted';
  }

  getCreatedAtDate(draw: DrawDto): string {
    return getDateStringInFormat(new Date(draw.createdAt!));
  }

  emitDrawTabViewChangeEvent(drawId: number) {
    this.drawTabChangeEvent.emit(DrawTabViewType.DetailView);
    this.drawId.emit(drawId);
  }
}
