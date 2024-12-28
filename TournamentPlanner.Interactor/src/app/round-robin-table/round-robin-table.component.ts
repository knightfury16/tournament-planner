import { Component, inject, input, Input } from '@angular/core';
import { GameTypeDto, MatchDto, PlayerDto } from '../tp-model/TpModel';
import { MatCardModule } from '@angular/material/card';
import { GameTypeService } from '../../Shared/game-type.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-round-robin-table',
  standalone: true,
  imports: [CommonModule, MatCardModule],
  templateUrl: './round-robin-table.component.html',
  styleUrl: './round-robin-table.component.scss',
})
export class RoundRobinTableComponent {
  @Input({ required: true }) players?: PlayerDto[];
  @Input({ required: true }) matches?: MatchDto[];

  @Input({ required: true }) gameType?: GameTypeDto | null;

  private _gameTypeService = inject(GameTypeService);



  public getTableRowData(
    rowIndex: number,
    columnIndex: number
  ): string | undefined {
    if (this.players == undefined) return;
    if (rowIndex == columnIndex) return '_';
    //!! remember this order
    var firstPlayer = this.players[columnIndex];
    var secondPlayer = this.players[rowIndex];
    var match = this.getMatch(firstPlayer, secondPlayer);
    var shouldIFlipScore = this.GetShuoulIFlip(match, firstPlayer, secondPlayer);
    var displayData = this.getDisplayData(match,shouldIFlipScore);
    return displayData;
  }
  GetShuoulIFlip(match: MatchDto | undefined, firstPlayer: PlayerDto, secondPlayer: PlayerDto) {
    if(match?.firstPlayer.id == firstPlayer.id)return false;
    return true;
  }
   
  getDisplayData(match: MatchDto | undefined, flipScore: boolean) {
    if(this.gameType == null || match?.winner == null || match.winner == undefined) return "Not Played"
    return this._gameTypeService.getDisplayScore(this.gameType,match.scoreJson!,flipScore)
  }
  getMatch(firstPlayer: PlayerDto, secondPlayer: PlayerDto) {
    return this.matches?.find((match) => 
      (match.firstPlayer.id == firstPlayer.id &&
        match.secondPlayer.id == secondPlayer.id) ||
      (match.firstPlayer.id == secondPlayer.id &&
        match.secondPlayer.id == firstPlayer.id)
    );
  }

  public getTableClass(rowIndex: number, columnIndex: number): string | undefined{
    if (this.players == undefined) return;
    //!! remember this order
    var firstPlayer = this.players[columnIndex];
    var secondPlayer = this.players[rowIndex];
    var match = this.getMatch(firstPlayer, secondPlayer);
    if(match?.winner == null || match?.winner == undefined)return 'not-played';
    if(match.winner.id == firstPlayer.id)return 'won';
    return 'lost'
    
  }
}
