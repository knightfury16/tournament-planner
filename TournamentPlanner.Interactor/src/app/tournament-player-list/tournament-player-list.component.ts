import { Component, EventEmitter, inject, Input, OnInit, Output, signal } from '@angular/core';
import { PlayerDto } from '../tp-model/TpModel';
import { TournamentPlannerService } from '../tournament-planner.service';
import { MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';
import { PlayerTabViewType } from '../tournament-details-homepage/tournament-details-homepage.component';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-tournament-player-list',
  standalone: true,
  imports: [MatListModule, MatButtonModule, MatCardModule],
  templateUrl: './tournament-player-list.component.html',
  styleUrl: './tournament-player-list.component.scss'
})
export class TournamentPlayerListComponent implements OnInit {
  @Input({ required: true }) tournamentId?: string;
  @Output() playerTabChangeEvent = new EventEmitter<PlayerTabViewType>();

  public players = signal<PlayerDto[] | undefined>(undefined);
  public tpServive = inject(TournamentPlannerService);

  async ngOnInit() {
    var playerResponse = await this.tpServive.getTournamentPlayers(this.tournamentId!);
    this.players.set(playerResponse);
  }
  //From the list view i will always trigger the detail view
  public emitTabViewChangeEvent(): void {
    this.playerTabChangeEvent.emit(PlayerTabViewType.DetailView);
  }


}
