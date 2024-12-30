import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatchDto } from '../../tp-model/TpModel';
//- just add the form will contain the logic of the view in add score component
@Component({
  selector: 'app-base-add-score',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './base-add-score.component.html',
  styleUrl: './base-add-score.component.scss'
})
export class BaseAddScoreComponent {

  @Input({required: true}) match?: MatchDto;
  @Output() matchTabChangeEE = new EventEmitter<void>();

  public addScoreForm = new FormGroup({});

}
