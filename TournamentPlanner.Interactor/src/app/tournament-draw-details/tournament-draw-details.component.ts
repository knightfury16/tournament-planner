import {
  Component,
  computed,
  EventEmitter,
  inject,
  Input,
  input,
  OnInit,
  Output,
  signal,
} from '@angular/core';
import { DrawTabViewType } from '../tournament-details-homepage/tournament-details-homepage.component';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatTabGroup, MatTabsModule } from '@angular/material/tabs';
import { CommonModule } from '@angular/common';
import { MatchTypeDto } from '../tp-model/TpModel';
import { TournamentPlannerService } from '../tournament-planner.service';
import { MatTableModule } from '@angular/material/table';
import { MatListModule } from '@angular/material/list';

@Component({
  selector: 'app-tournament-draw-details',
  standalone: true,
  imports: [
    MatIconModule,
    MatTabsModule,
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatTableModule,
    MatListModule
  ],
  templateUrl: './tournament-draw-details.component.html',
  styleUrl: './tournament-draw-details.component.scss',
})
export class TournamentDrawDetailsComponent implements OnInit {
  @Input({ required: true }) public matchTypeId?: number;
  @Output() drawTabChangeEvent = new EventEmitter<DrawTabViewType>();
  private _tpService = inject(TournamentPlannerService);

  public matchType = signal<MatchTypeDto | undefined>(undefined);
  public players = computed(() => this.matchType()?.players ?? undefined);
  public rounds = computed(() => this.matchType()?.rounds ?? undefined);

  async ngOnInit() {
    try {
      var response = await this._tpService.getMatchType(
        this.matchTypeId!.toString()
      );
      console.log('MATCHTYPE', response);
      this.matchType.set(response);
    } catch (error) {
      console.log(error);
      console.log(
        (error as any).error ??
          (error as any).error?.Error ??
          'An unknown error occurred.'
      );
    }
  }

  public emitDrawTabChangeEvent() {
    this.drawTabChangeEvent.emit(DrawTabViewType.ListView);
  }
}
