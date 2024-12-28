import { Component, input, Input } from '@angular/core';
import { GameTypeDto, MatchDto, PlayerDto } from '../tp-model/TpModel';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-round-robin-table',
  standalone: true,
  imports: [MatCardModule],
  templateUrl: './round-robin-table.component.html',
  styleUrl: './round-robin-table.component.scss',
})
export class RoundRobinTableComponent {
  @Input({ required: true }) players?: PlayerDto[];
  @Input({ required: true }) matches?: MatchDto[];

  @Input({ required: true }) gameType?: GameTypeDto | null;



  public getTableRowData(
    rowIndex: number,
    columnIndex: number
  ): string | undefined {
    if (this.players == undefined) return;
    if (rowIndex == columnIndex) return '_';
    var firstPlayer = this.players[columnIndex];
    var secondPlayer = this.players[rowIndex];
    var match = this.getMatch(firstPlayer, secondPlayer);
    return `${secondPlayer.name} vs. ${firstPlayer.name}`;
  }
  getMatch(firstPlayer: PlayerDto, secondPlayer: PlayerDto) {
    return this.matches?.find((match) => 
      (match.firstPlayer.id == firstPlayer.id &&
        match.secondPlayer.id == secondPlayer.id) ||
      (match.firstPlayer.id == secondPlayer.id &&
        match.secondPlayer.id == firstPlayer.id)
    );
  }
}
