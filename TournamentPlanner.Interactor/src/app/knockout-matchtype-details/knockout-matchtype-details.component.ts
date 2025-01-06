import { Component, computed, effect, Input } from '@angular/core';
import { GameTypeDto, RoundDto } from '../tp-model/TpModel';
import { MatchDto } from '../tp-model/TpModel';
import { PlayerDto } from '../tp-model/TpModel';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-knockout-matchtype-details',
  standalone: true,
  imports: [MatCardModule, MatIconModule, CommonModule, MatTooltipModule],
  templateUrl: './knockout-matchtype-details.component.html',
  styleUrl: './knockout-matchtype-details.component.scss'
})
export class KnockoutMatchtypeDetailsComponent {

  @Input({ required: true }) public players?: PlayerDto[];
  @Input({ required: true }) public rounds?: RoundDto[];

  public readonly playOff = "playoff 3/4";

  public matches = computed<MatchDto[]>(() => {
    if (!this.rounds) return [];
    return this.rounds.reduce((acc, round) =>
      round.matches ? [...acc, ...round.matches] : acc, [] as MatchDto[]);
  });

  public sortedRounds = computed(() => {
    return this.rounds
      ?.filter(round => round.roundName?.toLowerCase() !== this.playOff)
      .map(round => ({
        ...round,
        matches: round.matches?.sort((a, b) => a.id - b.id) || []
      }))
      .sort((a, b) => a.roundNumber - b.roundNumber) || [];
  });

  public playOffRound = computed(() => {
    return this.rounds?.filter(round => round.roundName?.toLocaleLowerCase() == this.playOff)[0];
  })

  isLastRound(roundIndex: number): boolean {
    return roundIndex === (this.sortedRounds()?.length || 0) - 1;
  }

  getMatchSpacing(roundIndex: number): number {
    // Base spacing multiplied by 2 for each round
    return Math.pow(2, roundIndex + 1) * 60;
  }

  getMatchWrapperTopPosition(roundIndex: number, matchIndex: number): number {
    const spacing = this.getMatchSpacing(roundIndex);
    const offset = spacing / 2;
    return matchIndex * spacing + offset;
  }

  getConnectorHeight(roundIndex: number): number {
    return Math.pow(2, roundIndex) * 60;
  }

  getVerticalLinePosition(matchIndex: number, isTop: boolean): string {
    return isTop ? '50%' : '-50%';
  }

  getPlayerSets(match: MatchDto, player: PlayerDto): number {
    if (!match.scoreJson) return 0;
    try {
      const score = JSON.parse(match.scoreJson);
      return player.id === match.firstPlayer.id ? score.Player1Sets : score.Player2Sets;
    } catch {
      return 0;
    }
  }

  public getContainerHeight() {
    const eachMatchWapperHeight = 80;
    const gap = 60;
    const maxMatch = this.players?.length! / 2;
    const offset = 10;
    var totalHeight = (eachMatchWapperHeight * maxMatch) + (gap * maxMatch) + offset;
    return totalHeight;
  }

}
