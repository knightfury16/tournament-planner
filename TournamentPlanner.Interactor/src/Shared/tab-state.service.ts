import { inject, Injectable } from '@angular/core';
import { StorageService } from './storage.service';
import { DefaultExpiryMintures } from '../app/tp-model/types';

@Injectable({
  providedIn: 'root'
})
export class TabStateService {

  private _storageService = inject(StorageService);
  private readonly TabStatePrefix = "TabState";


  public async saveTabState(tabName: string, tabIndex: number, expiryMinutes: number = DefaultExpiryMintures): Promise<boolean>
  {
     return await this._storageService.set(this.getTabStatePrefix(tabName),tabIndex, expiryMinutes);
  }


  public async getTabState<T>(tabName: string): Promise<T | null>
  {
    return await this._storageService.get<T>(this.getTabStatePrefix(tabName));
  }

  private getTabStatePrefix(tabName: string):string
  {
    return `${this.TabStatePrefix}:${tabName}` 
  }

}
