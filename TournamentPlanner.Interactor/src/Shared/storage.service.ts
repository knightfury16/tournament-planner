import { inject, Injectable } from '@angular/core';
import { IStorageProvider } from '../StorageProviders/IStorageProvider';
import { STORAGE_PROVIDER } from '../app/app.config';
import { DefaultApplicationPrefix, DefaultExpiryMintures, StoredItem } from '../app/tp-model/types';

@Injectable({
  providedIn: 'root'
})
export class StorageService {

  public storageProvider = inject<IStorageProvider>(STORAGE_PROVIDER);



  public async get<T>(key: string): Promise<T | null>
  {
    if(!key)return null;

    try {
      var applicationSpecificKey = this.generateApplicationSpecificKey(key);
      var itemStr = await this.storageProvider.get(applicationSpecificKey);

      if(!itemStr)return null;

      const  item : StoredItem<T> = JSON.parse(itemStr);

      var isExpire = this.isExpire(item.expiry);

      if(isExpire)
      {
        await this.remove(key);
        return null;
      }

      return item.value;
      
    } catch (error) {
      console.error("ERROR PARSING TAB STATE::", error);
      return null;
    }

  }

  public async set<T>(key: string, value: T, expiryMinutes?: number): Promise<boolean>
  {
    if(!key){
      console.error("Can not store with empty key");
      return false;
    }

    try {
      var applicationSpecificKey = this.generateApplicationSpecificKey(key);
      var expiryDate = this.getExpiryDate(expiryMinutes);
      
      const storedItem: StoredItem<T> = {
        value : value,
        expiry: expiryDate
      }

      var valueToStoreStringify = JSON.stringify(storedItem);
      await this.storageProvider.set(applicationSpecificKey,valueToStoreStringify);
      return true;
      
    } catch (error) {
      console.error("Error setting date", error);
      return false;
    }

  }

  private getExpiryDate(expiryMinutes: number | null | undefined) {
    var now = new Date();
    if(expiryMinutes == null || expiryMinutes == undefined)
      return new Date(now.getTime() + DefaultExpiryMintures * 60000);  
    return new Date(now.getTime() + expiryMinutes * 60000);
  }

  public async remove(key: string): Promise<void>{
    var applicationSpecificKey = this.generateApplicationSpecificKey(key);
    await this.storageProvider.remove(applicationSpecificKey);
  }

  public async clear(): Promise<void>
  {
    await this.storageProvider.clear();
  }

  private isExpire(expiryDate: string |  Date | undefined | null): boolean
  {
    if(expiryDate == undefined || expiryDate == null)return false; //if there is no expiry date return the value
    let expiryDateTime : Date;
    try {
      if(typeof expiryDate === 'string')expiryDateTime = new Date(expiryDate);
      else expiryDateTime = expiryDate;

      var now = new Date();
      return expiryDateTime <= now;

    } catch (error) {
      console.error("ERROR PROCESSING EXPIRY DATE::", error);
      return true;
      
    }
  }

  private generateApplicationSpecificKey(key: string): string{
    return `${DefaultApplicationPrefix}-${key}`
  }
}
