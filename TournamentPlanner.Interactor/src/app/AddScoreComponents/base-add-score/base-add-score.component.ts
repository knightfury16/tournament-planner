import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AddMatchScoreDto, MatchDto } from '../../tp-model/TpModel';
import { AdminTournamentService } from '../../../Shared/admin-tournament.service';
import { LoadingService } from '../../../Shared/loading.service';
//- just add the form will contain the logic of the view in add score component
@Component({
  selector: 'app-base-add-score',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './base-add-score.component.html',
  styleUrl: './base-add-score.component.scss'
})
export class BaseAddScoreComponent {
  public adminService = inject(AdminTournamentService);
  public loadingService = inject(LoadingService);

  @Input({ required: true }) match?: MatchDto;
  @Output() matchTabChangeEE = new EventEmitter<void>();

  public addScoreForm = new FormGroup({});

  public async addMatchScore(gameSpecificData: any) {
    try {
      var addMatchScoreDto: AddMatchScoreDto = {
        gamePlayed: new Date().toISOString(),
        gameSpecificScore: gameSpecificData
      }

      this.loadingService.show();
      var response = await this.adminService.addMatchScore(this.match!.id.toString(), addMatchScoreDto)
      this.loadingService.hide();
      this.matchTabChangeEE.emit();

    } catch (error) {
      this.loadingService.hide();
      console.log("Failed TO ADD SCORE")
      console.log(error);
    }

  }

}
