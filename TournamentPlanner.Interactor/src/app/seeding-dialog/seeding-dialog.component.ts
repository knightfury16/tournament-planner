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
  selector: 'app-seeding-dialog',
  standalone: true,
  imports: [MatCardModule, MatDialogModule, MatButtonModule, MatChipsModule, MatIconModule, CommonModule, MatSelectModule],
  templateUrl: './seeding-dialog.component.html',
  styleUrl: './seeding-dialog.component.scss'
})
export class SeedingDialogComponent implements OnInit {

  public data = inject<{ tournamentId: string, seedingDialog: string | undefined }>(MAT_DIALOG_DATA);
  public availablePlayers = signal<PlayerDto[]>([]);
  selectedPlayers: PlayerDto[] = [];

  private dialogRef = inject(MatDialogRef<SeedingDialogComponent>);
  private _tpService = inject(TournamentPlannerService);


  async ngOnInit() {
    try {
      if (this.data.tournamentId) {
        console.log(this.data.tournamentId);
        var tournamentPlayers = await this._tpService.getTournamentPlayers(this.data.tournamentId);
        this.availablePlayers.set(tournamentPlayers);
      }
    } catch (error) {
      console.error(error)
    }
  }

  public getSeedingDialog() {
    if (this.data.seedingDialog) return this.data.seedingDialog;
    return 'Seed Players'
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
    this.dialogRef.close(this.selectedPlayers.map(p => p.id));
  }
}
