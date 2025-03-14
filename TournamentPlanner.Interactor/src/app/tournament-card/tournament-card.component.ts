import { Component, Input, inject } from '@angular/core';
import { GameTypeSupported, NotAvailable, TournamentDto } from '../tp-model/TpModel';
import { transformTournamentIsoDate } from '../../Shared/Utility/dateTimeUtility';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { mapStringToEnum } from '../../Shared/Utility/stringUtility';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { MatTooltipModule } from '@angular/material/tooltip';
import { TournamentImageService } from '../../Shared/tournament-image.service';

@Component({
  selector: 'app-tournament-card',
  standalone: true,
  imports: [MatIconModule, MatCardModule, MatButtonModule, MatChipsModule, RouterModule, CommonModule, MatTooltipModule],
  templateUrl: './tournament-card.component.html',
  styleUrl: './tournament-card.component.scss'
})
export class TournamentCardComponent {

  @Input({ required: true, transform: transformTournamentIsoDate }) tournament: TournamentDto | null = null;
  @Input() manage: boolean = false;
  public tournamentImageService = inject(TournamentImageService);

  getVenue() {
    var venue = this.tournament?.venue;
    if (venue == undefined || venue == null || venue == "") return NotAvailable;
    return venue;
  }

  public getTournamentImageUrl(): string {
    var gameType = this.getGameType();
    return this.tournamentImageService.getTournamentCardImageUrl(gameType);
  }

  getGameType(): GameTypeSupported | null {
    const gameType = this.tournament?.gameTypeDto?.name;
    return gameType ? mapStringToEnum(GameTypeSupported, gameType) : null;
  }

  getLink(tournamentId: number | undefined): string | any[] | null | undefined {
    if (tournamentId == null) return;
    return this.manage
      ? ['/tp/manage-tournament-homepage', tournamentId]
      : ['/tp/tournament-details-homepage', tournamentId];
  }

  getLinkDisableValue(): boolean {
    return this.manage;
  }

  getStatusColor(): string {
    const currentDate = new Date();
    const startDate = this.tournament?.startDate ? new Date(this.tournament.startDate) : null;
    const endDate = this.tournament?.endDate ? new Date(this.tournament.endDate) : null;

    if (!startDate || !endDate) return 'gray';

    if (currentDate < startDate) return '#2196F3'; // Upcoming - Blue
    if (currentDate > endDate) return '#FF5722'; // Completed - Orange
    return '#4CAF50'; // Active - Green
  }

  getStatus(): string {
    const currentDate = new Date();
    const startDate = this.tournament?.startDate ? new Date(this.tournament.startDate) : null;
    const endDate = this.tournament?.endDate ? new Date(this.tournament.endDate) : null;

    if (!startDate || !endDate) return 'Unknown';

    if (currentDate < startDate) return 'Upcoming';
    if (currentDate > endDate) return 'Completed';
    return 'Active';
  }

}
