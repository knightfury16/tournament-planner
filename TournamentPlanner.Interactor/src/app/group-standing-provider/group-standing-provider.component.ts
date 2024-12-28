import { Component, ComponentRef, inject, Input, OnInit, Signal, viewChild, ViewContainerRef } from '@angular/core';
import { GameTypeDto, PlayerStandingDto } from '../tp-model/TpModel';
import { MatCardModule } from '@angular/material/card';
import { GroupStandingProviderService } from '../../Shared/group-standing-provider.service';
import { BaseGroupStandingComponent } from '../GroupStandingComponents/base-group-standing/base-group-standing.component';
import { TournamentPlannerService } from '../tournament-planner.service';

@Component({
  selector: 'app-group-standing-provider',
  standalone: true,
  imports: [MatCardModule],
  templateUrl: './group-standing-provider.component.html',
  styleUrl: './group-standing-provider.component.scss'
})
export class GroupStandingProviderComponent implements OnInit {
  @Input({required: true}) matchTypeId?: number;
  @Input({ required: true }) gameType?: GameTypeDto | null;
  vcr: Signal<ViewContainerRef | undefined> = viewChild('gameSpecificGroupStandingContainer', {read: ViewContainerRef});
   #groupStandingComponentRef: ComponentRef<BaseGroupStandingComponent> | undefined ;
  private _gsProviderService = inject(GroupStandingProviderService);
  private _tpService = inject(TournamentPlannerService);

  async ngOnInit(){
    var component = this._gsProviderService.getGroupStaindingComponent(this.gameType!);
    this.#groupStandingComponentRef = this.vcr()?.createComponent(component);
    var playerStandings = await this.getPlayerStandings();
    this.#groupStandingComponentRef?.setInput("playerStandings",playerStandings);
  }
  async getPlayerStandings():Promise<PlayerStandingDto[] | undefined> {
    try {
      return await this._tpService.getGroupStandingOfMatchType(this.matchTypeId!.toString());
    } catch (error) {
      console.log(error);
      console.log((error as any).error ?? (error as any).error?.Error ?? 'An unknown erro occurred.');
      return;
    }
  }
}
