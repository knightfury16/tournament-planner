import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipSet, MatChipsModule } from '@angular/material/chips';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-group-seeding-dialog',
  standalone: true,
  imports: [MatCardModule, MatDialogModule, MatButtonModule, MatChipsModule, MatIconModule, CommonModule, MatSelectModule],
  templateUrl: './group-seeding-dialog.component.html',
  styleUrl: './group-seeding-dialog.component.scss'
})
export class GroupSeedingDialogComponent {
  availablePlayers: string[] = ['Playerrrrrrrrrrrrrrrrrrrrrrrrrr 1', 'Player 2', 'Player 3', 'Player 4', 'Player 5']; // Dummy data
  selectedPlayers: string[] = [];

  private dialogRef = inject(MatDialogRef<GroupSeedingDialogComponent>);

  selectPlayer(player: string) {
    this.selectedPlayers.push(player);
    this.availablePlayers = this.availablePlayers.filter(p => p !== player); // Remove from dropdown
  }

  removePlayer(player: string) {
    this.availablePlayers.push(player);
    this.selectedPlayers = this.selectedPlayers.filter(p => p !== player); // Remove from chips
  }

  closeDialog() {
    this.dialogRef.close();
  }

  confirmSelection() {
    this.dialogRef.close(this.selectedPlayers);
  }
}
