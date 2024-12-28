import { Component, inject, Input, OnInit, Signal, viewChild, ViewContainerRef } from '@angular/core';
import { GameTypeDto } from '../tp-model/TpModel';
import { MatCardModule } from '@angular/material/card';
import { GroupStandingProviderService } from '../../Shared/group-standing-provider.service';

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

  private _gsProviderService = inject(GroupStandingProviderService);

  ngOnInit(): void {
    var component = this._gsProviderService.getGroupStaindingComponent(this.gameType!);
    this.vcr()?.createComponent(component);
  }
}
