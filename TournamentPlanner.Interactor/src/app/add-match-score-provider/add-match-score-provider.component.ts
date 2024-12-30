import { Component, ComponentRef, effect, inject, Input, signal, Signal, viewChild, ViewContainerRef } from '@angular/core';
import { GameTypeDto, MatchDto } from '../tp-model/TpModel';
import { BaseAddScoreComponent } from '../AddScoreComponents/base-add-score/base-add-score.component';
import { ScoreComponentProviderService } from '../../Shared/score-component-provider.service';

@Component({
  selector: 'app-add-match-score-provider',
  standalone: true,
  imports: [],
  templateUrl: './add-match-score-provider.component.html',
  styleUrl: './add-match-score-provider.component.scss'
})
export class AddMatchScoreProviderComponent {
  @Input({ required: true }) public match = signal<MatchDto | undefined>(undefined);
  @Input({ required: true }) public gameType?: GameTypeDto | null;

  vcr: Signal<ViewContainerRef | undefined> = viewChild('gameSpecificAddScoreForm', { read: ViewContainerRef });
  #scoreFormComponentRef: ComponentRef<BaseAddScoreComponent> | undefined;
  private _addScoreProviderService = inject(ScoreComponentProviderService);

  constructor(){
    effect(() =>{
      this.#scoreFormComponentRef?.setInput("match", this.match());
    })
  }
  
  ngOnInit() {
    var component = this._addScoreProviderService.porvideGameScoreComponentBasedOnGameType(this.gameType!);
    this.#scoreFormComponentRef = this.vcr()?.createComponent(component);
  }
}
