import { CommonModule } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipSet, MatChipsModule } from '@angular/material/chips';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { TournamentPlannerService } from '../tournament-planner.service';
import { PlayerDto } from '../tp-model/TpModel';

@Component({
  selector: 'app-group-seeding-dialog',
  standalone: true,
  imports: [MatCardModule, MatDialogModule, MatButtonModule, MatChipsModule, MatIconModule, CommonModule, MatSelectModule],
  templateUrl: './group-seeding-dialog.component.html',
  styleUrl: './group-seeding-dialog.component.scss'
})
export class GroupSeedingDialogComponent implements OnInit {

  public tournamentId = inject<string>(MAT_DIALOG_DATA);
  public availablePlayers = signal<PlayerDto[]>([]);
  selectedPlayers: PlayerDto[] = [];

  private dialogRef = inject(MatDialogRef<GroupSeedingDialogComponent>);
  private _tpService = inject(TournamentPlannerService);


  async ngOnInit() {
    try {
      if (this.tournamentId) {
        console.log(this.tournamentId);
        var tournamentPlayers = await this._tpService.getTournamentPlayers(this.tournamentId);
        this.availablePlayers.set(tournamentPlayers);
      }
    } catch (error) {
      console.error(error)
    }
  }

  selectPlayer(player: PlayerDto) {
    this.selectedPlayers.push(player);
    this.availablePlayers.update(value => value.filter(p => p.id !== player.id));
    this.availablePlayers.set(this.availablePlayers().filter(p => p.id !== player.id)); // Remove from dropdown
  }

  removePlayer(player: PlayerDto) {
    this.availablePlayers.update(value => [...value, player]); // Correctly adds player back
    this.selectedPlayers = this.selectedPlayers.filter(p => p.id !== player.id); // Remove from chips
  }

  closeDialog() {
    this.dialogRef.close();
  }

  confirmSelection() {
    this.dialogRef.close(this.selectedPlayers);
  }
}
